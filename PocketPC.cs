using MelonLoader;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;
using Il2CppInterop.Runtime;
using System.IO;
using System.Text.Json;

[assembly: MelonInfo(typeof(PocketPC.PocketPC), "PocketPC", "1.2.0", "PocketPC")]
[assembly: MelonGame("Data Center")]

namespace PocketPC
{
    public class ConfigData
    {
        public string ToggleKey { get; set; } = "T";
    }

    public class PocketPC : MelonMod
    {
        private static Key _tabletKey = Key.T;
        private static bool _isTabletOpen = false;
        private static Il2Cpp.ComputerShop _computerShopInstance;
        private static bool _inputSystemReady = false;
        private static bool _computerShopFound = false;
        private static float _searchTimer = 0f;
        private static float _searchInterval = 1f;

        // Config paths
        private static string _configDir;
        private static string _configPath;
        private static ConfigData _config;

        public override void OnInitializeMelon()
        {
            // Setup config directory in user home: ~/Mods/PocketPC/
            string userHome = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            _configDir = Path.Combine(userHome, "Mods", "PocketPC");
            _configPath = Path.Combine(_configDir, "config.json");

            // Load or create config
            LoadConfig();

            // Parse the key
            ParseToggleKey();

            LoggerInstance.Msg($"PocketPC Mod v1.2.0 loaded!");
            LoggerInstance.Msg($"Press {_config.ToggleKey} to open the tablet anywhere.");
            LoggerInstance.Msg($"Config file: {_configPath}");
        }

        private void LoadConfig()
        {
            try
            {
                // Create directory if it doesn't exist
                if (!Directory.Exists(_configDir))
                {
                    Directory.CreateDirectory(_configDir);
                    LoggerInstance.Msg($"Created config directory: {_configDir}");
                }

                // Load existing config or create default
                if (File.Exists(_configPath))
                {
                    string json = File.ReadAllText(_configPath);
                    _config = JsonSerializer.Deserialize<ConfigData>(json);
                    if (_config == null)
                    {
                        _config = new ConfigData();
                        SaveConfig();
                    }
                    LoggerInstance.Msg("Config loaded successfully.");
                }
                else
                {
                    // Create default config
                    _config = new ConfigData();
                    SaveConfig();
                    LoggerInstance.Msg($"Created default config at: {_configPath}");
                }
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Warning($"Config error: {ex.Message}. Using defaults.");
                _config = new ConfigData();
            }
        }

        private void SaveConfig()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_config, options);
                File.WriteAllText(_configPath, json);
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Warning($"Failed to save config: {ex.Message}");
            }
        }

        private void ParseToggleKey()
        {
            string keyString = _config.ToggleKey.Trim().ToUpper();

            if (System.Enum.TryParse<Key>(keyString, out Key parsedKey))
            {
                _tabletKey = parsedKey;
            }
            else
            {
                LoggerInstance.Warning($"Invalid key '{keyString}' in config. Using default 'T'.");
                _tabletKey = Key.T;
            }
        }

        public override void OnUpdate()
        {
            if (!_inputSystemReady)
            {
                try
                {
                    if (Keyboard.current != null)
                    {
                        _inputSystemReady = true;
                    }
                }
                catch { }
            }

            if (!_computerShopFound)
            {
                _searchTimer += Time.deltaTime;
                if (_searchTimer >= _searchInterval)
                {
                    _searchTimer = 0f;
                    FindComputerShop();
                }
                return;
            }

            if (!_inputSystemReady || _computerShopInstance == null) return;

            try
            {
                var keyboard = Keyboard.current;
                if (keyboard == null) return;

                if (keyboard[_tabletKey].wasPressedThisFrame)
                {
                    ToggleTablet();
                }

                if (_isTabletOpen && keyboard[Key.Escape].wasPressedThisFrame)
                {
                    CloseTablet();
                }
            }
            catch { }
        }

        private void FindComputerShop()
        {
            try
            {
                var shop = GameObject.FindObjectOfType<Il2Cpp.ComputerShop>();
                if (shop != null)
                {
                    _computerShopInstance = shop;
                    _computerShopFound = true;
                }
            }
            catch { }
        }

        private void ToggleTablet()
        {
            if (_computerShopInstance == null) return;

            if (_isTabletOpen)
            {
                CloseTablet();
            }
            else
            {
                OpenTablet();
            }
        }

        private void OpenTablet()
        {
            if (_computerShopInstance == null) return;

            try
            {
                // Call InteractOnClick which properly initializes the UI including cursor
                var interactMethod = _computerShopInstance.GetType().GetMethod("InteractOnClick",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (interactMethod != null)
                {
                    interactMethod.Invoke(_computerShopInstance, null);
                    _isTabletOpen = true;
                }
                else
                {
                    // Fallback: activate canvas directly
                    var canvas = _computerShopInstance.canvasComputerShop;
                    if (canvas != null && !canvas.activeSelf)
                    {
                        canvas.SetActive(true);
                        if (_computerShopInstance.mainScreen != null)
                            _computerShopInstance.mainScreen.SetActive(true);

                        _isTabletOpen = true;
                    }
                }
            }
            catch { }
        }

        private void CloseTablet()
        {
            if (_computerShopInstance == null)
            {
                _isTabletOpen = false;
                return;
            }

            try
            {
                _computerShopInstance.CloseShop();
                _isTabletOpen = false;
            }
            catch
            {
                _isTabletOpen = false;
            }
        }

        public static bool IsTabletOpen()
        {
            return _isTabletOpen;
        }

        public static void SetComputerShop(Il2Cpp.ComputerShop instance)
        {
            _computerShopInstance = instance;
            _computerShopFound = true;
        }

        public static ConfigData Config { get { return _config; } }
    }

    // Harmony patch to capture ComputerShop instance when it spawns
    [HarmonyPatch(typeof(Il2Cpp.ComputerShop), "Awake")]
    public class ComputerShopAwakePatch
    {
        static void Postfix(Il2Cpp.ComputerShop __instance)
        {
            if (__instance != null)
            {
                PocketPC.SetComputerShop(__instance);
            }
        }
    }
}

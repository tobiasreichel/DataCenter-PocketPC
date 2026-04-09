using MelonLoader;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;
using Il2CppInterop.Runtime;

[assembly: MelonInfo(typeof(PortableTabletMod.PortableTabletMod), "PortableTablet", "1.0.0", "PortableTablet")]
[assembly: MelonGame("Data Center")]

namespace PortableTabletMod
{
    public class PortableTabletMod : MelonMod
    {
        private Key _tabletKey = Key.T;
        private static bool _isTabletOpen = false;
        private static Il2Cpp.ComputerShop _computerShopInstance;
        private static bool _inputSystemReady = false;
        private static bool _computerShopFound = false;
        private static float _searchTimer = 0f;
        private static float _searchInterval = 1f;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Portable Tablet Mod v1.0.0 loaded!");
            LoggerInstance.Msg("Press T to open the tablet anywhere.");
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
    }

    // Harmony patch to capture ComputerShop instance when it spawns
    [HarmonyPatch(typeof(Il2Cpp.ComputerShop), "Awake")]
    public class ComputerShopAwakePatch
    {
        static void Postfix(Il2Cpp.ComputerShop __instance)
        {
            if (__instance != null)
            {
                PortableTabletMod.SetComputerShop(__instance);
            }
        }
    }
}

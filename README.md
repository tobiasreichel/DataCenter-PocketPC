# PocketPC for Data Center

<p align="center">
  <img src="https://img.shields.io/badge/version-1.2.0-blue.svg" alt="Version 1.2.0">
  <img src="https://img.shields.io/badge/game-Data%20Center-green.svg" alt="For Data Center">
  <img src="https://img.shields.io/badge/melonloader-0.6.x-orange.svg" alt="MelonLoader 0.6.x">
  <img src="https://img.shields.io/badge/license-MIT-brightgreen.svg" alt="MIT License">
</p>

A **MelonLoader mod** for the game [Data Center](https://store.steampowered.com/app/3069460/Data_Center/) that allows you to use the computer interface from anywhere in the game world. No more walking back to the computer station to order equipment or check your finances!

---

## 🎮 Features

- 📱 **Portable Computer** - Access the full OS interface from anywhere in the data center
- 🛒 **Shop on the Go** - Order servers, switches, and equipment while walking around
- 📊 **Quick Finance Check** - View Balance Sheet instantly
- 👥 **Hire Anywhere** - Recruit technicians without returning to the office
- 🌐 **Network Map** - Check your network topology from any location
- 🖱️ **Full Cursor Support** - Proper mouse cursor appears automatically
- ⚡ **Quick Toggle** - Press `T` to open, `ESC` to close

---

## 📥 Installation

### Prerequisites
- **Data Center** (Steam version)
- **MelonLoader** 0.6.x or later ([Download here](https://melonwiki.xyz/))

### Quick Install

1. Download the latest `PocketPC.dll` from [Releases](../../releases)
2. Copy the DLL file to your game's Mods folder:
   ```
   C:\Program Files (x86)\Steam\steamapps\common\Data Center\Mods\
   ```
3. Launch the game through MelonLoader

### Verification
If installed correctly, you'll see this message in the MelonLoader console:
```
[PortableTablet] Portable Tablet Mod v1.0.0 loaded!
```

---

## ⌨️ Controls

| Key | Action |
|-----|--------|
| **T** (default) | Open/Close tablet toggle |
| **ESC** | Close tablet (alternative) |

### Changing the Hotkey

The hotkey can be customized via the mod's config file:

1. Navigate to `~/Mods/PocketPC/` in your user home directory
2. Open `config.json` in a text editor
3. Change `"ToggleKey"` to your desired key (e.g., `"Tab"`, `"P"`, `"Insert"`, etc.)
4. Save and restart the game

**Example:**
```json
{
  "ToggleKey": "Tab"
}
```

**Config Location:**
- Windows: `C:\Users\[YourUsername]\Mods\PocketPC\config.json`
- Linux: `~/Mods/PocketPC/config.json`

Valid keys are any [Unity Input System Key names](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.Key.html) (case insensitive).

The config file is created automatically on first launch if it doesn't exist.

---

## 🔧 Building from Source

### Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- Data Center installed with MelonLoader

### Build Instructions

```bash
# Clone the repository
git clone https://github.com/yourusername/PocketPC.git
cd PocketPC

# Build in Release mode
dotnet build -c Release

# Copy to game (adjust path as needed)
copy "bin\Release\net6.0\PocketPC.dll" \
     "C:\Program Files (x86)\Steam\steamapps\common\Data Center\Mods\"
```

---

## 🧰 Troubleshooting

### Mod doesn't load
- Ensure MelonLoader is properly installed
- Check that you're using the correct game version
- Verify the DLL is in the `Data Center/Mods/` folder, not a subfolder

### Tablet opens but no cursor
- This is usually resolved by the mod automatically
- If issues persist, try clicking on the computer normally first, then using the mod

### Game crashes when opening tablet
- Make sure you're using the latest mod version
- Check that other mods aren't conflicting
- Disable other mods temporarily to test

### Key T doesn't work
- Check if another mod is using the T key
- You can modify the key in the mod source code and rebuild

---

## 🤝 Compatibility

| Component | Version | Status |
|-----------|---------|--------|
| Game | Data Center (Steam) | ✅ Fully compatible |
| MelonLoader | 0.6.x | ✅ Fully compatible |
| DHCPSwitches | Any | ✅ Compatible |
| RackBuilderMod | Any | ✅ Compatible |
| TexasSizedTrolley | Any | ✅ Compatible |
| DataCenter-MoreModules | Any | ✅ Compatible |

---

## 📁 Project Structure

```
PocketPC/
├── PocketPC.cs          # Main mod source
├── PocketPC.csproj      # Project configuration
├── .gitignore                      # Git ignore rules
├── LICENSE                         # MIT License
└── README.md                       # This file
```

---

## 📝 How It Works

The mod uses **Harmony** patches to hook into the game's `ComputerShop` class. When you press `T`, it calls `InteractOnClick()` - the same method the game uses when you physically click on the computer in-game.

This approach ensures:
- ✅ Full UI functionality
- ✅ Proper cursor initialization
- ✅ No conflicts with game systems
- ✅ Works with save games

---

## 📜 Version History

### v1.2.0 (Current)
- Custom config system at `~/Mods/PocketPC/config.json`
- JSON-based configuration
- Cleaner config format

### v1.1.0
- Added configurable hotkey via MelonPreferences
- Press T (or your configured key) to open tablet
- Config was saved in `UserData/MelonPreferences.cfg`

### v1.0.0
- Initial release
- Portable tablet functionality (T key)
- Proper cursor support via InteractOnClick()
- Support for all ComputerShop screens
- Harmony-based ComputerShop detection

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Credits

- Created for **Data Center**
- Uses [MelonLoader](https://melonwiki.xyz/) framework
- Built with [Harmony](https://github.com/pardeike/Harmony) patching library

---

## 💡 Contributing

Contributions are welcome! Feel free to:
- Report bugs
- Suggest features
- Submit pull requests

---

<p align="center">
  Made with ❤️ for Data Center players
</p>

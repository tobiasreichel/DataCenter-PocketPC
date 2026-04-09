# Portable Tablet Mod for Data Center

A MelonLoader mod for the game **Data Center** that allows you to use the computer interface from anywhere in the game world.

## Features

- **Open tablet anywhere** - Press `T` to access the computer UI from any location
- **Full functionality** - Shop, Balance Sheet, Asset Management, Hiring, Network Map all work
- **Proper cursor support** - Mouse cursor appears automatically when using the tablet
- **Quick close** - Press `ESC` to close the tablet

## Requirements

- [MelonLoader](https://melonwiki.xyz/) 0.6.x or later
- Data Center (Steam)

## Installation

1. Download `PortableTabletMod.dll` from the [Releases](../../releases) page
2. Copy `PortableTabletMod.dll` to `Data Center/Mods/`
3. Launch the game through MelonLoader

## Building from Source

### Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- MelonLoader installed in your Data Center game directory

### Build Steps

```bash
# Clone the repository
git clone https://github.com/yourusername/PortableTabletMod.git
cd PortableTabletMod

# Build the mod
dotnet build -c Release

# The mod DLL will be in bin/Release/net6.0/
# Copy it to your game:
cp bin/Release/net6.0/PortableTabletMod.dll \
   "C:/Program Files (x86)/Steam/steamapps/common/Data Center/Mods/"
```

## Usage

| Key | Action |
|-----|--------|
| **T** | Open/Close tablet |
| **ESC** | Close tablet (when open) |

## How It Works

The mod hooks into the `ComputerShop` component and calls `InteractOnClick()`, the same method used when clicking on the computer in-game. This ensures proper initialization including cursor visibility.

## Project Structure

```
PortableTabletMod/
├── PortableTabletMod.cs       # Main mod code
├── PortableTabletMod.csproj     # Project file
├── .gitignore                   # Git ignore rules
└── README.md                    # This file
```

## Compatibility

- **Game:** Data Center
- **MelonLoader:** 0.6.x or later
- **Works with:** Other mods (tested with DHCPSwitches, RackBuilderMod, TexasSizedTrolley, DataCenter-MoreModules)

## License

MIT License - feel free to use, modify, and distribute.

## Credits

Created for Data Center

# Hot Lava Archipelago

This plugin exists to integrate Hot Lava with the [Archipelago](https://archipelago.gg/) multiword randomizer software.

## Configuration

To connect to Archipelago, you will need to provide the room URL, the player/slot name, and the room password. This can be done in 1 of 2 ways:

### Config File

After downloading the mod, and launching the game in modded mode, a config file for the plugin should be generated in %AppData%\Thunderstore Mod Manager\DataFolder\HotLava\profiles\Default\BepInEx\config. Open this file up in any text editor and update the values as necessary.

### Command

When connecting to the room, provide the necessary values using the following command format: `/apconnect [<roomUrl>] [<playerName>] [<password>]` (Example: `/apconnect archipelago.gg:38281 Bongo9911 testPassword123`)
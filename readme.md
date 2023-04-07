# Midi-As-Keyboard

Mapping midi input to keyboard, Windows only.

## Usage

Custom your `config.json`, and run `dotnet run` in the root directory.

The parameters:
+ `playback`: whether to play the sound;
+ `patch`: the patch of the sound, see [here](https://www.midi.org/specifications/item/gm-level-1-sound-set);
+ `mappings`: see the file `Keys.cs` for the key codes. Supports:
  + Single Key: `Esc` (Simulate press and release);
  + Multiple Keys: `Ctrl+Shift+Alt+Tab` (Simulate press and release);
  + Sequence: `LWin+D Alt+F4 Return` (Execute at once).

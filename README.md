![Achievement get! RTFM](https://issotm.github.io/achievement.png "Good job! Your read the fucking manual!")

# AeviMap

AeviMap is a map editor for Aevilia, an in-development homebrew GB game. It's fairly crude, but at least it works.

The tool is only intended to be a companion for the source code of the game, since it's doesn't edit the ROM but generates a .blk file.
(Insert GitHub repo link here when one is finally created)

**Table of contents**
* [How to use this repo](#how-to-use-this-repo)
* [AeviMap's User Manual](#aevimaps-user-manual)
  * [Before you start](#before-you-start)
  * [Using AeviMap](#using-aevimap)
* [Troubleshooting](#halp-it-doesnt-work11)
* [Contact](#contact)


# How to use this repo

This repo has been created using Visual Studio. If you have Visual Studio and the GitHub extension, you can clone this repository and you're all set ! 👌

If you don't have the GitHub extension, you can simply clone the repo and load the .sln file in Visual Studio.


If you're not using Visual Studio look up Google to import the project. (I'm a noob at C# so I can't help you here)


# AeviMap's User Manual

## Before you start

### ROM

AeviMap requires, obviously, a ROM of Aevilia GB. Its current filename is `aevilia.gbc` (Note : by default, the ROM loading dialog will only accept `.gbc` files.)

The ROM's standard size is 128 kB (kilobytes), if it's not it's a bit weird. Re-download the ROM.

### INI file

AeviMap also requires a couple variables, mostly pointers, to know where in the ROM it's supposed to draw data from. That's the purpose of the `AeviMap.ini` file.

The INI file contains info for a specific build of Aevilia, since data tends to move around across builds. You can extract the info required from the ROM, but I will stash INI files for public releases of Aevilia here.

The INI file should be in the same folder as `AeviMap.exe`, but it can be in any directory in your PATH. (If you don't know what this means, just stick `AeviMap.ini` next to `AeviMap.exe`.)

If AeviMap can't find a INI file, it's will instead generate one using some default values in its memory. They are almost certainly wrong :3


## Using AeviMap

To use AeviMap, you must first load the ROM file. Once it's done, the rest of the functionalities become available. (If you try skipping this step, you'll be told to load a ROM first)

Then, to start working on a map, you can either load it from the ROM (select the map's ID on "Select map" then click "Load"), or load a preexisting .blk file.

### Loading an existing .blk file

This functionality allows you to resume work done on a map without starting from scratch.

Note that .blk files only contain block data, not the map's header. Thus, AeviMap uses the header of the map whose ID is in the "Select map" box for the metadata. The header is taken from the ROM.

It's not required that said map is loaded (if map #1 is loaded but the box indicates "0", AeviMap will use map #0's header)

If the .blk file's size doesn't match the expected size (map's width * map's height), AeviMap will complain. If this happens, you're probably loading the wrong map.

### Editing the map

The left panel is a map viewer, the right panel is a block picker.

Left-click a block in the block picker to select it, left-click in the map viewer to replace the hovered block with the selected one.

Right-click a block in the map viewer to select it without going through the block picker.


# HALP IT DOESNT WORK!!!11

## "Unhandled exception" or something

If you see this "unhandled exception" I recommend that you choose "Quit" instead of "Continue". Normally this shouldn't pop up, but when it does it'll probably be critical.

If you choose "Quit", AeviMap should still ask you to save your changes if you didn't save them. If the box popped up while you were saving changes, it means that for some reason it's impossible to do so. Sorry :/

Anyways; if this happens I recommend you open the "More info" panel, screenshot everything, and write me a mail including the screenshot and telling what you were doing (ie. what you clicked) when it popped up.


## "Attempted to read X bytes from YYYYY (decimal), which is impossible."

See below.

## "Attempted to read 0 bytes, which is impossible."

Somehow an incorrect number of bytes was (attempted to be) read from the ROM. AeviMap will close, but still ask you to save unsaved changes beforehand. (Note : don't click Cancel. It's, like, a VERY bad idea)

This usually happens because your INI file is wrong.


# Contact

ISSOtm : issotm.dev@gmail.com
(Include "AeviMap" or "Aevilia" in the title, otherwise it's going straight into the "Spam" folder.)

Kai : "nope :D"

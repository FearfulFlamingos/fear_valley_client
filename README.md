# FEAR VALLEY
## Introduction
*Fear Valley* is the name of the computer game we have created as our senior project. It is written in C# and implemented with the Unity engine. 

We decided to use the Unity engine for a few reasons:
* Previous familiarty
* Known ease of entry
* Far and away the most popular free game engine for professionals and individuals alike


## Installation
Currently, you can clone this repo and open the project through the Unity hub using version 2018.4.9. We decided to use the latest LTS version of unity. After opening the project, you can build the project using the unity editor.

At this time, we have no precompiled binary installation files.

### Installing and running the server
Download both the [server]() and [client]() binaries for your platform. You'll need to open port \#50000 on your computer for inbound and outbound connections. Afterwards, run the server and take note of your IP address. 

If you're running a LAN game, when you launch the client afterwards enter `127.0.0.1` for the IP address and `50000` for the port. The person you are playing against will need your IP address to connect.


### Installing the client
Download the [precompiled binary]() for your platform. It is a portable app, so run it anywhere.

After starting the client, enter the IP address and port of the server you'll be connecting to.

## Feature Timeline
This is our current timeline for project development:
* 12/2019: Rudimentary networking for LAN PvP
* 1/2020: Combat will be smooth
* 2/2020: Production assets in use; UI + models
* 3/2020: Rudimentary sound design
* 4/2020: Final polishing touches
* 5/2020: Project will be presented

## Tools Used
* Unity Engine
* Unity Tests
* Mono SQLite library
* Unity low-level networking API

## Miscellenea
* Want to play the tabletop version of this game? Click [here]() to download the rules.
* Create by Nathan Schulte ([@schuna05](https://www.github.com/schuna05)) and Gavin Lochtefeld ([@lochga01](https://www.github.com/lochga01)), Luther College class of 2020.
# Hoarse Dungeon Crawler

![Main menu](README_Pictures/Main_Menu.png)

## Overview

This console dungeon crawler game takes the player on a journey down a dark and dangerous dungeon.

## Features

These are the main features of the game:

### Explore and battle

Discover the dungeon - one step at a time.

![Playing the game](README_Pictures/PlayingBoard.png)

Fight two different kinds of monsters. Animate your dice throws.

![Discover the extent of the dungeon](README_Pictures/A_Game_Of_Discovery.png)

### Saving/Loading

Save your game and load it at a later time via connection to MongoDB.

#### Keeping the player up to date with what's going on:

![Keeping the user up to date](README_Pictures/User_Notifications.png)

#### Error handling.

![Keeping the user up to date](README_Pictures/Error_Handling.png)

### Menu and log

Take a look at your Main and In-Game menus - "Did you say TWO menus?!" 

![In game menu](README_Pictures/In_Game_Menu.png)

What happened 5 turns ago? Check your log!

![Game log](README_Pictures/Log.png)

### Features under development

- Actual win condition
- More than one level

"Should have worked these out before animating dice throws" you say!?

You are right.


## Code talk

This is a continuation of a school project I made during my first course of programming school.
The assignment this time was to use MongoDB for saving and loading games and also introducing a
log.

This version I've been putting some effort into introducing the Game, DatabaseHandler, ConsoleWriter 
and more classes, moving some stuff that was handled in weird places in the first version of the app.

I made a choice not to make a rehaul of the actual game loop and it's workings although it could definitely use some work :-)

### Code examples

All connections with the database have some form of error handling:

![Error_Handling](README_Pictures/Trying_Catching.png)

### Using events as a means of communication between level elements and the Game

![Logging](README_Pictures/Logging_Via_Event.png)


## Setup

To run the app you will need to have access to a MongoDB server.

You'll also need to set up user secrets for your connection string to the database like:

{ "ConnectionString" : "Your connection string"" }

## Enjoy!

/hoarse

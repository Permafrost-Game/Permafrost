# Permafrost Game
Permafrost is a stratergy game made with cross platform MonoGame.

This game is made for Team 2's submission for CS2010.

This game is currently in development and will be relesed in March 2020 for Windows, Linux, and Mac OS.

Pre-release versions can be [downloaded here](https://github.com/Permafrost-Game/Permafrost/releases/latest).

## Concept
Permafrost is a single player strategy game where a player controls a groupof characters with the aim to manage resources to achieve survival. Our concept is, top down colony management and survival game set in a distant future. The player will be expected to use gameplay mechanics to manage resources within the colony. Progression will be met with increased difficulty with random enemy encounters and changes to weather conditions.
## Story
Permafrost is set in a post-apocalyptic future. In an attempt to save the world from global warming, humanity built a global system of cooling towers. This however, allowed the AI controlling the towers to overthrow humanity by freezing the earth. The gameis set after these events in a hostile frozen wasteland, with humankind fighting for survival. From there the emergent story will be the focus as every game decision is up to the player.The game will start with an introductory cutscene explaining the events leading the present day.
## Graphics
Permafrost will be a 2D top down 3/4 view game. This is sometimes called the “RPG View” and we feel is best suited for this type of strategy game. The game will feature an overlay UI along the edges of the screen and will support UI elements such as drop-down menus anywhere on the screen. Each tile will be a 64x64 texture with muted colours to fit the game’s atmosphere. The game will contain a number of different textures for multiple different terrain types.

The graphics of the game will be reflective of the game’s atmosphere and will give the player the sense of the cold and warm that the colonists encounter in different scenarios. Warmer regions will have warmer colours, Colder areas will have colder and desaturated colours. Other effects such as vignette, overlaid precipitation particle effects will be used to add to the atmosphere of the game.
# Gameplay
The world will consist of procedurally generated zones. Each zone is of a fixed size of 50x50 tiles. Each tile will have one of many terrain types. Interactable objects are located in each zone, each with a different probability of appearing in a given zone.

An Interactable object is an object that exists in the game world and can be interacted with by colonists. Interacting with an object this way will provide a resource such as food. Some interactable will require a specific item or tool to be interacted with in certain ways.

Examples of some non-moveable interactables are:
  - Trees
  - Bushes
  - Wild grass
  - Stashes or storage boxes

Interactables may have multiple options for which actions can be performed. eg.a Tree could be burned, chopped down, tapped, or its fruit harvested.

Interactable objects can also moving. For example
  - Passive animals; rabbits, pigs, sheep, etc.
  - Hostile enemies; Polar bears, Robots, Aggressive Humans etc. 
Hostile enemies will beencountered frequently in many zones that the player may explorer. Each zone has a chance of containing a number of both passive and hostile creatures
## Colonists
Colonists interact with interactable objects. Colonists can pathfind and move around the map to reach a goal. They can perform actions and can be queued with multiple and repeating instructions. The player must instruct colonists with instructions. Each instruction will be between a Colonist and an Interactable. Colonists will also have an inventory for storing resources such as food, and equipment such as clothes and tools.
The game will feature several types of colonists, each with their own specialist task.
The three classes of colonist are Hunter, Builder, and Soldier.

## Buildings
Buildings are a type of interactable object, they will provide unique actions such as crafting, heat production, and resource storage. Buildings will be crafted by colonists and require a certain amount of resources to build.

Examples of buildings and their instructions are:

| Building Type | Instructions |
| ------------- | ------------ |
| Farm          | Sow, Harvest |
| Kitchen/ Oven | Cook meal, Butcher Animal |
| Camp fire     | Cook meal, Fuel |
| Storage room  | Store Resource, Collect Resource |

Excluded from this table are the instructions for Build and Demolish which all buildings will share.

Some buildings may have the same function as others, but perform tasks faster, or require different / less resources. This allows for upgraded building types and colonist progression. Eg Campfire --> Fireplace --> Oven.

## Towers
Towers play a large role in the overarching goal for the player. Around the game world there will be many zones that contain acooling tower. This tower will be guarded by AI Robots. The tower can be taken over and claimed by your colonists, converting the tower into a heating tower. It will require significant progression and resources for this to happen and is a high-risk endeavour. Once the tower is taken over by the colonist, the temperature of nearby tiles and neighbouring zones will be affected. Allowing the player to colonise other zones and explore the world further.

We intend to design the game to be played in a number ofways. A player might want to setup a base around a single tower, and then perform short explorations into other zones to gather resources. Or the player could have multiple small bases in different zone to allow for exploration over a further distance. A more nomadic method maybe favourable however not necessary. The game will be balanced in a way to give the player options for how they want to play the game.

## Controls
The game will have threemain control schemes:
1. Mouse click and drag controls for panning the camera. UI elements for cycling between colonists and issuing instructions.
2. WASD for panning camera, Hotkeys for cycling through colonists. Hotkeys for issuing instructions and to cycle between buildings.

All keyboard keys and mouse behaviour will be customisable through the options menu.

Instructions will be given to colonists through the control schemes mentioned above. UI elements will be used to allow for selection of different actions, colonists, and interactable (buildings, animals, trees etc.)

The intended control experience sees the player utilizing primarily the mouse input options butproviding keyboard shortcuts and options for more experienced players who want a quicker method for interacting with the game.

We are not prioritising controller support since we don’t believe that it is a good match for gameplay. However it may be a feature that we rereview later on in development.

HOW TO ADD A SUB-GOD

THINGS YOU HAVE TO DO:
-Create a subclass of Season_SubGod
-Override its GetName() function to include your Sub-God's name
-Create some powers that inherit from P_Season_SubGodPower
-Add them to "powers" in your Sub-God's constructor
-Add some integers to "powerLevelReqs" in your Sub-God's constructor reflecting the number of seal breaks required to use them
-Add an instance of your Sub-God to activeSubGods in God_Season's setup() function
-Overwrite the DLL in Paste Contents Into OptionalData -> SeasonGod

THINGS YOU SHOULD PROBABLY DO, BUT I'M NOT YOUR DAD:
-Add at least one power to your Sub-God's bonusPowers list (and appropriate ints to bonusPowerLevelReqs) to help players out if they random into your Sub-God
-Override GetKeywords() with a couple words so people selecting your god will know what they're getting into
-Add a custom portrait for your Sub-God to Paste Contents Into OptionalData -> SeasonGod, and override GetSpritePath() to point to it ("comseason.[whatever your filename is]")
-Add a custom "Sub-God has Activated" event to the same folder and override GetEventPath() to point to it (exactly the text under "id" for your event JSON)
-Add an entry in mod_config.json giving people an option to enable/disable your god
-Add a parameter for your god to Kernel_Season and edit receiveModConfigOpts_book to write to it
-Change God_Season's setup function so that your Sub-God is only added if enabled

THINGS YOU CAN DO IF YOU WANT:
-Add a custom "Sub-God has Activated with Bonus" event and override GetEventPathBonus() to have that event run instead of the one in GetEventPath() if the player randoms into your Sub-God
-Override GetAwakeningMessage() for unique text if your Sub-God is active when the last seal breaks
-Override GetVictoryMessage() for unique text if your Sub-God is active when you win
-Override OnActivate() to make something happen when your god becomes active (all the time or just when the player randoms into it)
-If you do, please also override UndoActivation so the player can undo the OnActivate() changes if they decide they don't want to play as your god after all before they do anything
-Override OnDeactivate() to make something happen when your Sub-God becomes inactive
-Override TurnTick_Active() if you want something to happen every turn your Sub-God is active

THINGS YOU PROBABLY SHOULDN'T DO UNLESS YOU'RE REALLY SURE:
-Override OnSubGodTransition to make something happen when Sub-Gods swap in a way that doesn't involve your Sub-God
-Override TurnTick_Inactive to make something happen every turn your Sub-God is inactive

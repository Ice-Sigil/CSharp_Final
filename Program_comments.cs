// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
//uh oh lmao I need to revoke access to that
// one mo
//think that did it?
//So i have framework 6.0 and 8.0, says I need 7.0 ugggghghghSKGhszlduhgzsurghzsjfglzsrgi -DANTE 
// Alright i got it to build the project, jack if you could modify the folder 
// to display to the terminal instead of the debug console that would be sick 


//note: he said the important part is in 5.1


//we can have a map so that if we have a 2d space we can just print the
//layout once and move with "n, e, s, w" directions
//(dante made a map for his btw its pretty neat)

//Idea structure for things that exist ingame:
// generic base class, has booleans like canAttack or canTalk etc
// inherit every object from there and add on specifics 
//i.e. table has canAttack = false, an npc would have canTalk = true
// and handle the rest in specific classes


/*
Nick's Notes
----------Encounter Mechanic Stuff---------
Spawn random encounters based on a random number g

difficulty.SetTo("master difficulty")
sets baseNumber = 5

(randomly generated number).compareTo(baseNumber)
if(randnum > basenum){
    spawnenemy
}
else if(randnum == (basenum + 2))
{
    spawnHumanNPC
}
basenum is lower for higher difficulty
----------Respawn Mechanics----------------
When you died, you're dead
Ask player "new game? y/n" (yes runs Start(), no runs End())
needs more thought

----------Some things to note--------------
- our parser is in Play()
- it waits for "finished" to equal true to complete

----------Main Structure-------------------
Start() method should include character info, name, class, difficulty level
Play() method is the parser
End() method does ending stuff
*/
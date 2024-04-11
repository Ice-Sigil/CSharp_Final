public interface NPCInt
{
    /*THINGS WE NEED:
    - NPC Health DONE
    - NPC "Moves" (attack, wait, damage, ect. Must incorporate into combat system)
    - Specialized NPCs such as enemies, shopkeepers, and questgivers
    - Dialogue for every NPC that they can say
    (i.e. shopkeeper will tell you about items, enemies might taunt you if they're human, ect)
    - A boolean to track whether they're human or not (?)
    - A way for a room to contain/hold an NPC. This could either be in rooms or in NPCs systems
    - A way to tell the player that there is an NPC in the room (can incorporate w/ ^), i.e. discovery dialogue 
    */

    private Room _roomLoc;
    private string _discoveryDialogue;
    private bool _isHuman; //Maybe they say something every turn? How store?
    private bool _isShopkeeper;
    private bool _isHostile; //Might not be used for everything, just if you attack a shopkeeper
    private string[] _possibleDialogue = new string[5]; //Can just be set to null if not human

    private string[] _moves; //Potentially not best way to hold moves, can be determined later

    private int _HP{ get return _HP; set _HP = value; }

    private void modifyHealth(int damage){
        HP += damage;
    }

    private void sayDialogue(){
        Player player = new Player();
        var rand = new Random();
        player.NormalMessage(_possibleDialogue[rand.Next(0, 5)]);
    }



}
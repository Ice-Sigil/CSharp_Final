using System.Collections;
using System.Collections.Generic;
using System;
namespace StarterGame{
public class NPCInt
{
    /*THINGS WE NEED:
    - NPC Health DONE
    - NPC "Moves" (attack, wait, damage, ect. Must incorporate into combat system)
    - Specialized NPCs such as enemies, shopkeepers, and questgivers
    - Dialogue for every NPC that they can say DONE
    (i.e. shopkeeper will tell you about items, enemies might taunt you if they're human, ect) DONE 
    - A boolean to track whether they're human or not (?) DONE
    - A way for a room to contain/hold an NPC. This could either be in rooms or in NPCs systems
    - A way to tell the player that there is an NPC in the room (can incorporate w/ ^), i.e. discovery dialogue 
    */
        public Room _roomLoc;
        public string _discoveryDialogue;
        public bool _isHuman; 
        public bool _isShopkeeper;
        public bool _isHostile; //Might not be used for everything, just if you attack a shopkeeper
        public string[] _possibleDialogue = new string[5]; //Can just be set to null if not human

        public string[] _moves; //Potentially not best way to hold moves, can be determined later

        public int _HP{
            get{
            return _HP; }
            set {
            _HP = value; }
        }
        public int _maxHP{
            get {
                return _maxHP; } 
            set {
                _maxHP = value;}
        }

        public void modifyHealth(int damage){
            _HP += damage;
        }

        public void sayDialogue(Player player){
            var rand = new Random();
            player.NormalMessage(_possibleDialogue[rand.Next(0, 4)]);
        }



    }
  }
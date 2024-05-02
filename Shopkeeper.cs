using System.Collections;
using System.Collections.Generic;
using System;   
namespace StarterGame{
     public class Shopkeeper : Character
    {
        public Shopkeeper(string? name, int hp,int atk, int def) : base(name, hp, atk, def){}
        Room? currentRoom;
        private string _discoveryDialogue = "Shopkeeper: \"Good to see you. Lots of things for sale here. Take a look.\"";
    
        private string[] possibleDialogue = new string[]{"Is this really what you want to do? Fine, then. Die like the rest.", "Fool. You'll get what you deserve.", "You should have never come down here.", "May the gods let you die quickly.", "Wretched filth. I'll put you where you belong."};
        public string getDialogue(){
            return _discoveryDialogue;
        }
        public string getAttackDialogue(){
             Random random= new Random(4);
             return possibleDialogue[random.Next()];
        }
        }
    }
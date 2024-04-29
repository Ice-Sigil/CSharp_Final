using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public abstract class Command{
        private string _name; //Command name
        public string Name { get { return _name; } set { _name = value; } } 
        private string _secondWord; //The Comand's possible second word
        public string SecondWord { get { return _secondWord; } set { _secondWord = value; } }

        public Command(){
            this.Name = "";
            this.SecondWord = null;
        }
        public bool HasSecondWord(){
            return this.SecondWord != null;
        }
        public abstract bool Execute(Player player);
    }
}

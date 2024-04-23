using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame{
    public class Character{

        public Character(string name, int hp, int atk, int def){
            Name = name;
            HP = hp;
            ATK = atk;
            DEF = def;
        }

        private string? _name;
        public string? Name{
            get {
               return _name; 
            }
            set{ 
                _name = value; 
            }
        }
        private int? _hp;
        public int? HP{
            get{
                return _hp;
            }
            set{
                _hp = value; 
            }
        }
        private int _atk;
        public int ATK{
            get{
                return _atk;
            }
            set{
                _atk = value;
            }
        } 
        private int? _def; 
        public int? DEF{
            get{
                return _def;
            }
            set{
                _def = value; 
            }
        }
    }
}
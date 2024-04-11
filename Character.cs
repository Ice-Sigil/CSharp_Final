using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame{
    public class Character{
        private string? _name;
        public string? Name{
            get{
                _name; 
            }
            set{ 
                _name = value; 
            }
        }
        private int _hp
        public int HP{
            get{
                _hp
            }
            set{
                _hp = value; 
            }
        }
        private int _atk; 
        public string ATK{
            get{
                _atk;
            }
            set{
                _atk = value; 
            }
        }
         private int _def; 
        public string DEF{
            get{
                _def;
            }
            set{
                _def = value; 
            }
        }
    }
}
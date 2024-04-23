using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame{
    public class Character{

        public Character(): this("CharName"){}
        public Character(String name): this(name, 0){}
        public Character(String name, int hp): this(name, hp, 0){}
        public Character(String name, int hp, int atk) : this(name, hp, atk, 0){}
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
using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame{
    public class Character{
        private string? _name;
        public string? Name{
            get {
               return _name; 
            }
            set{ 
                _name = value; 
            }
        }
    
        private int _hp;
        public int HP{
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
        private int _def; 
        public int DEF{
            get{
                return _def;
            }
            set{
                _def = value; 
            }
        }
        public Character(): this("No name"){}
        public Character(string? name) : this(name, 0){}
        public Character(string? name, int hp) :this(name, hp, 0){}
        public Character(string? name, int hp, int atk) :this(name, hp, atk, 0){}
        public Character(string? name, int hp,int atk, int def) : this(name, hp, atk, def, 0){}
        public Character(string? name, int hp, int atk, int def, int lootchance){
            _name = name;
            _hp = hp;
            _def = def;
            _atk = atk;
        }
        //Attack Method  
        public int Attack(Character target){
            int damage = Math.Max(ATK - target.DEF, 0); // Ensure damage is not negative
            target.TakeDamage(damage);
            return damage; 
        }
        //Defend Method 
        public int Defend(Character target){
            int damage = Math.Max(target.ATK - DEF, 0);
            TakeDamage(damage);
            return damage; 
        }
        //Take Damage Method
        public void TakeDamage(int damage){
            HP = Math.Max(HP - damage, 0); // Ensure HP does not go below 0
        }
    }
}
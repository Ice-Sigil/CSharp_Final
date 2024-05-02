using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame{
    public class Character : ICharacter{
        private string? _name;
        public string? Name{
            get{
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
        private int _maxHP;
        public int MHP{
            get{
                return _maxHP;
            } 
            set{
                _maxHP = value;
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
        public string? MOD;

        //Generic Constructor for all Characters
        public Character(string name, int hp, int atk, int def){
            Name = name; 
            HP = hp; 
            ATK = atk; 
            DEF = def;  
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
            switch(MOD){
                case "easy":
                HP = Math.Max(HP - damage, 1); // Ensure HP does not go below 0
                break;
                case "medium":
                HP = Math.Max(HP - damage, 1); // Ensure HP does not go below 0
                break;
                case "hard":
                HP = Math.Max(HP - damage, 1); // Ensure HP does not go below 0
                break;
                default:
                break;

            }
            
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame{
    /*
     * Spring 2024
     */
    public class Room
    {
        private Dictionary<string, Room> _exits;
        private string _tag;
        public string Tag 
        { 
            get 
            { 
                return _roomDelegate==null?_tag:_roomDelegate.OnTag(_tag); 
            } 
            set 
            { 
                _tag = value; 
            } 
        }
        private IRoomDelegate _roomDelegate;
        public IRoomDelegate RoomDelegate
        {
            get {return _roomDelegate;}
            set 
            {
                if(value == null)
                {
                    if(_roomDelegate == null)
                    {
                        _roomDelegate.ContainingRoom = null;
                    }
                }
                else
                {
                    if(value.ContainingRoom != null)
                    {
                        value.ContainingRoom = null;
                    }
                    value.ContainingRoom = this;
                }
                _roomDelegate = value;
            }
        }
        private IItem _itemOnFloor;
        public Room() : this("No Tag"){}

        // Designated Constructor
        public Room(string tag)
        {
            _itemOnFloor = null;
            _roomDelegate = null;
            _exits = new Dictionary<string, Room>();
            this.Tag = tag;
        }

        public IItem Drop(IItem item)
        {
            IItem oldItem = _itemOnFloor;
            _itemOnFloor = item;
            return oldItem;
        }

        public IItem Pickup(string itemName)
        {
            IItem oldItem = null;
            if (_itemOnFloor != null)
            {
                if(_itemOnFloor.Name.Equals(itemName))
                {
                    oldItem = _itemOnFloor;
                    _itemOnFloor = null;
                    return oldItem;
                }
            }
            return oldItem;
        }

        public void SetExit(string exitName, Room room)
        {
            _exits[exitName] = room;
        }

        public Room GetExit(string exitName)
        {
            Room room = null;
            _exits.TryGetValue(exitName, out room);
            if(_roomDelegate != null)
            {
                room = _roomDelegate.OnGetExit(room);
            }
            return room;
        }

        public string GetExits()
        {
            string exitNames = "Exits: ";
            Dictionary<string, Room>.KeyCollection keys = _exits.Keys;
            foreach (string exitName in keys)
            {
                exitNames += " " + exitName;
            }

            return exitNames;
        }

        public string Description()
        {
            return "You are " + this.Tag + ".\n *** " + this.GetExits() + "\nItem: " + (_itemOnFloor == null ? "" : _itemOnFloor.Name);
        }
    }

    public class TrapRoom : IRoomDelegate
    {
        private bool _active;
        private Room _containingRoom;
        public Room ContainingRoom
        {
            set
            {
                _containingRoom = value;
            }
            get
            {
                return _containingRoom;
            }
        }
        private string magicWord;
        public TrapRoom(string magicWord)
        {
            _active = false;
            NotificationCenter.Instance.AddObserver("PlayerDidSayAWord", PlayerDidSayAWord);
            this.magicWord = magicWord;
        }
        public Room OnGetExit(Room room)
        {
            return _active?null:room;
        }
        public void PlayerDidSayAWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player != null)
            {
                if(player.CurrentRoom == ContainingRoom)
                {
                    Dictionary<string,object> userInfo = notification.UserInfo;
                    if(userInfo != null)
                    {
                        object spokenWordObject = null;
                        userInfo.TryGetValue("word", out spokenWordObject);
                        if(spokenWordObject != null)
                        {
                            string spokenWord = (string) spokenWordObject;
                            if(spokenWord.Equals(magicWord))
                            {
                                _active = false;
                                player.InfoMessage("You disabled the trap");
                            }
                            else
                            {
                                player.ErrorMessage("Nope, not the magic word :)");
                            }
                        }
                    }
                }
            }
        }
        public string OnTag(string tag)
        {
            return tag + (_active?".\n *** You are in a trap room! You must \"say\" the magic word to unlock the doors":"");
        }
    }
    public class CombatRoom : IRoomDelegate
    {
        private Enemy _enemy; 
        Random rand = new Random();
        public Enemy Enemy {
            get { return _enemy; }
            set { _enemy = value; }
        }
        private bool _active;
        private Room _containingRoom;
        public Room ContainingRoom
        {
            set
            {
                _containingRoom = value;
            }
            get
            {
                return _containingRoom;
            }
        }
        public CombatRoom(Enemy enemy)
        {
            _active = true;
            _enemy = enemy;
            NotificationCenter.Instance.AddObserver("PlayerDidStartCombat", PlayerDidStartCombat);
        }

        public void PlayerDidStartCombat(Notification notification){
            Player player = (Player)notification.Object;
            if (_active){
                if (player != null){
                    if(player.CurrentRoom == ContainingRoom){
                        StartCombat(player); 
                        _active = false;
                    }
                }
            }
        }

        public Room OnGetExit(Room room)
        {
            return _active?null:room;
        }

        public string OnTag(string tag)
        {


            return tag + (_active?".\n *** You have entered a Combat room! You cannot escape until the Enemy is defeated.":"");

        }
        public void StartCombat(Player player){
        while (player.HP > 0 && _enemy.HP > 0){
            CombatMenu(player, _enemy);  
            string? playerAction = Console.ReadLine(); 
            Console.ReadKey(); 
            switch (playerAction?.ToLower()){
                case "a":
                //Attack
                Console.WriteLine("You swing with your weapon! ");
                //NEED ATTACK COMMAND(maybe)
                int dmg = player.Attack(_enemy);
                Console.WriteLine("Dealing " + dmg + " damage!");
                break; 
                case "d":
                //Defend
                Console.WriteLine("You stand your ground!!!");
                //NEED DEFEND COMMAND(maybe)
                dmg = player.Defend(_enemy); 
                Console.WriteLine("You take " + dmg + " damage!");
                break;
                case "i":
                //Item
                break;
                case "r":
                //Run
                // NEED to finish RUN method
                player.Run(_enemy); 
                break;
                default:
                break; 
            }
            Console.ReadKey();  
            if(_enemy.HP > 0){
                //if the _enemy is alive they retaliate
                //in the future implement different actions other than Attack
                Console.WriteLine("The enemy swings!");
                int eDmg = _enemy.Attack(player);
                Console.WriteLine("You take " + eDmg + " damage!");
                Console.ReadKey(); 
            }
        }
        if (player.HP > 0){    
            int coins = rand.Next(10,30); 
            // absurd leveling to see where it goes
            int _xp = rand.Next(13,50); 
            //NEED GAINXP(), GAINCOIN(), AND LEVELCHECK() FUNCTIONS FROM MY PROTO PLAYER CLASS
            Console.WriteLine("You are victorious!");  
            Console.WriteLine("You gained " + _xp + " Exp and " + coins + " coins!");
            player.GainXP(_xp);
            player.GainCOIN(coins);
            //player.LevelCheck(player); 
            Console.ReadKey(); 
        }
        else{
            //Death Check 
            Console.WriteLine("You died!");
            Console.ReadKey(); 
            Environment.Exit(0); 
        }
    }
    public void CombatMenu(Player player, Enemy enemy){
        Console.Clear(); 
        Console.WriteLine(enemy.Name);
        Console.WriteLine("HP: " + enemy.HP);
        Console.WriteLine("=======================");
        Console.WriteLine("|| (A)ttack (D)efend ||");
        Console.WriteLine("||  (S)pells (I)tems ||");
        Console.WriteLine("||       (R)un       ||");
        Console.WriteLine("=======================");
        Console.WriteLine("Name: " + player.Name);
        Console.WriteLine("Level: " + player.LVL);
        Console.WriteLine("Exp: " + player.XP + " / " + player.MXP);
        Console.WriteLine("HP: " + player.HP + " / "  + player.MHP);
        Console.WriteLine("MP: ");
        Console.WriteLine("Potions: ");
        }   
    }
}



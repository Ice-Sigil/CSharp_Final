﻿using System.Collections;
using System.Collections.Generic;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace StarterGame{
    /*
     * Spring 2024
     */
    public class Room{
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
        private IItemContainer _itemsOnFloor;
        public Room() : this("No Tag"){}

        // Designated Constructor
        public Room(string tag)
        {
            _itemsOnFloor = new ItemContainer("floor", 0f);
            _roomDelegate = null;
            _exits = new Dictionary<string, Room>();
            this.Tag = tag;
        }

        public IItem Drop(IItem item)
        {
            IItem oldItem = _itemsOnFloor.Remove(item.Name);
            _itemsOnFloor.Insert(item);
            return oldItem;
        }

        public IItem Pickup(string itemName)
        {
            return _itemsOnFloor.Remove(itemName);
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
            return "You are " + this.Tag + ".\n *** " + this.GetExits() + "\nItems: " + _itemsOnFloor.Description;
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
        int coins;
        int _xp;
        public Enemy Enemy {
            get { return _enemy; }
            set { _enemy = value; }
        }
        private bool _active;
        private bool _escape = false;
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
        while (player.HP > 0 && _enemy.HP > 0 && !_escape){
            CombatMenu(player, _enemy);
            string? playerAction = Console.ReadLine();
            switch (playerAction?.ToLower())
            {
                case "a":
                    //Attack
                    Console.WriteLine("You swing with your weapon! ");
                    //NEED ATTACK COMMAND(maybe)
                    int dmg = player.Attack(_enemy);
                    Console.WriteLine("Dealing " + dmg + " damage!");
                    break;
                case "i":
				    player.Inventory();
                    break;
                case "r":
                    /*//Run
                    // NEED to finish RUN method
                    player.Run(enemy);*/
                    if (player.CurrentRoom != GameWorld.Instance._worldOut)
                    {
                        _escape = false;
                        Random chance = new Random();
                        switch (chance.Next(0, 3))
                        {
                            case 1:
                                player.WarningMessage("Escape failed..!");
                                break;
                            default:
                                player.InfoMessage("You escape successfully...");
                                _escape = true;
                                break;
                        }
                    }
					else
					{
						player.WarningMessage("You can't escape the boss battle!?");
					}
                    break;
                default:
                    break;
            }
            Console.ReadKey();  
            if(_enemy.HP > 0 && playerAction != "i" && playerAction != ""){
                //if the _enemy is alive they retaliate
                //in the future implement different actions other than Attack
                Console.WriteLine("The enemy swings!");
                int eDmg = _enemy.Attack(player);
                Console.WriteLine("You take " + eDmg + " damage!");
                Console.ReadKey(); 
            }
        }
        if (player.HP > 0 && _escape == false){    
            coins = rand.Next(10,30); 
            // absurd leveling to see where it goes
            _xp = rand.Next(13,50); 
            //NEED GAINXP(), GAINCOIN(), AND LEVELCHECK() FUNCTIONS FROM MY PROTO PLAYER CLASS
            Console.WriteLine("You are victorious!");  
            Console.WriteLine("You gained " + _xp + " Exp and " + coins + " coins!");
            player.GainXP(_xp);
            player.GainCOIN(coins);
            //player.LevelCheck(player); 
            Console.ReadKey(); 
        }
        else if(_escape == true)
        {
            coins = 0;
            _xp = 0;
			Console.WriteLine("You gain 0 coins and XP.");
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
        Console.WriteLine("|| (A)ttack (I)tems  ||");
        Console.WriteLine("||       (R)un       ||");
        Console.WriteLine("=======================");
        Console.WriteLine("Name: " + player.Name);
        Console.WriteLine("Level: " + player.LVL);
        Console.WriteLine("Exp: " + player.XP + " / " + player.MXP);
        Console.WriteLine("HP: " + player.HP + " / "  + player.MHP);
        Console.WriteLine("Potions: ");
        }   
    }
    // barrier for shop class
    public class ShopRoom : IRoomDelegate
    {
        private Shopkeeper _shopkeeper; 
        public Shopkeeper Shopkeeper {
            get { return _shopkeeper; }
            set { _shopkeeper = value; }
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
        public ShopRoom(Shopkeeper shopkeeper)
        {
            _active = true;
            _shopkeeper = shopkeeper;
            NotificationCenter.Instance.AddObserver("PlayerDidEnterShop", PlayerDidEnterShop);
        }

        public void PlayerDidEnterShop(Notification notification){
            Player player = (Player)notification.Object;
            if (_active){
                if (player != null){
                    if(player.CurrentRoom == ContainingRoom){
                        player.NormalMessage(_shopkeeper.getDialogue());
                    }
                }
            }
        }

        public Room OnGetExit(Room room)
        {
            return room;
        }

        public string OnTag(string tag)
        {
            return tag + (_active?".\n *** You have entered a Shop Room! You are free to purchase whatever you please.":"");

        }
        public void ShopMenu(){
        Console.Clear(); 
        Console.WriteLine("=======================");
        Console.WriteLine("|| (B)uy      (S)ell ||");
        Console.WriteLine("||      (A)ttack     ||");
        Console.WriteLine("||     (G)oodbye     ||");
        Console.WriteLine("=======================");
        }
        public void ShopLoop(Player player, Shopkeeper shopkeeper){
            ShopMenu();
            string? playerInput = Console.ReadLine().ToLower();
            while(playerInput != "g"){
                switch(playerInput){
                    case "b": 
                     //   shopkeeper.displayWares;
                        break;
                    case "s":
                    //    player.displayInventory;
                        break;
                    case "a":
                    
                        break;
                }
            }
        }
    }
 }



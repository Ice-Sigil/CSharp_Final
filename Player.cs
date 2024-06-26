﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace StarterGame{
    /*
     * Spring 2024
     */
    //Added a character class to hold Name, HP, MP?, ATK & DEF-Dante
    public class Player : Character {
        public int LVL {get; private set;}  // Player level
        public int XP {get; private set;}  // Experience points
        public int MXP {get; set;} // Max Exp
        public int COIN {get; set;} // Player Coins 
        Random rand = new Random();
        private Room _currentRoom = null; // Player's current location 
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }
        public float WeightLimit { get {return _weightLimit;} set {_weightLimit = value;} }
        Stack<Room> stack = new Stack<Room>();
        
        private Item _currentWeapon;
        public Item CurrentWeapon { get { return _currentWeapon;} set { _currentWeapon = value; } }
        private Item _currentArmor;
        public Item CurrentArmor { get { return _currentArmor;} set { _currentArmor = value; } }
        private ItemContainer _backpack;
        public ItemContainer Backpack { get { return _backpack;} set { _backpack = value; } }
        private float _weightLimit = 10.0f;
        
        //Constructors
        public Player() : this(null) { }
        public Player(Room room) : this(room, null) { }
        public Player(Room room, string? name) : this(room, name, 0) { }
        public Player(Room room, string? name, int hp) : this(room, name, hp, 0) { }
        public Player(Room room, string? name, int hp, int atk) : this(room, name, hp, atk, 0){}
        public Player(Room room, string name, int hp, int atk, int def) : base(name, hp, atk, def){
            //Starting Weapon
            CurrentWeapon = new Item("Dagger", 3.0f, 4, 5, false);
            CurrentArmor = new Item("Leather Rags", 1.0f, 1, 1, false);
            _currentRoom = room;
            _backpack = new ItemContainer("backpack", 0f);
            LVL = 1; // to start
            XP = 0;
            MXP = 13;
            COIN = 13;
            HP = hp; 
            MHP = hp;
            ATK = atk + CurrentWeapon.UseValue;
            DEF = def + CurrentArmor.UseValue;
            //Starting Items for the Player 
            Item fullRestore = new Item("Restore", 1.0f, MHP, 0, true);
            Give(fullRestore);
            Item healthPotion = new Item("Potion", 0.5f, 20, 0, true);
            for (int x = 0; x < 4; x++){
                Give(healthPotion);
            }
            
        }
        public void WaltTo(string direction){
            stack.Push(this.CurrentRoom);
            Room nextRoom = this.CurrentRoom.GetExit(direction);
            if(_backpack.Weight < _weightLimit)
            {
                if (nextRoom != null)
                {
                    CurrentRoom = nextRoom; 
                    Notification notification = new Notification("PlayerDidEnterRoom", this);
                    NotificationCenter.Instance.PostNotification(notification);
                    Notification combatStart = new Notification("PlayerDidStartCombat", this); 
                    NotificationCenter.Instance.PostNotification(combatStart);
                    Notification shopEnter = new Notification("PlayerDidEnterShop", this);
                    NotificationCenter.Instance.PostNotification(shopEnter); //Activating observers for game events
                    Console.Clear(); 
                    GameWorld.Instance.Map(this);
                    NormalMessage("\n" + this.CurrentRoom.Description());
                }   
            }
            else
                {
                    ErrorMessage("\nThere is no door on " + direction);
                }
            }
            else
            {
                WarningMessage("Overencumbered! Drop some items to progress...");
            }
        }

        public void GoBack(Player player){
            if(player.CurrentRoom != GameWorld.Instance.Entrance)
            {
                player.CurrentRoom = stack.Peek();
                stack.Pop();
            }
            else
            {
                player.ErrorMessage("No turning back now!");
            }
        }

        public void OutputMessage(string message){
            Console.Write(message);
        }

        public void ColoredMessage(string message, ConsoleColor newColor){
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            OutputMessage(message);
            Console.ForegroundColor = oldColor;
        }

        public void NormalMessage(string message){
            ColoredMessage(message, ConsoleColor.White);
        }

        public void InfoMessage(string message){
            ColoredMessage(message, ConsoleColor.Blue);
            Console.WriteLine();
        }

        public void WarningMessage(string message){
            ColoredMessage(message, ConsoleColor.DarkYellow);
            Console.WriteLine();
        }

        public void ErrorMessage(string message){
            ColoredMessage(message, ConsoleColor.Red);
            Console.WriteLine();
        }

        public void GainXP(int amount){
            XP += amount;
            LevelCheck(this);
        // Check for level up and increase stats as needed
        }
        public void GainCOIN(int amount){
            COIN += amount; 
        }
        private void LevelCheck(Player player){
            if (player.XP >= player.MXP){
                Console.WriteLine(player.Name + " has leveled up!!!");  
                player.MHP += 2;
                player.HP = player.MHP; 
                player.XP = 0;
                player.MXP *= 2; 
                player.ATK += 2; 
                player.DEF += 2;
                player.LVL ++;
                COIN = player.COIN; //ensures coins arent lost on a level up
                DisplayPlayerStats();  
                Console.ReadKey(); 
            }
            else{
                //Do Nothing 
            }
        }
        public void DisplayPlayerStats(){
            Console.WriteLine("====================");
            Console.WriteLine("|Name: " + Name);
            Console.WriteLine("|Level:" + LVL);
            Console.WriteLine("|Exp: " + XP + "/" + MXP); 
            Console.WriteLine("|HP: " + HP );
            Console.WriteLine("|ATK: " + ATK + " (" + CurrentWeapon.UseValue + ")");
            Console.WriteLine("|DEF: " + DEF + " (" + CurrentArmor.UseValue + ")"); 
            Console.WriteLine("|Coins: " + COIN);
            Console.WriteLine("|Current Weapon: " + CurrentWeapon.Name);
            Console.WriteLine("|Current Armor: " + CurrentArmor.Name);
            Console.WriteLine("====================");
        }
          public void Inspect(string itemName){
            IItem pickedUpItem = CurrentRoom.Pickup(itemName); //Makes sure the item exists in the room before you inspect it
            if (pickedUpItem != null){
                InfoMessage("\nItem Info: " + pickedUpItem.Description); //Gives you the item description if you have it.
            }
            else{
                ErrorMessage("\n There is no item named that, or there is no such item in the room.");
            }
        }

        public void Say(string word){
            Console.Write("The player says: \"");
            NormalMessage(word);
            Console.Write("\"\n");
            Notification notification = new Notification("PlayerDidSayAWord", this); //Create notification for saying word
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            userInfo["word"] = word;
            notification.UserInfo = userInfo;
            NotificationCenter.Instance.PostNotification(notification);
        }
        public void Inventory(){
            NormalMessage(_backpack.Description + "\n"); 
            bool inInventoryMenu = true; //Set game state to stay in the inventory until player manually exits.
            if (inInventoryMenu){
            Console.WriteLine("(U)se||(E)xit||(T)hrow"); //Inventory menu
            string? playerInput = Console.ReadLine();
            switch (playerInput){
                case "U":
                Console.WriteLine("Which item would you like to use? "); 
                string? itemChoice = Console.ReadLine();
                if (itemChoice != null){
                Use(itemChoice);
                }
                break;
                case "E":
                inInventoryMenu = false; //Set game state to not inventory and break out of the menu
                break;
                default:
                Console.WriteLine("Invalid Input...");
                break;
                } 
            }
        }
        public void InventoryDisplay(){
            NormalMessage(_backpack.Description + "\n"); 
        }
        public void Unlock(Player player)
        {
            foreach(string key in _backpack.items.Values)
            {
                if(key != null)
                {
                    if(key.Key == "key")
                    {
                        WorldEvent.ExecuteEvent();
                    }
                }
            }

        }
        public void Give(IItem item){
            if (item != null){
               _backpack.Insert(item);
               item.Count++;
            }
            else{
                Console.WriteLine("Cannot give that item for it does not exist...");
            }
        }
        public void Use(string itemName){
            //Currently in Development 
            //Will iterate through the inventory to match with the given name and then use the item, 
            //then call Take() here instead of in Combat and only call Use() during Combat -Dante
            IItem item = Take(itemName);
            Item itemInUse = (Item)item; //Cast to item to integrate with other systems
            if (itemInUse != null && itemInUse.Count > 1 && itemInUse.IsUsable == true){
                Heal(itemInUse);
                itemInUse.Count--;
                Give(itemInUse);
                }
            else if (itemInUse != null && itemInUse.Count == 1 && itemInUse.IsUsable == true){
                Heal(itemInUse);
                itemInUse.Count--;
            }
            else if (itemInUse != null && itemInUse.IsUsable != false){
                Console.WriteLine("That " + itemName + " is not useable");
            }
            else{
               Console.WriteLine("That " + itemName + " does not exist.");  
            }
        }
        public void Heal(IItem item){
            if (item.UseValue > 0){
                    Console.WriteLine("The Player used a " + item.Name + " gaining " + item.UseValue + " HP!"); 
                    HP += item.UseValue;
                    if (HP > MHP){
                        HP = MHP; //prevents overhealing
                }
            }
        }
        public void EquipWeapon(Item item){
            if (CurrentWeapon == null){
                CurrentWeapon = item; 
                Console.WriteLine("The player equips" + item.Name); 
            }
            else{
                //You already have a weapon equiped
            }
        }
        public void EquipArmor(){

        }
        public void UnEquipArmor(){

        }
        public void UnEquipWeapon(Item item){
            if (CurrentWeapon != null){
                CurrentWeapon = null; 
                Console.WriteLine("The player unequips" + item.Name); 
            }
            else{
                
            }
        }

        public IItem Take(string itemName){
            
            return _backpack.Remove(itemName);
        }
        public void Pickup(string itemName){
            IItem item = CurrentRoom.Pickup(itemName);
            if (item != null){
                Give(item);
                NormalMessage("You have picked up the " + itemName);
            }
            else{
                ErrorMessage("There is no item named " + itemName +" in this room.");
            }
        }

        public void Drop(string itemName){
            IItem item = Take(itemName);
            if (item != null){
                CurrentRoom.Drop(item);
                NormalMessage("You have dropped up the " + itemName);
            }
            else{
                ErrorMessage("There is no item named " + itemName +" in your inventory.");
            }
        }
        public void Buy(ItemContainer shop, Item item){
            bool? itemExists = null;
            Item? itemInInventory = null;
            foreach (Item i in _backpack.Items.Values){
                if (i.Name.Equals(item.Name)){
                    itemInInventory = i;
                    itemExists = true;
                }
            }
            if (itemExists == true){
                itemInInventory.Count ++;
                COIN -= item.Cost;
                shop.Remove(item.Name);   
            }
            else{
            Give(item);
            COIN -= item.Cost;
            shop.Remove(item.Name);
            }
        }
        public void Sell(ItemContainer shop, Item item){
            Take(item.Name);
            COIN += item.Cost;
            shop.Insert(item);
        }
        public void SetDifficulty(string difficulty){
            float diffInt = 1.0f;
            switch(difficulty)
            {
            case "easy":
                diffInt = 1.5f;
                break;
            case "medium":
                diffInt = 1;
                break;
            case "hard":
                diffInt = 0.5f;
                break;
            default:
                break;
            }
            if (MOD != 0.0f)
            {
                MOD = diffInt;
            }
        }
    }
}

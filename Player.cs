using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    //Added a character class to hold Name, HP, MP?, ATK & DEF-Dante
    public class Player : Character 
    {
        public int LVL {get; private set;}  // Player level
        public int XP {get; private set;}  // Experience points
        public int MXP {get; private set;} // Max Exp
        public int COIN {get; private set;} // Player Coins 
        Random rand = new Random();
        private Room _currentRoom = null; // Player's current location 
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        private IItemContainer _backpack;

        //Constructors
        public Player() : this(null) { }
        public Player(Room room) : this(room, null) { }
        public Player(Room room, string? name) : this(room, name, 0) { }
        public Player(Room room, string? name, int hp) : this(room, name, hp, 0) { }
        public Player(Room room, string? name, int hp, int atk) : this(room, name, hp, atk, 0){}
        public Player(Room room, string name, int hp, int atk, int def) : base(name, hp, atk, def)
        {
            _currentRoom = room;
            _backpack = new ItemContainer("backpack", 0f);
            LVL = 1; // to start
            XP = 0;
            MXP = 13;
            COIN = 13;
            HP = hp; 
            MHP = hp;
            ATK = atk;
            DEF = def; 
        }
        public void WaltTo(string direction)
        {
            Room nextRoom = this.CurrentRoom.GetExit(direction);
            if (nextRoom != null)
            {
                CurrentRoom = nextRoom;
                Notification notification = new Notification("PlayerDidEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                Notification combatStart = new Notification("PlayerDidStartCombat", this); 
                NotificationCenter.Instance.PostNotification(combatStart);
                Notification shopEnter = new Notification("PlayerDidEnterShop", this);
                NotificationCenter.Instance.PostNotification(shopEnter);
                NormalMessage("\n" + this.CurrentRoom.Description());
            }
            else
            {
                ErrorMessage("\nThere is no door on " + direction);
            }
        }

        public void OutputMessage(string message)
        {
            Console.Write(message);
        }

        public void ColoredMessage(string message, ConsoleColor newColor)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            OutputMessage(message);
            Console.ForegroundColor = oldColor;
        }

        public void NormalMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.White);
        }

        public void InfoMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Blue);
            Console.WriteLine();
        }

        public void WarningMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.DarkYellow);
            Console.WriteLine();
        }

        public void ErrorMessage(string message)
        {
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
             //   player.MHP += 2;
            //    player.HP = player.MHP; 
                player.XP = 0;
                player.MXP *= 2; 
                player.ATK += 2; 
                player.DEF += 2;
                player.LVL ++;
                DisplayPlayerStats();  
                Console.ReadKey(); 
            }
            else{
                //Do Nothing 
            }
        }
        //Needs to be finished
        public void Run(Enemy enemy){
            Random chance = new Random();
            switch (chance.Next(0, 3)){
                case 1:
                    //No escape. Stay in combat system
                    
                    break;
                case 2:
                    //Rough escape. Exit combat system, but take damage.
                    
                    break;
                case 3:
                    //True escape. Exit combat system without damage.
                    
                    break;
                default:
                    break;
            } 
        }
        public void DisplayPlayerStats(){
            Console.WriteLine("====================");
            Console.WriteLine("|Name: " + Name);
            Console.WriteLine("|Level:" + LVL);
            Console.WriteLine("|Exp: " + XP + "/" + MXP); 
            Console.WriteLine("|HP: " + HP );
            Console.WriteLine("|ATK: " + ATK);
            Console.WriteLine("|DEF: " + DEF); 
            Console.WriteLine("|Coins: " + COIN);
            Console.WriteLine("====================");
        }
          public void Inspect(string itemName){
            IItem pickedUpItem = CurrentRoom.Pickup(itemName);
            if (pickedUpItem != null){
                InfoMessage("\nItem Info: " + pickedUpItem.Description);
            }
            else{
                ErrorMessage("\n There is no item named that, or there is no such item in the room.");
            }
        }

        public void Say(string word)
        {
            Console.Write("The player says: \"");
            NormalMessage(word);
            Console.Write("\"\n");
            Notification notification = new Notification("PlayerDidSayAWord", this);
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            userInfo["word"] = word;
            notification.UserInfo = userInfo;
            NotificationCenter.Instance.PostNotification(notification);
    
        }

        public void Inventory(){
            NormalMessage(_backpack.Description); //itemcontainer, fix later
        }

        public void Give(IItem item){
            _backpack.Insert(item);
        }

        public IItem Take(string itemName){
            return _backpack.Remove(itemName);
        }

        public void Pickup(string itemName){
            IItem item = CurrentRoom.Pickup(itemName);
            if (itemName != null){
                Give(item);
                NormalMessage("You have picked up the " + itemName);
            }
            else{
                ErrorMessage("There is no item named " + itemName +" in this room.");
            }
        }

        public void Drop(string itemName){
            IItem item = Take(itemName);
            if (itemName != null){
                CurrentRoom.Drop(item);
                NormalMessage("You have dropped up the " + itemName);
            }
            else{
                ErrorMessage("There is no item named " + itemName +" in your inventory.");
            }
        }
    }
}

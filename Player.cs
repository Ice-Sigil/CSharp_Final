using System.Collections;
using System.Collections.Generic;
using System;

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
        private Room _currentRoom = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        public Player(Room room)
        {
            _currentRoom = room;
            Name = ""; 
            HP = 30;
            //ATK = 5;
            //DEF = 5;
            LVL = 1; // to start
            XP = 0;
            MXP = 13;
            COIN = 13;
        }

        public void WaltTo(string direction)
        {
            Room nextRoom = this.CurrentRoom.GetExit(direction);
            if (nextRoom != null)
            {
                CurrentRoom = nextRoom;
                // Notification notification = new Notification("PlayerDidEnterRoom", );
                //NotificationCenter.Instance.PostNoficiation(notification)
                NormalMessage("\n" + this.CurrentRoom.Description());
            }
            else
            {
                ErrorMessage("\nThere is no door on " + direction);
            }
        }

        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
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
        }

        public void WarningMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.DarkYellow);
        }

        public void ErrorMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Red);
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
                    //No escape
                    
                    break;
                case 2:
                    //Rough escape
                    
                    break;
                case 3:
                    //True escape
                    
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
            Console.WriteLine("|" );
            Console.WriteLine("|" ); 
            Console.WriteLine("====================");
        }
          public void Inspect(string itemName){
            ItemClass pickedUpItem = CurrentRoom.pickup(itemName);
            if (pickedUpItem != null){
                InfoMessage("\nItem Info: " + pickedUpItem.Description);
            }
            else{
                ErrorMessage("\n There is no item named that, or there is no such item in the room.");
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace StarterGame
{
    public class GameWorld
    {
        public int Width { get; set; }
        public int Height { get; set;}
        public Room[,] Floor{ get; set; }
        public string right = "right";
        public string left = "left";
        public string up = "up";
        public string down = "down";
        public string tile = "on a regular floor tile";

        private static GameWorld _instance;
        public static GameWorld Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }
        private Room _entrance;
        public Room Entrance { get { return _entrance; } }
        private Room _triggerRoom;
        public Room _worldOut;
        private Room _worldInAnnex;
        private Room _combatRoom; 
        private Room _shopRoom;

        private static Enemy[] _enemyArray = { new Enemy("goblin", 50, 2, 1)};

        private GameWorld()
        {
            CreateWorld();
            NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);
        }

        public void PlayerDidEnterRoom(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player != null)
            {
                if(player.CurrentRoom == _triggerRoom)
                {
                    player.WarningMessage("\n***Trigger Activated.");
                }
                if(player.CurrentRoom == _worldOut)
                {
                    player.InfoMessage("\n***Exit found..!");
                }
                if (player.CurrentRoom == _combatRoom)
                {
                    player.WarningMessage("You are in a Combat Room"); 
                }
            }
        }

        private void CreateWorld()
        {
            Random random = new Random();
            Height = random.Next(5,8); //Height can be from 5 to 8
            Width = 3;                 //the Width is gonna be the same so it doesnt take too long to complete the Floor, will change after project is over
            int direction;

            Floor = new Room[Height,Width];
            /*
                #E# <-- _entrance Floor[0,Width-2]         
                ###                
                ###                
                ###         
                #X# <-- _worldOut Floor[Height-1,Width-2]

            */
            Floor[0,1] = new Room("at the beginning of the floor");
            Floor[1,1] = new Room(tile);
            _entrance = Floor[0,1];

            Floor[Height-1,Width-2] = new Room("at the exit");
            Floor[Height-2,Width-2] = new Room(tile);
            _worldOut = Floor[Height-1,Width-2];
            direction = random.Next(1,10);

            for(int i = 1; i < Height-1; i+=2)    //create Floor
            {
                for(int j = 0; j < Width; j++)
                {
                    if(i == Height-1) {break;}
                    if(i % 2 != 0)
                    {
                        Floor[i,j] = new Room(tile);
                    }
                    if(direction < 6)
                    {
                        Floor[i,0] = new Room(tile);
                        Floor[i+1,0] = new Room(tile);
                        direction = random.Next(2,7);
                    }
                    else if(direction > 5)
                    {
                        Floor[i,2] = new Room(tile);     // 1 2 3 4 5  6 7 8 9 10 
                        Floor[i+1,2] = new Room(tile);
                        direction = random.Next(4,9);     //change this to influence chance of either left or right path
                    }
                }
            }

            MakeExits();
            FillRoom();
        }

        public void MakeExits()
        {
            Floor[0,1].SetExit(down,Floor[1,1]);
            for(int i = 1; i < Height; i++)    //create exits
            {
                for(int j = 0; j < Width; j++)
                {
                    if(Floor[i,j] != null)
                    {
                        for(int k = 1; k <= 4; k++)
                        {
                            if(Floor[i-1,j] != null)
                            {
                                Floor[i,j].SetExit(up,Floor[i-1,j]);
                            }
                            if(j != 2)
                            {
                                if(Floor[i,j+1] != null)
                                {
                                    Floor[i,j].SetExit(right,Floor[i,j+1]);
                                }
                            }
                            if(i != (Height-1))
                            {
                                if(Floor[i+1,j] != null)
                                {
                                    Floor[i,j].SetExit(down,Floor[i+1,j]);
                                }
                            }
                            if(j != 0)
                            {
                                if(Floor[i,j-1] != null)
                                {
                                    Floor[i,j].SetExit(left,Floor[i,j-1]);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void FillRoom()
        {
            TrapRoom tp = new TrapRoom("unlock");
            IItem item = new Item("knife");
            Floor[0,1].Drop(item);
            Floor[1,1].RoomDelegate = tp;

            IItem decorator = new Item("gem", 0.5f);
            item.Decorate(decorator);
            decorator = new Item("gold", 0.7f);
            item.Decorate(decorator);

            IItemContainer chest = new ItemContainer("chest", 0f);
            Floor[1,1].Drop(chest);
            item = new Item("ball", 0.5f);
            chest.Insert(item);
            item = new Item("bat", 3.5f);
            chest.Insert(item);

            Enemy practiceDummy = new Enemy("Bandit", 20, 5, 5);
            CombatRoom cr = new CombatRoom(_enemyArray[0]);
            Floor[Height-1,Width-2].RoomDelegate = cr;

            Shopkeeper shopkeeper = new Shopkeeper("Shopman", 20, 5, 5);
            ShopRoom sr = new ShopRoom(shopkeeper);
            Floor[Height-2, Width-2].RoomDelegate = sr;
            
        }

        public void Map(Player player)
        {
            for(int i = 0; i < Height; i++)
            {
                Console.WriteLine();
                for(int j = 0; j < Width; j++)
                {
                    switch (player.CurrentRoom == Floor[i,j])
                    {
                    case true:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("P");
                        Console.ResetColor();
                        break;
                    case false when Floor[i,j] == null:
                        // Console.BackgroundColor = ConsoleColor.Black;
                        // Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                        // Console.ResetColor();
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(" ");
                        Console.ResetColor();
                        break;
                    }
                }
            }
            Console.WriteLine();
            player.InfoMessage(" ^ the exit");
        }
    
        // private void CreateWorld()
        // {
        //     Room outside = new Room("outside the main entrance of the university");
        //     Room scctparking = new Room("in the parking lot at SCCT");
        //     Room boulevard = new Room("on the boulevard");
        //     Room universityParking = new Room("in the parking lot at University Hall");
        //     Room parkingDeck = new Room("in the parking deck");
        //     Room scct = new Room("in the SCCT building");
        //     Room theGreen = new Room("in the green in from of Schuster Center");
        //     Room universityHall = new Room("in University Hall");
        //     Room schuster = new Room("in the Schuster Center");

        //     //annex center
        //     Room davidson = new Room(" in Davidson Student Center");
        //     Room clockTower = new Room(" at the Clock Tower");
        //     Room woodhall = new Room(" in Woodhall");
        //     Room greekCenter = new Room(" at the Greek Center");

        //     outside.SetExit("right", boulevard);

        //     boulevard.SetExit("left", outside);
        //     boulevard.SetExit("up", scctparking);
        //     boulevard.SetExit("right", theGreen);
        //     boulevard.SetExit("down", universityParking);

        //     scctparking.SetExit("right", scct);
        //     scctparking.SetExit("down", boulevard);

        //     scct.SetExit("left", scctparking);
        //     scct.SetExit("down", schuster);

        //     schuster.SetExit("up", scct);
        //     schuster.SetExit("down", universityHall);
        //     schuster.SetExit("left", theGreen);

        //     theGreen.SetExit("right", schuster);
        //     theGreen.SetExit("left", boulevard);

        //     universityHall.SetExit("up", schuster);
        //     universityHall.SetExit("left", universityParking);

        //     universityParking.SetExit("up", boulevard);
        //     universityParking.SetExit("right", universityHall);
        //     universityParking.SetExit("down", parkingDeck);

        //     parkingDeck.SetExit("up", universityParking);
        //     //set up room delegates
        //     TrapRoom tp = new TrapRoom();
        //     scct.RoomDelegate = tp;

        //     //Build annex world
        //     davidson.SetExit("right", clockTower);
        //     clockTower.SetExit("down", greekCenter);
        //     clockTower.SetExit("up", woodhall);

        //     greekCenter.SetExit("down", clockTower);

        //     _triggerRoom = scctparking;
        //     _entrance = outside;
        //     _worldOut = schuster;
        //     _worldInAnnex = davidson;
        // }
        private class WorldEvent
        {
            private Room _trigger;
            public Room Trigger { get { return _trigger; } }
            private Room _worldOut;
            private Room _worldInAnnex;
            private string _directionFromWorld;
            private string _directionToWorld;
            private Room _combatRoom;

            public WorldEvent(Room trigger, Room worldOut, Room worldInAnnex, string directionFromWorld, string directionToWorld, Room combatRoom)
            {
                _trigger = trigger;
                _worldOut = worldOut;
                _worldInAnnex = worldInAnnex;
                _directionFromWorld = directionFromWorld;
                _directionToWorld = directionToWorld;
                _combatRoom = combatRoom;
            }


            public void ExecuteEvent()
            {
                _worldOut.SetExit(_directionFromWorld, _worldInAnnex);
                _worldInAnnex.SetExit(_directionToWorld, _worldOut);
            }

        }
        //use room's .drop function to add items to rooms. Can use the same for shops?

    }
}

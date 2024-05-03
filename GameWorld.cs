using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace StarterGame{
    public class GameWorld{
        public int Width { get; set; }
        public int Height { get; set;}
        public Room[,] Floor{ get; set; }
        public string right = "right";
        public string left = "left";
        public string up = "up";
        public string down = "down";
        public string tile = "on a regular floor tile";
        public static int _level = 0;

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
        private Room _lockedRoom;
        public Room _worldOut;
        private Room _worldInAnnex;
        private Room _bossRoom; 
        private Room _shopRoom;
        public Enemy _shopKeep = new Enemy("Angry ShopKeeper", 9999, 99, 99);
            
        private static Enemy[] _enemyBosses = { 
            new Enemy("Boss 1: Bandit Chief", 50, 5, 3), 
            new Enemy("Boss 2: Mutant Rat", 75, 6, 4),
            new Enemy("Boss 3: Stone Golem", 100, 10, 6),
            new Enemy("Boss 4: Ghost Rider", 150, 15, 9),
            new Enemy("Final Boss: Dr. Obando", 200, 20, 10)
        };

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
                if(player.CurrentRoom == _lockedRoom)
                {
                    player.WarningMessage("\n***Trigger Activated.");
                }
                if (player.CurrentRoom == _bossRoom)
                {
                    player.WarningMessage("The boss is defeated... [use 'next']"); 
                }
            }
        }

        public bool NextFloor(Player player)
        {
            if (player.CurrentRoom == _worldOut)
            {
                if (_level < 5)
                {
                    Floor = null;
                    CreateWorld();
                    player.CurrentRoom = _entrance;
                    player.InfoMessage("\n" + player.CurrentRoom.Description());
                }
                else
                {
                    _level = 0;
                    player.WarningMessage("You just beat the final boss! Congraturaltionese!");
                    player.WarningMessage(":DDDDD");
                    return true;
                }
            }
            else
            {
                player.WarningMessage("You need to be at the exit to proceed to the next floor!");
            }
            return false;
        }

        private void CreateWorld()
        {
            _level += 1;
            _shopRoom = null;
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
            Floor[0,1] = new Room("at the beginning of Floor " + _level);
            
            Floor[1,1] = new Room(tile);
            _entrance = Floor[0,1];

            Floor[Height-1,Width-2] = new Room("at the exit");
            Floor[Height-2,Width-2] = new Room(tile);
            _lockedRoom = Floor[Height-1,Width-2];
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
            Random rand = new Random();
            TrapRoom tp = new TrapRoom("unlock");
            IItem item = new Item("knife", 10.0f);
            IItem key = new Item("key");
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

            CombatRoom cr = new CombatRoom(_enemyBosses[_level-1]);
            Floor[Height-1,Width-2].RoomDelegate = cr;
	        _bossRoom = Floor[Height-1, Width-2];

            Shopkeeper shopkeeper = new Shopkeeper("Shopman", 20, 5, 5);
            ShopRoom sr = new ShopRoom(shopkeeper);
            Enemy enemy = new Enemy("placeholder",10,10,10);
            //Floor[Height-2, Width-2].RoomDelegate = sr;
	        //_shopRoom = Floor[Height-2, Width-2];
            bool dropped = false;
            for(int i = 1; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    if(Floor[i,j] != null)
                    {
                        if(Floor[i,j] != _lockedRoom)
                        {
                            double roomChance = rand.NextDouble();
                            if(_shopRoom == null)
                            {
                                if(roomChance < 0.30)
                                {
                                    sr = new ShopRoom(shopkeeper);
                                    Floor[i,j].RoomDelegate = sr;
                                    _shopRoom = Floor[i,j];
                                }
                            }
                            roomChance = rand.NextDouble();
                            if(roomChance < 0.25)
                            {
                                if(Floor[i,j] != _shopRoom)
                                {
                                    enemy = enemy.enemyFactory(_level);
                                    cr = new CombatRoom(enemy);
                                    Floor[i,j].RoomDelegate = cr;
                                }
                            }
                            if(!dropped)
                            {
                                roomChance = rand.NextDouble();
                                if(roomChance < 0.25)
                                {
                                    if(Floor[i,j] != _lockedRoom)
                                    {
                                        Floor[i,j].Drop(key);
                                        dropped = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
                        Console.Write(" ");
                        break;
                    default:
                        if(Floor[i,j] == _bossRoom)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write(" ");
                            Console.ResetColor();
                        }
                        else if(Floor[i,j] == _shopRoom)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write(" ");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write(" ");
                            Console.ResetColor();
                        }
                        break;
                    }
                }
            }
            Console.WriteLine();
            player.InfoMessage(" ^ the exit\n");
            Legend();
        }

        public void Legend()
        {
            Console.WriteLine("Legend:\nP  <- The Player");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(" ");
            Console.ResetColor();
            Console.Write("  <- The Floor\n");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write(" ");
            Console.ResetColor();
            Console.Write("  <- The Shopkeeper\n");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
            Console.ResetColor();
            Console.Write("  <- The Boss/Exit");
        }

        public static void Reset()
        {
            _level = 0;
            _instance = null;
            Game game = new Game();
            Player _player = new Player(GameWorld.Instance.Entrance, "Prisoner", 30, 12, 6);
            //_player.CurrentRoom = GameWorld.Floor[0,1];

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

        //     _lockedRoom = scctparking;
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
            private Room _bossRoom;
            private Room _lockedRoom;

            public WorldEvent(Room trigger, Room worldOut, Room worldInAnnex, string directionFromWorld, string directionToWorld, Room combatRoom, Room lockedRoom)
            {
                _trigger = trigger;
                _worldOut = worldOut;
                _worldInAnnex = worldInAnnex;
                _directionFromWorld = directionFromWorld;
                _directionToWorld = directionToWorld;
                _bossRoom = combatRoom;
                _lockedRoom = lockedRoom;
            }


            public void ExecuteEvent()
            {
                _worldOut = _lockedRoom;
                _lockedRoom = null;
            }

        }
        //use room's .drop function to add items to rooms. Can use the same for shops?

    }
}

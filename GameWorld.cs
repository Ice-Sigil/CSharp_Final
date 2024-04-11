using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace StarterGame
{
    public class GameWorld
    {
        int _width,_height;
        Room[,] floor;
        public int Width { get {return _width;} set {_width = value;} }
        public int Height { get {return _height;} set {_height = value;} }
        public Room[,] Floor{get { return floor; } set { floor = value; } }
        public string west = "west";
        public string east = "east";
        public string south = "south";
        public string north = "north";
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
        private Room _worldOut;
        private Room _worldInAnnex;

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
                    player.InfoMessage("\n***Moving to next floor...");
                }
            }
        }

        private void CreateWorld()
        {
            Random random = new Random();
            _height = 5;//random.Next(5,7); //height can be from 5 to 7
            _width = 3;                 //the width is gonna be the same so it doesnt take too long to complete the floor, will change after project is over
            int direction;

            floor = new Room[_height,_width];
            /*
                #E# <-- _entrance floor[0,width-2]         
                ###                
                ###                
                ###         
                #X# <-- _worldOut floor[height-1,width-2]

            */
            floor[0,1] = new Room("at the beginning of the floor");
            _entrance = floor[0,1];

            floor[_height-1,_width-2] = new Room("at the exit");
            _worldOut = floor[_height-1,_width-2];
            direction = 5;

            for(int i = 1; i < _height; i++)
            {
                for(int j = 0; j < _width; j++)
                {
                    if(i == _height-1) {break;}
                    
                    if(direction <= 2)
                    {
                        floor[i,0] = new Room(tile);
                        floor[i,1] = new Room(tile);
                        direction = random.Next(3,9);
                    }
                    else if(direction >= 8)
                    {
                        floor[i,1] = new Room(tile);
                        floor[i,2] = new Room(tile);
                        direction = random.Next(1,7);
                    }
                    else
                    {
                        floor[i,1] = new Room(tile);
                        direction = random.Next(1,9);
                    }
                }
            }
            Map(_height,_width,floor);
        }
        public void Map(int height, int width, Room[,] floor)
        {
            for(int i = 0; i < height; i++)
            {
                Console.WriteLine();
                for(int j = 0; j < width; j++)
                {
                    if(floor[i,j] == null)
                    {
                        Console.Write("#");
                    }
                   // if(Player.CurrentRoom)
                    else
                    {
                        Console.Write("O");
                    }
                }
            }
            Console.WriteLine();
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

        //     outside.SetExit("west", boulevard);

        //     boulevard.SetExit("east", outside);
        //     boulevard.SetExit("south", scctparking);
        //     boulevard.SetExit("west", theGreen);
        //     boulevard.SetExit("north", universityParking);

        //     scctparking.SetExit("west", scct);
        //     scctparking.SetExit("north", boulevard);

        //     scct.SetExit("east", scctparking);
        //     scct.SetExit("north", schuster);

        //     schuster.SetExit("south", scct);
        //     schuster.SetExit("north", universityHall);
        //     schuster.SetExit("east", theGreen);

        //     theGreen.SetExit("west", schuster);
        //     theGreen.SetExit("east", boulevard);

        //     universityHall.SetExit("south", schuster);
        //     universityHall.SetExit("east", universityParking);

        //     universityParking.SetExit("south", boulevard);
        //     universityParking.SetExit("west", universityHall);
        //     universityParking.SetExit("north", parkingDeck);

        //     parkingDeck.SetExit("south", universityParking);
        //     //set up room delegates
        //     TrapRoom tp = new TrapRoom();
        //     scct.RoomDelegate = tp;

        //     //Build annex world
        //     davidson.SetExit("west", clockTower);
        //     clockTower.SetExit("north", greekCenter);
        //     clockTower.SetExit("south", woodhall);

        //     greekCenter.SetExit("north", clockTower);

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

            public WorldEvent(Room trigger, Room worldOut, Room worldInAnnex, string directionFromWorld, string directionToWorld)
            {
                _trigger = trigger;
                _worldOut = worldOut;
                _worldInAnnex = worldInAnnex;
                _directionFromWorld = directionFromWorld;
                _directionToWorld = directionToWorld;
            }

            public void ExecuteEvent()
            {
                _worldOut.SetExit(_directionFromWorld, _worldInAnnex);
                _worldInAnnex.SetExit(_directionToWorld, _worldOut);
            }

        }

    }
}
using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class Room
    {
        private Dictionary<string, Room> _exits;
        private string _tag;
        public string Tag { get { return _tag; } set { _tag = value; } }

        public Room() : this("No Tag"){}
        public ItemClass itemOnFloor;
        // Designated Constructor
        public Room(string tag)
        {
            itemOnFloor = null;
            _exits = new Dictionary<string, Room>();
            this.Tag = tag;
        }

        public ItemClass drop(ItemClass item){
            ItemClass oldItem = itemOnFloor;
            itemOnFloor = item;
            return oldItem;
        }

        public ItemClass pickup(string itemName){
            ItemClass oldItem = null;
            if(itemOnFloor != null){
                if(itemOnFloor.Name.Equals(itemName)){
                    oldItem=itemOnFloor;
                    itemOnFloor = null;
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
            return "You are " + this.Tag + ".\n *** " + this.GetExits();
        }
    }
}

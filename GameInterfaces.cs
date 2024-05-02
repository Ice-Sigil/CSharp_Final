using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterGame
{
    public interface IRoomDelegate
    {
        public Room ContainingRoom {get; set;}
        public Room OnGetExit(Room room);
        public string OnTag(string tag);
    }

    public interface IItem
    {
        public string Name {get;set;}
        public float Weight {get;}
        public string Description {get;}
        public void Decorate(IItem decorator);
        public bool IsContainer { get; }
        public int Count { get; set; }
        public int HealAmount { get; set; }

    }

    public interface IItemContainer : IItem
    {
        public bool Insert(IItem item);
        public IItem Remove(string itemName);
    }
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterGame
{
    public class Item : IItem
    {
        private string _name;
        public string Name { get {return _name;} set {_name = value;}}
        private float _weight;
        public float Weight {get {return _weight + (_decorator==null?0:_decorator.Weight);}}
        public string Description { get {return Name + ", weight = " + Weight;}}
        private IItem _decorator;
        public bool IsContainer { get { return false; }}
        public Item() : this("Nameless") {}
        public Item(string name) : this(name, 1f) {}
        public Item(string name, float weight)
        {
            Name = name;
            _weight = weight;
            _decorator = null;
        }

        public void Decorate(IItem decorator)
        {
            if(_decorator == null)
            {
                _decorator = decorator;
            }
            else
            {
                _decorator.Decorate(decorator);
            }

        }
    }

    public class ItemContainer : Item, IItemContainer
    {
        private Dictionary<string, IItem> _items;
        public new bool IsContainer { get { return true; } }
        public new float Weight
        {
            get
            {
                float myWeight = base.Weight;
                foreach(IItem item in _items.Values)
                {
                    myWeight += item.Weight;
                }
                return myWeight;
            }
        }
        public new string Description
        {
            get
            {
                string itemNames = "";
                foreach(string name in _items.Keys)
                {
                    itemNames += " " + name;
                }
                return Name + ", weight = " + Weight + "\n" + itemNames;
            }
        }
        public ItemContainer() : this("nameless") { }
 
        public ItemContainer(string name) : this(name, 1f){ }

        //Designated Constructor
        public ItemContainer(string name, float weight) : base(name, weight)
        {
            _items = new Dictionary<string, IItem>();
        }

        public bool Insert(IItem item)
        {
            _items[item.Name] = item;
            return true;
        }

        public IItem Remove(string itemName)
        {
            IItem itemToRemove = null;
            _items.TryGetValue(itemName, out itemToRemove);
            if(itemToRemove != null)
            {
                _items.Remove(itemName);
            }
            return itemToRemove;
        }
    }
}
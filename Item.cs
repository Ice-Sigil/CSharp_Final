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
        private int _price;
        private int Price {get {return _price;} set {_price = value;}}
        public string Description { get {return Name + ", weight = " + Weight;}}
        private IItem _decorator;
        public bool IsContainer { get { return false; }}
        public bool IsUsable;
        private int _useValue;
        public int UseValue { get {return _useValue;} set {_useValue = value;}}
        private int _count;
        public int Count { get {return _count;} set {_count = value;}}
        private int _cost;
        public int Cost { get {return _cost;} set {_cost = value;}}
        public Item() : this("Nameless") {}
        public Item(string name) : this(name, 1f, 0, 0) {}
        
        public Item(string  name, float weight, int amount, int cost){
            Name = name;
            _weight = weight;
            _decorator = null;
            UseValue = amount;
            Cost = cost;
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
        override public string ToString(){
            return Name + " x" + Count + " "; 
        }
    }

    public class ItemContainer : Item, IItemContainer
    {
        private Dictionary<string, IItem> _items;
        public Dictionary<string, IItem> Items{
            get { return _items; }
            set { _items = value; }
        }
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
                string itemsInContainer = "";
                foreach (Item item in _items.Values)
                {
                    itemsInContainer += item.ToString();
                }
                return Name + ", weight = " + Weight + "\n" + itemsInContainer;
            }
        }
        public ItemContainer() : this("nameless") { }
 
        public ItemContainer(string name) : this(name, 1f){ }

        //Designated Constructor
        public ItemContainer(string name, float weight) : base(name, weight, 0, 0)
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
                itemToRemove.Count--;
            }
            return itemToRemove;
        }
    }
}
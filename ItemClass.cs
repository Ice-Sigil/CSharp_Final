using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame{
    public class ItemClass{
        private string _name;

        public string Name {get {return _name;} set {_name = value;}}

        private float _weight;

        public float Weight{get {return _weight;}}

        public string Description{get {return Name + ", weight = "+ _weight;}}

        public ItemClass() : this("Nameless") { }

        public ItemClass(string name) {
            _name = name;
        }
    }
}
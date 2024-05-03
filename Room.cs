using System.Collections;
using System.Collections.Generic;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace StarterGame{
    /*
     * Spring 2024
     */
    public class Room{
        private Dictionary<string, Room> _exits;
        private string _tag;
        public string Tag 
        { 
            get 
            { 
                return _roomDelegate==null?_tag:_roomDelegate.OnTag(_tag); 
            } 
            set 
            { 
                _tag = value; 
            } 
        }
        private IRoomDelegate _roomDelegate;
        public IRoomDelegate RoomDelegate
        {
            get {return _roomDelegate;}
            set 
            {
                if(value == null)
                {
                    if(_roomDelegate == null)
                    {
                        _roomDelegate.ContainingRoom = null; //Un-designates a delegated room if there's no delegate attached to it
                    }
                }
                else
                {
                    if(value.ContainingRoom != null)
                    {
                        value.ContainingRoom = null;
                    }
                    value.ContainingRoom = this;
                }
                _roomDelegate = value;
            }
        }
        private IItemContainer _itemsOnFloor;
        public Room() : this("No Tag"){}

        // Designated Constructor
        public Room(string tag)
        {
            _itemsOnFloor = new ItemContainer("floor", 0f);
            _roomDelegate = null;
            _exits = new Dictionary<string, Room>();
            this.Tag = tag;
        }

        public IItem Drop(IItem item)
        {
            IItem oldItem = _itemsOnFloor.Remove(item.Name); //Stores the old item if there's already one on the ground
            _itemsOnFloor.Insert(item); //Puts the selected item onto the ground
            return oldItem; //Gives you the old one if one was there so it's not permanently lost
        }

        public IItem Pickup(string itemName)
        {
            return _itemsOnFloor.Remove(itemName); //Gives you the item that was on the ground
        }

        public void SetExit(string exitName, Room room)
        {
            _exits[exitName] = room; //Sets a room's exit
        }

        public Room GetExit(string exitName)
        {
            Room room = null;
            _exits.TryGetValue(exitName, out room); //Try to get the exit object with the key of the name of the exit
            if(_roomDelegate != null)
            {
                room = _roomDelegate.OnGetExit(room);
            }
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
            return "You are " + this.Tag + ".\n *** " + this.GetExits() + "\nItems: " + _itemsOnFloor.Description;
        }
    }

    public class TrapRoom : IRoomDelegate
    {
        private bool _active;
        private Room _containingRoom;
        public Room ContainingRoom
        {
            set
            {
                _containingRoom = value;
            }
            get
            {
                return _containingRoom;
            }
        }
        private string magicWord;
        public TrapRoom(string magicWord)
        {
            _active = false;
            NotificationCenter.Instance.AddObserver("PlayerDidSayAWord", PlayerDidSayAWord);
            this.magicWord = magicWord;
        }
        public Room OnGetExit(Room room)
        {
            return _active?null:room;
        }
        public void PlayerDidSayAWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player != null)
            {
                if(player.CurrentRoom == ContainingRoom)
                {
                    Dictionary<string,object> userInfo = notification.UserInfo;
                    if(userInfo != null)
                    {
                        object spokenWordObject = null;
                        userInfo.TryGetValue("word", out spokenWordObject);
                        if(spokenWordObject != null)
                        {
                            string spokenWord = (string) spokenWordObject;
                            if(spokenWord.Equals(magicWord))
                            {
                                _active = false;
                                player.InfoMessage("You disabled the trap");
                            }
                            else
                            {
                                player.ErrorMessage("Nope, not the magic word :)");
                            }
                        }
                    }
                }
            }
        }
        public string OnTag(string tag)
        {
            return tag + (_active?".\n *** You are in a trap room! You must \"say\" the magic word to unlock the doors":"");
        }
    }
    public class CombatRoom : IRoomDelegate
    {
        private Enemy _enemy; 
        Random rand = new Random();
        int coins;
        int _xp;
        public Enemy Enemy {
            get { return _enemy; }
            set { _enemy = value; }
        }
        private bool _active;
        private bool _escape = false;
        private Room _containingRoom;
        public Room ContainingRoom
        {
            set
            {
                _containingRoom = value;
            }
            get
            {
                return _containingRoom;
            }
        }
        public CombatRoom(Enemy enemy)
        {
            _active = true;
            _enemy = enemy;
            NotificationCenter.Instance.AddObserver("PlayerDidStartCombat", PlayerDidStartCombat);
        }

        public void PlayerDidStartCombat(Notification notification){
            Player player = (Player)notification.Object;
            if (_active){
                if (player != null){
                    if(player.CurrentRoom == ContainingRoom){
                        StartCombat(player); 
                        _active = false;
                    }
                }
            }
        }

        public Room OnGetExit(Room room)
        {
            return _active?null:room;
        }

        public string OnTag(string tag)
        {


            return tag + (_active?".\n *** You have entered a Combat room! You cannot escape until the Enemy is defeated.":"");

        }
        public void StartCombat(Player player){
        while (player.HP > 0 && _enemy.HP > 0 && !_escape){
            CombatMenu(player, _enemy);
            string? playerAction = Console.ReadLine();
            switch (playerAction?.ToLower())
            {
                case "a":
                    //Attack
                    Console.WriteLine("You swing with your weapon! ");
                    //NEED ATTACK COMMAND(maybe)
                    int dmg = player.Attack(_enemy);
                    Console.WriteLine("Dealing " + dmg + " damage!");
                    break;
                case "i":
				    player.Inventory();
                    break;
                case "r":
                    /*//Run
                    // NEED to finish RUN method
                    player.Run(enemy);*/
                    if (player.CurrentRoom != GameWorld.Instance._worldOut)
                    {
                        _escape = false;
                        Random chance = new Random();
                        switch (chance.Next(0, 3))
                        {
                            case 1:
                                player.WarningMessage("Escape failed..!");
                                break;
                            case 2:
                                player.InfoMessage("You escaped, but barely... [-2 HP]");
                                player.HP -= 2;
                                _escape = true;
                                break;
                            default:
                                player.InfoMessage("You escape successfully...");
                                _escape = true;
                                break;
                        }
                    }
					else
					{
						player.WarningMessage("You can't escape the boss battle!?");
					}
                    break;
                default:
                    break;
            }
            Console.ReadKey();  
            if(_enemy.HP > 0){
                //if the _enemy is alive they retaliate
                //in the future implement different actions other than Attack
                Console.WriteLine("The enemy swings!");
                int eDmg = _enemy.Attack(player);
                Console.WriteLine("You take " + eDmg + " damage!");
                Console.ReadKey(); 
            }
        }
        if (player.HP > 0 && _escape == false){    
            coins = rand.Next(10,30); 
            // absurd leveling to see where it goes
            _xp = rand.Next(13,50); 
            //NEED GAINXP(), GAINCOIN(), AND LEVELCHECK() FUNCTIONS FROM MY PROTO PLAYER CLASS
            Console.WriteLine("You are victorious!");  
            Console.WriteLine("You gained " + _xp + " Exp and " + coins + " coins!");
            player.GainXP(_xp);
            player.GainCOIN(coins);
            //player.LevelCheck(player); 
            Console.ReadKey(); 
        }
        else if(_escape == true)
        {
            coins = 0;
            _xp = 0;
			Console.WriteLine("You gain 0 coins and XP.");
            Console.ReadKey();
        }
        else{
            //Death Check 
            Console.WriteLine("You died!");
            Console.ReadKey(); 
            Environment.Exit(0); 
        }
    }
    public void CombatMenu(Player player, Enemy enemy){
        Console.Clear(); 
        Console.WriteLine(enemy.Name);
        Console.WriteLine("HP: " + enemy.HP);
        Console.WriteLine("=======================");
        Console.WriteLine("|| (A)ttack (I)tems  ||");
        Console.WriteLine("||       (R)un       ||");
        Console.WriteLine("=======================");
        Console.WriteLine("Name: " + player.Name);
        Console.WriteLine("Level: " + player.LVL);
        Console.WriteLine("Exp: " + player.XP + " / " + player.MXP);
        Console.WriteLine("HP: " + player.HP + " / "  + player.MHP);
        }   
    }
    // barrier for shop class
    public class ShopRoom : IRoomDelegate
    {
        Random random = new Random();
        private Shopkeeper _shopkeeper;
        private IItemContainer shopInventory = new ItemContainer("Shop's Inventory:");
        private IItem[] useableItems = GameWorld.getUseableItems();
        private IItem[] nonUsableItems = GameWorld.getNonUsableItems();
        public Shopkeeper Shopkeeper {
            get { return _shopkeeper; }
            set { _shopkeeper = value; }
        }
        private bool _active;
        private Room _containingRoom;
        public Room ContainingRoom
        {
            set
            {
                _containingRoom = value;
            }
            get
            {
                return _containingRoom;
            }
        }
        public ShopRoom(Shopkeeper shopkeeper)
        {
            _active = true;
            _shopkeeper = shopkeeper;
            NotificationCenter.Instance.AddObserver("PlayerDidEnterShop", PlayerDidEnterShop);
        }

        public void PlayerDidEnterShop(Notification notification){
            Player player = (Player)notification.Object;
            if (_active){
                if (player != null){
                    if(player.CurrentRoom == ContainingRoom){
                        ShopLoop(player, _shopkeeper);
                    }
                }
            }
        }

        public Room OnGetExit(Room room)
        {
            return room;
        }

        public string OnTag(string tag)
        {
            return tag + (_active?".\n *** You have entered a Shop Room! You are free to purchase whatever you please.":"");

        }
        public void ShopMenu(){
        //Console.Clear(); 
        Console.WriteLine("=======================");
        Console.WriteLine("|| (B)uy      (S)ell ||");
        Console.WriteLine("||     (G)oodbye     ||");
        Console.WriteLine("=======================");
        }
        public void ShopLoop(Player player, Shopkeeper shopkeeper){
            Random random = new Random();
            shopInventory.Insert(useableItems[random.Next(useableItems.Length-1)]);
            shopInventory.Insert(useableItems[random.Next(useableItems.Length-1)]);
            shopInventory.Insert(nonUsableItems[random.Next(nonUsableItems.Length-1)]);
            ShopMenu();
            string? playerInput = Console.ReadLine().ToLower();
            while(playerInput != "g"){
                ShopMenu();
                switch(playerInput){
                    case "b":
                        bool isBuying = true;
                        while(isBuying){
                            player.NormalMessage("Press q to exit the buy menu. Current Inventory: \n");
                            player.InventoryDisplay();
                            playerInput = Console.ReadLine().ToLower();
                            player.InfoMessage("Shopkeeper: This is what we have right now. Take a look.");
                            player.NormalMessage(shopInventory.Description +"\n");
                            if(playerInput == "q"){
                                isBuying = false;
                            }
                        }
                        break;
                    case "s":
                        bool isSelling = true;
                        while(isSelling){
                            player.NormalMessage(shopInventory.Description);
                            playerInput = Console.ReadLine().ToLower();
                            if(playerInput == "q"){
                            isSelling = false;
                            }
                        }
                        break;
                }
            }
        }
    }
 }



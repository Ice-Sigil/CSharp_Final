namespace StarterGame;
public class Enemy : Character{
    public int LootChance { get; private set; } // Represents the chance of dropping loot
<<<<<<< HEAD
    public Enemy() : this("No Name") { }
    public Enemy(string name) : this(name, 0) { }
    public Enemy(string name, int hp) :this(name, hp, 0) { }
    public Enemy(string name, int hp, int atk) :this(name, hp, atk, 0) { }
    public Enemy(string name, int hp, int atk, int def) :this(name, hp, atk, def, 0) { }
    public Enemy(string? name, int hp, int atk, int def, int lootChance){   
        Name = name;
        HP = hp;
        ATK = atk;
        DEF = def; 
=======
    public Enemy(string? firstName, string? lastName, 
        int hp, int atk, int def, int lootChance) : base(firstName, lastName, hp, atk, def){
>>>>>>> 6cd973d65662f4abdab59982174fced6f35913c7
        LootChance = lootChance; 
    }
}
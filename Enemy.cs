namespace StarterGame;
public class Enemy : Character{
    public int LootChance { get; private set; } // Represents the chance of dropping loot
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
        LootChance = lootChance; 
    }
}
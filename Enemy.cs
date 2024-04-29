namespace StarterGame;
public class Enemy : Character{
    public int LootChance { get; private set; } // Represents the chance of dropping loot
    public Enemy(string? name, int hp, int atk, int def) : base(name, hp, atk, def){ }
}
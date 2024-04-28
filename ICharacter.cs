namespace StarterGame;

public interface ICharacter{
    public string? Name { get; set; } //Character Name
    public int HP { get; set; } //Hit points
    public int ATK { get; set; } //Attack Attribute
    public int DEF { get; set; } //Defense Attribute
}


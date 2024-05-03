namespace StarterGame;
public class Enemy : Character{
    public int LootChance { get; private set; } // Represents the chance of dropping loot
    public Enemy(string name, int hp, int atk, int def) : base(name, hp, atk, def){ }

    private string[] _nameArr = {
        "Goblin",
        "Bandit",
        "Skeleton",
        "Gargoyle",
        "Flaming Imp"
    };
    public Enemy enemyFactory(int level)
    {
        Random rand = new Random();
        int hp = 0;
        int atk = 0;
        int def = 0;
        string name = _nameArr[rand.Next(0,4)];
        switch(level)
        {
            case 1:
                hp = rand.Next(15,25);
                atk = rand.Next(3,7);
                def = rand.Next(0,4);
                break;
            case 2:
                hp = rand.Next(25,45);
                atk = rand.Next(6,10);
                def = rand.Next(4,5);
                break;
            case 3:
                hp = rand.Next(40,55);
                atk = rand.Next(10,13);
                def = rand.Next(5,7);
                break;
            case 4:
                hp = rand.Next(50,75);
                atk = rand.Next(11,15);
                def = rand.Next(7,9);
                break;
            case 5:
                hp = rand.Next(80,100);
                atk = rand.Next(17,20);
                def = rand.Next(9,11);
                break;
        }
        Enemy enemy = new Enemy(name, hp, atk, def);
        return enemy;
    }
}
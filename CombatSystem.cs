using System.Collections;
using System.Collections.Generic;
using System;

// I have to fix a lot of this class in order for it to actually connect to this version. Will do next time we meet up. 
// -Dante

namespace StarterGame{
    public class CombatSystem{
    Random rand = new Random(); 
    public CombatSystem(){}
    public void StartCombat(Player player, Enemy enemy, bool random){
        if (random){
            //implement random combat with a given dictionary of enemies based on Floor Level     
        }
        else{
            //if not random proceed to the combat loop
        }
        while (player.HP > 0 && enemy.HP > 0){
            CombatMenu(player, enemy);  
            string? playerAction = Console.ReadLine(); 
            Console.ReadKey(); 
            switch (playerAction?.ToLower()){
                case "a":
                //Attack
                Console.WriteLine("You swing with your weapon! ");
                //NEED ATTACK COMMAND
                int dmg = player.Attack(enemy);
                Console.WriteLine("Dealing " + dmg + " damage!");
                break; 
                case "d":
                //Defend
                Console.WriteLine("You stand your ground!!!");
                //NEED DEFEND COMMAND
                dmg = player.Defend(enemy); 
                Console.WriteLine("You take " + dmg + " damage!");
                break;
                case "i":
                //Item
                break;
                case "r":
                //Run
                // NEED RUN COMMAND
                player.Run(enemy); 
                break;
                default:
                break; 
            }
            Console.ReadKey();  
            if(enemy.HP > 0){
                //if the enemy is alive they retaliate
                //in the future implement different actions other than Attack
                Console.WriteLine("The enemy swings!");
                int eDmg = enemy.Attack(player);
                Console.WriteLine("You take " + eDmg + " damage!");
                Console.ReadKey(); 
            }
        }
        if (player.HP > 0){    
            int coins = rand.Next(10,30); 
            // absurd leveling to see where it goes
            int _xp = rand.Next(13,50); 
            //NEED GAINXP(), GAINCOIN(), AND LEVELCHECK() FUNCTIONS FROM MY PROTO PLAYER CLASS
            player.GainXP(_xp);
            player.GainCOIN(coins);
            Console.WriteLine("You are victorious!");  
            Console.WriteLine("You gained " + _xp + " Exp and " + coins + " coins!");
            //player.LevelCheck(player); 
            Console.ReadKey(); 
        }
        else{
            //Death Check 
            Console.WriteLine("You died!");
            Console.ReadKey(); 
            Environment.Exit(0); 
        }
    }
    //NEED TO MAKE ENEMY CLASS
    //ADD PLAYER ATTRIBUTES
    public void CombatMenu(Player player, Enemy enemy){
        Console.Clear(); 
        Console.WriteLine(enemy.Name);
        Console.WriteLine("HP: " + enemy.HP);
        Console.WriteLine("=======================");
        Console.WriteLine("|| (A)ttack (D)efend ||");
        Console.WriteLine("||  (S)pells (I)tems ||");
        Console.WriteLine("||       (R)un       ||");
        Console.WriteLine("=======================");
        Console.WriteLine("Name: " + player.Name);
        Console.WriteLine("Level: " + player.LVL);
        Console.WriteLine("Exp: " + player.XP + " / " + player.MXP);
        Console.WriteLine("HP: " + player.HP + " / " + player.MHP);
        Console.WriteLine("MP: ");
        Console.WriteLine("Potions: ");
    }
}
}
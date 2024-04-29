using System;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            while(!quit)
            {
                string? input = "";
                bool valid = false;
                Game game = new Game();
                game.Start();
                game.Play();
                Console.WriteLine("Start a new game? Y/N");
                while (!valid)
                {
                    input = Console.ReadLine();
                    if(input.ToLower() == "n")
                    {
                        valid = true;
                        quit = true;
                    }
                    else if(input.ToLower() == "y")
                    {
                        valid = true;
                        Console.WriteLine("Starting new game...");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }   
                if(quit){ game.End(); }
            }
        }
    }
}

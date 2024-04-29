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
            Game game = new Game();
            game.Start();
            game.Play();
            game.End();
        }
    }
}

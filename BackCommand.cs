using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class BackCommand : Command
    {

        public BackCommand() : base()
        {
            this.Name = "back";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.WarningMessage("\n'back' doesn't take any arguments.");
            }
            else
            {
                player.GoBack(player);
            }
            return false;
        }
    }
}

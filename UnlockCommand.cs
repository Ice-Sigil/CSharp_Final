using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class UnlockCommand : Command
    {

        public UnlockCommand() : base()
        {
            this.Name = "unlock";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.WarningMessage("'unlock' takes no arguments!");
            }
            else
            {
                player.Unlock(player);
            }
            return false;
        }
    }
}

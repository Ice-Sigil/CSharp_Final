using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class InspectCommand : Command
    {

        public InspectCommand() : base()
        {
            this.Name = "inspect";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Inspect(this.SecondWord);
            }
            else
            {
                player.WarningMessage("\nInspect what?");
            }
            return false;
        }
    }
}

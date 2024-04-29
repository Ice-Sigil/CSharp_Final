using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class InventoryCommand : Command
    {

        public InventoryCommand() : base()
        {
            this.Name = "inventory";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.WarningMessage("I cannot do an inventory on " + SecondWord);
            }
            else
            {
                player.Inventory();
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterGame
{
    public class PickupCommand : Command
    {

        public PickupCommand() : base()
        {
            this.Name = "Pickup";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Pickup(SecondWord);
            }
            else
            {
                player.ErrorMessage(" I cannot pick up nothing.");
            }
            return false;
        }
    }
}
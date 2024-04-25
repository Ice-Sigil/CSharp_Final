using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterGame
{
    public class DropCommand : Command
    {

        public DropCommand() : base()
        {
            this.Name = "Drop";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Drop(SecondWord);
            }
            else
            {
                player.ErrorMessage(" I cannot drop up nothing.");
            }
            return false;
        }
    }
}
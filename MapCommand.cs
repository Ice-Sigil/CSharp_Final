using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterGame
{
    public class MapCommand : Command
    {

        public MapCommand() : base()
        {
            this.Name = "map";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.WarningMessage("Incorrect parameters. (just use \"map\")");
            }
            else
            {
                GameWorld.Instance.Map(player);
            }
            return false;
        }
    }
}
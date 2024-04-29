using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterGame
{
    public class StartCommand : Command
    {

        public StartCommand() : base()
        {
            this.Name = "start";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.SetDifficulty(SecondWord);
                return true;
            }
            else
            {
                player.ErrorMessage("You need a difficulty!");
            }
            return false;
        }
    }
}
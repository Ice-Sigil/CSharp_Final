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
                if(this.SecondWord == "easy" || this.SecondWord == "medium" || this.SecondWord == "hard")
                {
                    player.SetDifficulty(SecondWord);
                    GameWorld.Instance.Map(player); 
                    player.InfoMessage(player.CurrentRoom.Description());
                    return true;
                }
                else
                {
                    player.ErrorMessage("Difficulty not on the list!");
                }
            }
            else
            {
                player.ErrorMessage("You need a difficulty!");
            }
            return false;
        }
    }
}

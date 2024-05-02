using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterGame{
    public class StatsCommand : Command{

        public StatsCommand() : base(){
            this.Name = "stats";
        }

        override

        public bool Execute(Player player){
            if(this.HasSecondWord()){
                player.ErrorMessage("I don't know how to stats " + SecondWord);
            }
            else{
                player.DisplayPlayerStats();
            }
            return false;
        }
    }
}
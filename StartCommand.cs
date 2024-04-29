using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarterGame;

namespace CSharp_Final{
    public class StartCommand : Command{
        public StartCommand() : base()
        {
            this.Name = "start";
        }

        public override bool Execute(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
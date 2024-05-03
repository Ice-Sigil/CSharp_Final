using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterGame
{
	public class NextCommand : Command
	{

		public NextCommand() : base()
		{
			this.Name = "next";
		}

		override
		public bool Execute(Player player)
		{
			if (this.HasSecondWord())
			{
				player.WarningMessage("Incorrect parameters (just use \"next\"");
			}
			else
			{
				return GameWorld.Instance.NextFloor(player);
				
			}
			return false;
		}
	}
}
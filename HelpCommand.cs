using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class HelpCommand : Command
    {
        private CommandWords _words;

        public HelpCommand() : this(new CommandWords()){}

        // Designated Constructor
        public HelpCommand(CommandWords commands) : base()
        {
            _words = commands;
            this.Name = "help";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.WarningMessage("\nPlease input 'help' as a command by itself.");
            }
            else
            {
                player.InfoMessage("\nThe walls of the dungeon threaten to crush your spirit. You must continue on. \n\nYour available commands are " + _words.Description());
            }
            return false;
        }
    }
}

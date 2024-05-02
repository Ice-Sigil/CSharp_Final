using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    /*
     * Spring 2024
     * Foundation Class that holds the Player and Parser
     *
     *
     */

    public class Game
    {
        private Player _player;
        private Parser _parser;
        private bool _playing;
        private bool _gameStart;

        public Game()
        {
            _playing = false;
            _parser = new Parser(new CommandWords());
            _player = new Player(GameWorld.Instance.Entrance, "Prisoner", 30, 16, 8);
        }

        public void Play()
        {

            // Enter the main command loop.  Here we repeatedly read commands and
            // execute them until the game is over.
            if(_playing)
            {
                bool finished = false;
                while (!finished)
                {
                    Console.Write("\n>");
                    Command command = _parser.ParseCommand(Console.ReadLine());
                    if(_gameStart)
                    {
                        if (command == null)
                        {
                            _player.ErrorMessage("I don't understand...");
                        }
                        else
                        {
                            finished = command.Execute(_player);
                        }
                    }
                    else
                    {
                        if(command == null)
                        {
                            _player.WarningMessage("That's not a command...");
                        }
                        else if(command.ToString() == "start" || command.ToString() == "help")
                        {
                            _gameStart = command.Execute(_player);
                        }
                        else
                        {
                            _player.WarningMessage("You can't use that command yet...");
                        }
                    }
                    
                }
            }

        }

        public void Start()
        {
            _playing = true;
            _player.InfoMessage(Welcome());
            _gameStart = false;
        }

        public void End()
        {
            _playing = false;
            _player.InfoMessage(Goodbye());
        }

        public string Welcome()
        {
            return "Welcome to Dante-Jackson-NickTopia!\n\nType 'start' and your difficulty to begin [easy, medium, hard]\n\nType 'help' if you need help.";
        }

        public string Goodbye()
        {
            return "\nThank you for playing, Goodbye. \n";
        }

    }
}


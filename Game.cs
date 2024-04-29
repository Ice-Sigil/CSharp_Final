﻿using System.Collections;
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
            _player = new Player(GameWorld.Instance.Entrance, "Prisoner", 30, 12, 6);
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
                            Console.WriteLine("Nope!");
                        }
                        else if(command.ToString() == "start")
                        {
                            _gameStart = command.Execute(_player);
                        }
                        else
                        {
                            Console.WriteLine("Nope!");
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
            return "Welcome to the World of CSU!\n\n The World of CSU is a new, incredibly boring adventure game.\n\nType 'help' if you need help." + _player.CurrentRoom.Description();
        }

        public string Goodbye()
        {
            return "\nThank you for playing, Goodbye. \n";
        }

    }
}

using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using PlayerGameAkkaRemote.Messages;
using FluentColorConsole;

namespace PlayerGameAkkaRemote.Actors
{
    internal class GameActor : ReceiveActor
    {
        private List<IActorRef> _players = new List<IActorRef>(); 

        public GameActor()
        {
            Receive<Join>(m => HandleJoin(m));
            Receive<Leave>(m => HandleLeave(m));
            Receive<GetPlayers>(m => HandleGetPlayers(m));
            ColorConsole.WithYellowText.WriteLine($"Game created...");
        }

        private void HandleGetPlayers(GetPlayers m)
        {
            ColorConsole.WithYellowText.WriteLine($"Returning player list...");
            Sender.Tell(new PlayerList(_players)); 
        }

        private void HandleLeave(Leave m)
        {
            _players.Remove(Sender); 
            ColorConsole.WithYellowText.WriteLine($"Player {Sender.Path} has left the game...");
            ListPlayers();
        }

        private void HandleJoin(Join m)
        {            
            if (!_players.Contains(Sender))
            {
                foreach (IActorRef player in _players)
                {
                    player.Tell(new PlayerJoined(Sender));
                }

                _players.Add(Sender);
                ColorConsole.WithYellowText.WriteLine($"Player {Sender.Path} has joind the game...");
            }

            ListPlayers();
        }

        private void ListPlayers()
        {
            ColorConsole.WithGreenText.WriteLine("Current list of players:");
            foreach (IActorRef player in _players)
            {
                ColorConsole.WithGreenText.WriteLine(player.Path);
            }
        }
    }
}

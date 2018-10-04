using Akka.Actor;
using PlayerGameAkka.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using FluentColorConsole;

namespace PlayerGameAkka.Actors
{
    internal class GameActor : ReceiveActor
    {
        private List<IActorRef> _players = new List<IActorRef>(); //Internal state is always private, but must
                                                                  //not be thread safe

        public GameActor()
        {
            Receive<Join>(m => HandleJoin(m));
            Receive<Leave>(m => HandleLeave(m));
            Receive<GetPlayers>(m => HandleGetPlayers(m));
            ColorConsole.WithYellowText.WriteLine($"Game created...");
        }

        private void HandleGetPlayers(GetPlayers m)
        {
            Sender.Tell(new PlayerList(_players)); //DANGER!
        }

        private void HandleLeave(Leave m)
        {
            _players.Remove(Sender); // No thread safety needed!
            ColorConsole.WithYellowText.WriteLine($"Player {Sender.Path} has left the game...");
        }

        private void HandleJoin(Join m)
        {
            if (!_players.Contains(Sender))
            {
                _players.Add(Sender);
                ColorConsole.WithYellowText.WriteLine($"Player {Sender.Path} has joind the game...");
            }
        }
    }
}

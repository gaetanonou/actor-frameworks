using Akka.Actor;
using FluentColorConsole;
using PlayerGameAkka.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerGameAkka.Actors
{
    internal class PlayerActor : ReceiveActor
    {
        IActorRef _game = null; //Private state, can be remote or local actor!

        public PlayerActor()
        {
            Receive<Command>(m => HandleCommand(m));
            ColorConsole.WithYellowText.WriteLine($"Player created...");
        }

        private void HandleCommand(Command m)
        {
            switch (m.Key)
            {
                case 'j':
                    {
                        _game = m.Game;
                        _game.Tell(new Join());
                    }
                    break;
                case 'l':
                    {
                        _game.Tell(new Leave());
                    }
                    break;
                case 'p':
                    {
                        var GetPlayersTask = _game.Ask<PlayerList>(new GetPlayers());
                        GetPlayersTask.Wait();
                        ColorConsole.WithGreenText.WriteLine("Following players are in the game:");
                        foreach(IActorRef player in GetPlayersTask.Result.Players)
                        {
                            ColorConsole.WithGreenText.WriteLine(player.Path);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

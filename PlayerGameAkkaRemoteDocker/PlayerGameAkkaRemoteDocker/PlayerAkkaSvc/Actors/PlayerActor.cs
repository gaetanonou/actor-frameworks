using Akka.Actor;
using FluentColorConsole;
using System;
using System.Collections.Generic;
using System.Text;
using PlayerGameAkkaRemote.Messages;

namespace PlayerAkkaSvc.Actors
{
    internal class PlayerActor : ReceiveActor
    {
        IActorRef _game = null; //Private state, can be remote or local actor!

        public PlayerActor()
        {
            Receive<Command>(m => HandleCommand(m));
            Receive<PlayerJoined>(m => HandlePlayerJoined(m));
            ColorConsole.WithYellowText.WriteLine($"Player created...");
        }

        private void HandlePlayerJoined(PlayerJoined m)
        {
            ColorConsole.WithGreenText.WriteLine($"Player {m.Player.Path} joined the game");
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
                        _game = null;
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

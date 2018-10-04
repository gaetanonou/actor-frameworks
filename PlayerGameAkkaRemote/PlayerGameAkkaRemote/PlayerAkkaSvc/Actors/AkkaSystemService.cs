using Akka.Actor;
using FluentColorConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayerGameAkkaRemote.Messages;

namespace PlayerAkkaSvc.Actors
{
    public class AkkaSystemService
    {
        private ActorSystem _actorSystem = null;
        //private IActorRef _player1 = null;
        //private IActorRef _player2 = null;
        //private IActorRef _player3 = null;
        private Dictionary<int, IActorRef> _players = new Dictionary<int, IActorRef>();
        private IActorRef _game = null; 

        public AkkaSystemService()
        {
            _actorSystem = ActorSystem.Create("akka-demo-playerservice",
                @"akka {  
                    stdout-loglevel = DEBUG
                    loglevel = DEBUG
                    log-config-on-start = on
                    actor {
                        provider = remote
                        debug {
                            receive = on
                            autoreceive = on
                            lifecycle = on
                            event-stream = on
                            unhandled = on
                        }
                    }
                    remote {
                        dot-netty.tcp {
                            port = 9090
                            hostname = localhost
                        }
                    }
            ");

            //Resolve on remote system !!
            var _gameSelection = _actorSystem.ActorSelection("akka.tcp://akka-demo-gameserver@localhost:8080/user/game1");
            var _gameIdentificationTask = _gameSelection.Ask<ActorIdentity>(new Identify(0));
            _gameIdentificationTask.Wait();
            _game = _gameIdentificationTask.Result.Subject;
            ColorConsole.WithYellowText.WriteLine("Resolved remote game actor");

            //We will create players dynamically now
            //_player1 = _actorSystem.ActorOf<PlayerActor>("player1");
            //_player2 = _actorSystem.ActorOf<PlayerActor>("player2");
            //_player3 = _actorSystem.ActorOf<PlayerActor>("player3");
        }           

        public void Join(int playerId)
        {
            if (!_players.ContainsKey(playerId))
            {
                IActorRef player = _actorSystem.ActorOf<PlayerActor>("player" + playerId.ToString());
                _players[playerId] = player;
                player.Tell(new Command('j', _game));
            }
        }

        public void Leave(int playerId)
        {
            if (_players.ContainsKey(playerId))
            {
                IActorRef player = _players[playerId];
                player.Tell(new Command('l', null));
            }
        }

        internal void ListPlayers(int playerId)
        {
            if (_players.ContainsKey(playerId))
            {
                IActorRef player = _players[playerId];
                player.Tell(new Command('p', null));
            }
        }
    }
}

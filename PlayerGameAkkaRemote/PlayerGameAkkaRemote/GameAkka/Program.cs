using Akka.Actor;
using PlayerGameAkkaRemote.Actors;
using System;

namespace GameAkka
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSystem actorSystem = ActorSystem.Create("akka-demo-gameserver",
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
                            port = 8080
                            hostname = localhost
                        }
                    }
            ");

            IActorRef game = actorSystem.ActorOf<GameActor>("game1");
            Console.ReadKey();

            actorSystem.Terminate().Wait();
            Console.ReadKey();
        }
    }
}

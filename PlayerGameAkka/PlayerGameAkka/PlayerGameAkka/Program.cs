using Akka.Actor;
using PlayerGameAkka.Actors;
using System;


namespace PlayerGameAkka
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSystem actorSystem = ActorSystem.Create("akka-demo", 
                @"akka {  
                    stdout-loglevel = DEBUG
                    loglevel = DEBUG
                    log-config-on-start = on
                    actor {
                        debug {
                            receive = on
                            autoreceive = on
                            lifecycle = on
                            event-stream = on
                            unhandled = on
                        }
                    }
            ");

            IActorRef game = actorSystem.ActorOf<GameActor>("game1");
            IActorRef player1 = actorSystem.ActorOf<PlayerActor>("player1");
            IActorRef player2 = actorSystem.ActorOf<PlayerActor>("player2");
            IActorRef player3 = actorSystem.ActorOf<PlayerActor>("player3");
            Console.ReadKey();

            player1.Tell(new Messages.Command('j', game));
            Console.ReadKey();

            player2.Tell(new Messages.Command('j', game));
            Console.ReadKey();

            player2.Tell(new Messages.Command('p', game));
            Console.ReadKey();

            actorSystem.Terminate().Wait();
            Console.ReadKey();
        }
    }
}

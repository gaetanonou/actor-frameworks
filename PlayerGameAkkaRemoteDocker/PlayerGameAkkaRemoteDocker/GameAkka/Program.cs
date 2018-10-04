using Akka.Actor;
using FluentColorConsole;
using Microsoft.Extensions.Configuration;
using PlayerGameAkkaRemote.Actors;
using System;
using System.IO;
using System.Threading;

namespace GameAkka
{
    class Program
    {
        private static readonly AutoResetEvent _endapp = new AutoResetEvent(false); //Console ReadKey doesn't work in Docker !!

        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnCtrlC);

            //Get configuration from json and environment
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();
            ColorConsole.WithYellowText.WriteLine($"name set to <{configuration["akka:name"]}>");
            ColorConsole.WithYellowText.WriteLine($"host set to <{configuration["akka:host"]}>");
            ColorConsole.WithYellowText.WriteLine($"port set to <{configuration["akka:port"]}>");

            string akkaConfig = @"akka {  
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
                            public-port = " + configuration["akka:publicport"] + @"
                            port = " + configuration["akka:port"] + @"
                            public-hostname = " + configuration["akka:publichost"] + @"
                            hostname = " + configuration["akka:host"] + @"
                        }
                    }
                }";

            ActorSystem actorSystem = ActorSystem.Create(configuration["akka:name"], akkaConfig);

            IActorRef game = actorSystem.ActorOf<GameActor>("game1");

            _endapp.WaitOne();

            actorSystem.Terminate().Wait();
        }

        private static void OnCtrlC(object sender, ConsoleCancelEventArgs e)
        {
            _endapp.Set();
        }
    }
}

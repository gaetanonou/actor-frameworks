using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerGameAkka.Messages
{
    internal class Command
    {
        public char Key { get; private set; }
        public IActorRef Game { get; private set; }

        public Command(char key, IActorRef game)
        {
            Key = key;
            Game = game;
        }
    }
}

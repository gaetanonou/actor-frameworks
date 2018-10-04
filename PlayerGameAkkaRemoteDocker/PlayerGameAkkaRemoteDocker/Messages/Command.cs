using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerGameAkkaRemote.Messages
{
    public class Command
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

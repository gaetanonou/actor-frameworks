using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace PlayerGameAkkaRemote.Messages
{
    public class PlayerJoined
    {
        public IActorRef Player { get; private set; }

        public PlayerJoined(IActorRef player)
        {
            Player = player;
        }
    }
}

﻿using Akka.Actor;
using System.Collections.Generic;

namespace PlayerGameAkka.Messages
{
    internal class PlayerList   
    {
        public List<IActorRef> Players { get; private set; } //Immutable ??????

        public PlayerList(List<IActorRef> players)
        {
            //Players = players
            Players = new List<IActorRef>(players);
        }
    }
}

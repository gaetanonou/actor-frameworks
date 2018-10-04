using FluentColorConsole;
using Orleans;
using SouthParkInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SouthParkSilo.Grains
{
    public class CharacterGrain : Grain, ICharacterGrain
    {
        private int _numberOfTimesAsked = 0;

        public Task<int> AreYouAlive()
        {
            _numberOfTimesAsked += 1;
            return Task.FromResult(_numberOfTimesAsked);
        }
    }
}

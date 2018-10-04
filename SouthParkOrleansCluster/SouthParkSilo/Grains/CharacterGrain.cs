using FluentColorConsole;
using Orleans;
using Orleans.Providers;
using SouthParkInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SouthParkSilo.Grains
{
    [StorageProvider(ProviderName = "LocalSql")]
    public class CharacterGrain : Grain<CharacterState>, ICharacterGrain
    {
        public Task<int> AreYouAlive()
        {
            State.NumberOfTimesAsked += 1;
            this.WriteStateAsync();
            return Task.FromResult(State.NumberOfTimesAsked);
        }
    }
}

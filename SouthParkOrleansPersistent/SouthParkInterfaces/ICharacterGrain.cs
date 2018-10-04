using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SouthParkInterfaces
{
    public interface ICharacterGrain : IGrainWithStringKey
    {
        Task<int> AreYouAlive();
    }
}

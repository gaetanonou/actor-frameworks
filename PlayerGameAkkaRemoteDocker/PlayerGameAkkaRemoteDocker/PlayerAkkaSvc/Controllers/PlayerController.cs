using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlayerAkkaSvc.Actors;

namespace PlayerAkkaSvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        AkkaSystemService _akkaSystem = null;

        public PlayerController(AkkaSystemService akkaSystem)
        {
            _akkaSystem = akkaSystem;
        }

        // POST api/player/{playerId}/join
        [HttpPost]
        [Route("{playerId}/join")]
        public void Join(int playerId)
        {
            _akkaSystem.Join(playerId);
        }

        // POST api/player/{playerId}/leave
        [HttpPost]
        [Route("{playerId}/leave")]
        public void Leave(int playerId)
        {
            _akkaSystem.Leave(playerId);
        }

        // GET api/player
        [HttpGet]
        [Route("{playerId}/list")]
        public ActionResult Get(int playerId)
        {
            _akkaSystem.ListPlayers(playerId);
            return new OkResult();
        }
    }
}

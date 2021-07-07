using Microsoft.AspNetCore.Mvc;
using PokerLibrary.TexasHoldEm.Models;

namespace PokerWithFriends.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            var deck = new Deck();
            return Ok(deck);
        }
    }
}

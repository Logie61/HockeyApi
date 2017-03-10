using HockeyApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Controllers
{
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        private HockeyApiContext _context;

        public PlayersController(HockeyApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Players()
        {
            var players = _context.Players;
            return Ok(players);
        }
    }
}

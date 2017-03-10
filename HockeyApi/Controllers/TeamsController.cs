using HockeyApi.Data;
using HockeyApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Controllers
{
    [Route("api/[controller]")]
    public class TeamsController : Controller
    {
        private HockeyApiContext _context;

        public TeamsController(HockeyApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Teams()
        {
            var teams = _context.Teams.Include(t => t.Players);
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public IActionResult Team(Guid id)
        {
            var team = _context.Teams.Include(t => t.Players).SingleOrDefault(t => t.Id == id);

            return Ok(team);
        }

        [HttpGet("byleague/{leagueId}")]
        public IActionResult ByLeague(Guid leagueId)
        {
            var teams = _context.Teams.Include(t => t.Players).Where(t => t.LeagueId == leagueId);

            return Ok(teams);
        }

        [HttpPost]
        public IActionResult Create([FromBody]Team model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Teams.Add(model);
            _context.SaveChanges();

            return CreatedAtAction("Team", new { id = model.Id }, model);
        }
    }
}

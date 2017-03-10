using HockeyApi.Data;
using HockeyApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HockeyApi.Controllers
{
    [Route("api/[controller]")]
    public class LeaguesController : Controller
    {
        private HockeyApiContext _context;

        public LeaguesController(HockeyApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Leagues()
        {
            var leagues = _context.Leagues.Include(l => l.Teams).ToList();
            return Ok(leagues);
        }

        [HttpPost]
        public IActionResult Create([FromBody]League model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Leagues.Add(model);
            _context.SaveChanges();
            return CreatedAtAction("League", new { id = model.Id }, model);
        }

        [HttpPut]
        public IActionResult Update([FromBody]League model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Leagues.Update(model);
            _context.SaveChanges();

            return RedirectToAction("League", "Leagues", new { id = model.Id });
        }

        [HttpGet("{id}")]
        public IActionResult League(Guid id)
        {
            var league = _context.Leagues.Include(l => l.Teams).SingleOrDefault(l => l.Id == id);

            return Ok(league);
        }

        [HttpGet("bycountry/{country}")]
        public IActionResult ByCountry(string country)
        {
            var leagues = _context.Leagues.Include(l => l.Teams).Where(l => l.Country == country);

            return Ok(leagues);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if(id == default(Guid))
            {
                return BadRequest("Invalid id provided");
            }

            var league = _context.Leagues.SingleOrDefault(l => l.Id == id);
            if (league == null)
            {
                return BadRequest($"No league with id {id}");
            }

            _context.Leagues.Remove(league);
            _context.SaveChanges();

            return Ok();
        }
    }
}

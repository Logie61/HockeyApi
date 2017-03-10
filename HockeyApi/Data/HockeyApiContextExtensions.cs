using Microsoft.EntityFrameworkCore;
using System.Linq;
using HockeyApi.Domain;
using Bogus;
using System;

namespace HockeyApi.Data
{
    public static class HockeyApiContextExtensions
    {
        public static void EnsureSeedData(this HockeyApiContext context)
        {
            var leagueFaker = new Faker<League>()
                .StrictMode(false)
                .RuleFor(l => l.Country, f => f.Address.Country())
                .RuleFor(l => l.Name, f => f.Company.CompanyName());

            var teamFaker = new Faker<Team>()
                .StrictMode(false)
                .RuleFor(t => t.Name, f => f.Company.CompanyName());

            var playerFaker = new Faker<Player>()
                .StrictMode(false)
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.Country, f => f.Address.Country());
                


            var pendingMigrations = context.Database.GetPendingMigrations();
            if (!pendingMigrations.Any())
            {
                if (!context.Leagues.Any())
                {
                    var leagueMax = 10;

                    for(var i = 0; i < leagueMax; i++)
                    {
                        context.Leagues.Add(leagueFaker.Generate());
                    }
  
                    context.SaveChanges();
                }

                if (!context.Teams.Any())
                {
                    var leagues = context.Leagues;
                    foreach(var league in leagues)
                    {
                        for(var i = 0; i < 10; i++)
                        {
                            var team = teamFaker.Generate();
                            team.League = league;
                            team.LeagueId = league.Id;
                            team.Country = league.Country;
                            context.Teams.Add(team);
                        }
                    }
                    context.SaveChanges();
                }

                if (!context.Players.Any())
                {
                    foreach(var team in context.Teams)
                    {
                        for(var i=0; i < 22; i++)
                        {
                            var player = playerFaker.Generate();
                            player.TeamId = team.Id;
                            player.Team = team;
                            context.Players.Add(player);
                        }
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}

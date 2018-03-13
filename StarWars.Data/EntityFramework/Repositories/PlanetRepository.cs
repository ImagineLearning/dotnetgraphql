using StarWars.Core.Models;
using Microsoft.Extensions.Logging;
using StarWars.Core.Data;
using Nest;

namespace StarWars.Data.EntityFramework.Repositories
{
    public class PlanetRepository : BaseRepository<Planet, int>, IPlanetRepository
    {
        public PlanetRepository() { }

        public PlanetRepository(StarWarsContext db, IElasticClient client, ILogger<PlanetRepository> logger)
            : base(db, client, logger)
        {
        }
    }
}

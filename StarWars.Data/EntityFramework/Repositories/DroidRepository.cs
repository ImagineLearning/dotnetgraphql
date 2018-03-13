using StarWars.Core.Models;
using Microsoft.Extensions.Logging;
using StarWars.Core.Data;
using Nest;

namespace StarWars.Data.EntityFramework.Repositories
{
    public class DroidRepository : BaseRepository<Droid, int>, IDroidRepository
    {
        public DroidRepository() { }

        public DroidRepository(StarWarsContext db, IElasticClient client, ILogger<DroidRepository> logger)
            : base(db, client, logger)
        {
        }
    }
}

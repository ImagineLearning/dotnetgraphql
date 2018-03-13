using StarWars.Core.Models;
using Microsoft.Extensions.Logging;
using StarWars.Core.Data;
using Nest;

namespace StarWars.Data.EntityFramework.Repositories
{
    public class EpisodeRepository : BaseRepository<Episode, int>, IEpisodeRepository
    {
        public EpisodeRepository() { }

        public EpisodeRepository(StarWarsContext db, IElasticClient client, ILogger<EpisodeRepository> logger)
            : base(db, client, logger)
        {
        }
    }
}

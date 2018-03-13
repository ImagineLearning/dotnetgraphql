using StarWars.Core.Models;
using Microsoft.Extensions.Logging;
using StarWars.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nest;

namespace StarWars.Data.EntityFramework.Repositories
{
    public class HumanRepository : BaseRepository<Human, int>, IHumanRepository
    {
        public HumanRepository() { }

        public HumanRepository(StarWarsContext db, IElasticClient client, ILogger<HumanRepository> logger)
            : base(db, client, logger)
        {
        }

		public override Task<Human> Get(int id)
        {
            _logger.LogInformation("Get {type} with id = {id}", typeof(Human).Name, id);
			return _client.SearchAsync<Human>(s => s.Query(q => q.Match(m => m.Field(f => f.Id).Query(id.ToString())))).ContinueWith(x => x.Result.Documents.ToList().FirstOrDefault());
            //return _db.Set<TEntity>().SingleOrDefaultAsync(c => c.Id.Equals(id));
        }
    }
}

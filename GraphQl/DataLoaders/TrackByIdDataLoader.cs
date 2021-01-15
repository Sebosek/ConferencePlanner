using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ConferencePlanner.GraphQl.Data;

using GreenDonut;

using HotChocolate.DataLoader;

using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQl.DataLoaders
{
    public class TrackByIdDataLoader : BatchDataLoader<int, Track>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public TrackByIdDataLoader(
            IBatchScheduler batchScheduler, 
            IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected override async Task<IReadOnlyDictionary<int, Track>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Tracks
                .Where(w => keys.Contains(w.Id))
                .ToDictionaryAsync(k => k.Id, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
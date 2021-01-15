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
    public class SpeakerByIdDataLoader : BatchDataLoader<int, Speaker>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        
        public SpeakerByIdDataLoader(
            IBatchScheduler batchScheduler, 
            IDbContextFactory<ApplicationDbContext> dbContextFactory) 
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory;
        }
        
        protected override async Task<IReadOnlyDictionary<int, Speaker>> LoadBatchAsync(
            IReadOnlyList<int> keys, 
            CancellationToken cancellationToken)
        {
            await using var context = _dbContextFactory.CreateDbContext();
        
            return await context.Speakers
                .Where(w => keys.Contains(w.Id))
                .ToDictionaryAsync(k => k.Id, cancellationToken).ConfigureAwait(false);
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ConferencePlanner.GraphQl.Attributes;
using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.DataLoaders;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQl.Speakers
{
    [ExtendObjectType(Name = Consts.QUERY)]
    public class SpeakerQueries
    {
        [UseApplicationDbContext]
        public Task<List<Speaker>> GetSpeakersAsync([ScopedService] ApplicationDbContext context) =>
            context.Speakers.ToListAsync();

        public Task<Speaker> GetSpeakerByIdAsync(
            [ID(nameof(Speaker))] int id, 
            SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) => dataLoader.LoadAsync(id, cancellationToken);
        
        public async Task<IReadOnlyCollection<Speaker>> GetSpeakersByIdAsync(
            [ID(nameof(Speaker))] int[] ids, 
            SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) => 
            await dataLoader.LoadAsync(ids, cancellationToken).ConfigureAwait(false);
    }
}
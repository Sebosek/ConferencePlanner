using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ConferencePlanner.GraphQl.Attributes;
using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.DataLoaders;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQl.Tracks
{
    [ExtendObjectType(Name = Consts.QUERY)]
    public class TrackQueries
    {
        [UseApplicationDbContext]
        [UsePaging]
        public IQueryable<Track> GetTracks(
            [ScopedService] ApplicationDbContext context) =>
            context.Tracks.OrderBy(o => o.Name);

        [UseApplicationDbContext]
        public Task<Track> GetTrackByNameAsync(
            string name,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken) => 
            context.Tracks.FirstAsync(t => t.Name == name, cancellationToken);

        [UseApplicationDbContext]
        public async Task<IReadOnlyCollection<Track>> GetTrackByNamesAsync(
            IEnumerable<string> names,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken) =>
            await context.Tracks
                .Where(t => names.Contains(t.Name))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

        public Task<Track> GetTrackByIdAsync(
            [ID(nameof(Track))] int id,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken) => trackById.LoadAsync(id, cancellationToken);

        public async Task<IReadOnlyCollection<Track>> GetTrackByIdsAsync(
            [ID(nameof(Track))] int[] ids,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken) =>
            await trackById.LoadAsync(ids, cancellationToken).ConfigureAwait(false);
    }
}
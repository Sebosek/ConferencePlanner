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
    [ExtendObjectType(Name = "Query")]
    public class TrackQueries
    {
        [UseApplicationDbContext]
        public async Task<IEnumerable<Track>> GetTracksAsync(
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken) =>
            await context.Tracks.ToListAsync(cancellationToken).ConfigureAwait(false);

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

        public async Task<IReadOnlyCollection<Track>> GetTracksByIdsAsync(
            [ID(nameof(Track))] int[] ids,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken) =>
            await trackById.LoadAsync(ids, cancellationToken).ConfigureAwait(false);
    }
}
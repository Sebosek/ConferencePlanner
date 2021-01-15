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

namespace ConferencePlanner.GraphQl.Sessions
{
    [ExtendObjectType(Name = Consts.QUERY)]
    public class SessionQueries
    {
        [UseApplicationDbContext]
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken) => 
            await context.Sessions.ToListAsync(cancellationToken).ConfigureAwait(false);

        public Task<Session> GetSessionByIdAsync(
            [ID(nameof(Session))] int id,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken) => sessionById.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Session>> GetSessionsByIdAsync(
            [ID(nameof(Session))] int[] ids,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken) =>
            await sessionById.LoadAsync(ids, cancellationToken).ConfigureAwait(false);
    }
}
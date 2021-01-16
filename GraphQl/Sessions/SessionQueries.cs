using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ConferencePlanner.GraphQl.Attributes;
using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.DataLoaders;
using ConferencePlanner.GraphQl.Types;

using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQl.Sessions
{
    [ExtendObjectType(Name = Consts.QUERY)]
    public class SessionQueries
    {
        [UseApplicationDbContext]
        [UsePaging(typeof(NonNullType<SessionType>))]
        [UseFiltering(typeof(SessionFilterInputType))]
        [UseSorting]
        public IQueryable<Session> GetSessions([ScopedService] ApplicationDbContext context) => 
            context.Sessions;

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
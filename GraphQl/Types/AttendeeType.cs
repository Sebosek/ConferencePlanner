using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.DataLoaders;

using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQl.Types
{
    public class AttendeeType : ObjectType<Attendee>
    {
        protected override void Configure(IObjectTypeDescriptor<Attendee> descriptor)
        {
            descriptor.ImplementsNode()
                .IdField(p => p.Id)
                .ResolveNode(WithDataLoader);

            descriptor.Field(f => f.SessionAttendees)
                .ResolveWith<AttendeeResolvers>(r => r.GetSessionsAsync(default!, default!, default!, default))
                .Name("session");
        }
        
        private static Task<Attendee> WithDataLoader(IResolverContext context, int id) =>
            context.DataLoader<AttendeeByIdDataLoader>().LoadAsync(id, context.RequestAborted);
        
        private class AttendeeResolvers
        {
            public async Task<IReadOnlyCollection<Session>> GetSessionsAsync(
                Attendee attendee,
                [ScopedService] ApplicationDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken)
            {
                var ids = await dbContext.Attendees
                    .Where(w => w.Id == attendee.Id)
                    .Include(i => i.SessionAttendees)
                    .SelectMany(s => s.SessionAttendees.Select(e => e.SessionId))
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                return await sessionById.LoadAsync(ids, cancellationToken);
            }
        }
    }
}
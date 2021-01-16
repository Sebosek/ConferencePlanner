using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.DataLoaders;
using ConferencePlanner.GraphQl.Extensions;

using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQl.Types
{
    public class SpeakerType : ObjectType<Speaker>
    {
        protected override void Configure(IObjectTypeDescriptor<Speaker> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(p => p.Id)
                .ResolveNode(WithDataLoader);
            
            descriptor
                .Field(f => f.SessionSpeakers)
                .ResolveWith<SpeakerResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .UseDbContext<ApplicationDbContext>()
                .Name("sessions");
        }

        private static Task<Speaker> WithDataLoader(IResolverContext context, int id) =>
            context.DataLoader<SpeakerByIdDataLoader>().LoadAsync(id, context.RequestAborted);
        
        private class SpeakerResolvers
        {
            public async Task<IReadOnlyCollection<Session>> GetSessionsAsync(
                Speaker speaker,
                [ScopedService] ApplicationDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken)
            {
                var ids = await dbContext.Speakers
                    .Where(w => w.Id == speaker.Id)
                    .Include(i => i.SessionSpeakers)
                    .SelectMany(s => s.SessionSpeakers.Select(e => e.SessionId))
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                return await sessionById.LoadAsync(ids, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
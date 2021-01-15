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
    public class SessionType : ObjectType<Session>
    {
        protected override void Configure(IObjectTypeDescriptor<Session> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(p => p.Id)
                .ResolveNode(WithDataLoader);

            descriptor
                .Field(f => f.SessionSpeakers)
                .ResolveWith<SessionResolvers>(r => r.GetSpeakersAsync(default!, default!, default!, default))
                .Name("speakers");
            
            descriptor
                .Field(f => f.SessionAttendees)
                .ResolveWith<SessionResolvers>(r => r.GetAttendeeAsync(default!, default!, default!, default))
                .Name("attendees");

            descriptor
                .Field(p => p.Track)
                .ResolveWith<SessionResolvers>(r => r.GetTrackAsync(default!, default!, default));

            descriptor
                .Field(p => p.TrackId)
                .ID(nameof(Track));
        }
        
        private static Task<Session> WithDataLoader(IResolverContext context, int id) =>
            context.DataLoader<SessionByIdDataLoader>().LoadAsync(id, context.RequestAborted);
        
        private class SessionResolvers
        {
            public async Task<IReadOnlyCollection<Speaker>> GetSpeakersAsync(
                Session session,
                [ScopedService] ApplicationDbContext dbContext,
                SpeakerByIdDataLoader speakerById,
                CancellationToken cancellationToken)
            {
                var ids = await dbContext.Sessions
                    .Where(w => w.Id == session.Id)
                    .Include(i => i.SessionSpeakers)
                    .SelectMany(s => s.SessionSpeakers.Select(e => e.SpeakerId))
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                return await speakerById.LoadAsync(ids, cancellationToken);
            }
            
            public async Task<IReadOnlyCollection<Attendee>> GetAttendeeAsync(
                Session session,
                [ScopedService] ApplicationDbContext dbContext,
                AttendeeByIdDataLoader attendeeById,
                CancellationToken cancellationToken)
            {
                var ids = await dbContext.Sessions
                    .Where(w => w.Id == session.Id)
                    .Include(i => i.SessionAttendees)
                    .SelectMany(s => s.SessionAttendees.Select(e => e.AttendeeId))
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                return await attendeeById.LoadAsync(ids, cancellationToken);
            }

            public Task<Track> GetTrackAsync(
                Session session,
                TrackByIdDataLoader trackById,
                CancellationToken cancellationToken)
            {
                if (session.TrackId is null)
                {
                    return Task.FromResult<Track>(null!);
                }

                return trackById.LoadAsync(session.TrackId.Value, cancellationToken);
            }
        }
    }
}
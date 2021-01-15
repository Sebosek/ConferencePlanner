using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.DataLoaders;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQl.Sessions
{
    public class ScheduleSessionPayload : SessionPayloadBase
    {
        public ScheduleSessionPayload(Session session) : base(session)
        {
        }

        public ScheduleSessionPayload(UserError error) : base(new[] {error})
        {
        }

        public Task<Track> GetTrackAsync(
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken)
        {
            return trackById.LoadAsync(Session.Id, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Speaker>> GetSpeakersAsync(
            [ScopedService] ApplicationDbContext dbContext,
            SpeakerByIdDataLoader speakerById,
            CancellationToken cancellationToken)
        {
            var ids = await dbContext.Sessions
                .Where(w => w.Id == Session.Id)
                .Include(i => i.SessionSpeakers)
                .SelectMany(s => s.SessionSpeakers.Select(e => e.SpeakerId))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return await speakerById.LoadAsync(ids, cancellationToken);
        }
    }
}
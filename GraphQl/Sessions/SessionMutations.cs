using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using ConferencePlanner.GraphQl.Attributes;
using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;

using HotChocolate;
using HotChocolate.Types;

namespace ConferencePlanner.GraphQl.Sessions
{
    [ExtendObjectType(Name = Consts.MUTATION)]
    public class SessionMutations
    {
        [UseApplicationDbContext]
        public async Task<AddSessionPayload> AddSessionAsync(
            AddSessionInput input,
            [ScopedService] ApplicationDbContext context,
            [Service] IMapper mapper,
            CancellationToken cancellationToken)
        {
            if (input.Title is not {Length: >0})
            {
                return new AddSessionPayload(
                    new UserError($"{nameof(AddSessionInput.Title)} cannot be empty", "EMPTY_TITLE"));
            }

            if (!input.SpeakerIds.Any())
            {
                return new AddSessionPayload(
                    new UserError("No speaker assigned", "NO_SPEAKER"));
            }

            var session = mapper.Map<Session>(input);

            await context.Sessions.AddAsync(session, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new AddSessionPayload(session);
        }

        [UseApplicationDbContext]
        public async Task<ScheduleSessionPayload> ScheduleSessionPayload(
            ScheduleSessionInput input,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            if (input.EndTime < input.StartTime)
            {
                return new ScheduleSessionPayload(
                    new UserError($"Session have negative lenght. Don't you swap times?", "SWAP_TIME"));
            }

            var session = await context.Sessions.FindAsync(new object[] {input.SessionId}, cancellationToken)
                .ConfigureAwait(false);
            
            if (session is null)
            {
                return new ScheduleSessionPayload(
                    new UserError("Session not found", "SESSION_NOT_FOUND"));
            }
            
            session.TrackId = input.TrackId;
            session.StartTime = input.StartTime;
            session.EndTime = input.EndTime;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new ScheduleSessionPayload(session);
        }
    }
}
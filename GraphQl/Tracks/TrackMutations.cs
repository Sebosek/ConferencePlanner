using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using ConferencePlanner.GraphQl.Attributes;
using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQL.Tracks;
using HotChocolate;
using HotChocolate.Types;

namespace ConferencePlanner.GraphQl.Tracks
{
    [ExtendObjectType(Name = Consts.MUTATION)]
    public class TrackMutations
    {
        [UseApplicationDbContext]
        public async Task<AddTrackPayload> AddTrackAsync(
            AddTrackInput input,
            [ScopedService] ApplicationDbContext context,
            [Service] IMapper mapper,
            CancellationToken cancellationToken)
        {
            if (input.Name is not {Length: >0})
            {
                return new AddTrackPayload(
                    new UserError($"{nameof(AddTrackInput.Name)} cannot be empty", "EMPTY_NAME"));
            }

            var entity = mapper.Map<Track>(input);

            await context.Tracks.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new AddTrackPayload(entity);
        }

        public async Task<RenameTrackPayload> RenameTrackAsync(
            RenameTrackInput input,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            if (input.Name is not {Length: >0})
            {
                return new RenameTrackPayload(
                    new UserError($"{nameof(AddTrackInput.Name)} cannot be empty", "EMPTY_NAME"));
            }

            var track = await context.Tracks.FindAsync(new object[] {input.Id}, cancellationToken)
                .ConfigureAwait(false);

            track.Name = input.Name;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new RenameTrackPayload(track);
        }
    }
}
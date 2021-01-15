using System.Threading.Tasks;

using AutoMapper;

using ConferencePlanner.GraphQl.Attributes;
using ConferencePlanner.GraphQl.Data;

using HotChocolate;
using HotChocolate.Types;

namespace ConferencePlanner.GraphQl.Speakers
{
    [ExtendObjectType(Name = Consts.MUTATION)]
    public class SpeakerMutations
    {
        [UseApplicationDbContext]
        public async Task<AddSpeakerPayload> AddSpeakerAsync(
            AddSpeakerInput input,
            [ScopedService] ApplicationDbContext context,
            [Service] IMapper mapper)
        {
            var entity = mapper.Map<Speaker>(input);

            await context.Speakers.AddAsync(entity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return new AddSpeakerPayload(entity);
        }
    }
}
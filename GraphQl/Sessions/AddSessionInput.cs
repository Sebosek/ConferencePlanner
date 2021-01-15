using System.Collections.Generic;

using ConferencePlanner.GraphQl.Data;

using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQl.Sessions
{
    public record AddSessionInput(
        string Title,
        string? Abstract,
        [ID(nameof(Speaker))] IEnumerable<int> SpeakerIds);
}
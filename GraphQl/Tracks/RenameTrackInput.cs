using ConferencePlanner.GraphQl.Data;

using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQl.Tracks
{
    public record RenameTrackInput(
        [ID(nameof(Track))] int Id,
        string Name);
}
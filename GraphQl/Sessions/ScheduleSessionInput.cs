using System;

using ConferencePlanner.GraphQl.Data;

using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQl.Sessions
{
    public record ScheduleSessionInput(
        [ID(nameof(Session))] int SessionId,
        [ID(nameof(Track))] int TrackId,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);
}
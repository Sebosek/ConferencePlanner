using System.Collections.Generic;
using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;

namespace ConferencePlanner.GraphQl.Tracks
{
    public abstract class TrackPayloadBase : Payload
    {
        public Track Track { get; }

        protected TrackPayloadBase(Track track)
        {
            Track = track;
        }

        protected TrackPayloadBase(IEnumerable<UserError> errors) :
            base(errors)
        {
            Track = default!;
        }
    }
}
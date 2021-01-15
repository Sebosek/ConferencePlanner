using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.Tracks;

namespace ConferencePlanner.GraphQL.Tracks
{
    public class RenameTrackPayload : TrackPayloadBase
    {
        public RenameTrackPayload(Track track) : 
            base(track)
        {
        }

        public RenameTrackPayload(UserError error) : 
            base(new []{error})
        {
        }
    }
}
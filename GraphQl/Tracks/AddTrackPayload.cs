using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;

namespace ConferencePlanner.GraphQl.Tracks
{
    public class AddTrackPayload : TrackPayloadBase
    {
        public AddTrackPayload(Track track) : 
            base(track)
        {
        }

        public AddTrackPayload(UserError error) : 
            base(new []{error})
        {
        }
    }
}
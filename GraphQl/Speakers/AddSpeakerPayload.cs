using System.Collections.Generic;

using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;

namespace ConferencePlanner.GraphQl.Speakers
{
    public class AddSpeakerPayload : SpeakerPayloadBase
    {
        public AddSpeakerPayload(Speaker speaker) : 
            base(speaker)
        {
        }

        public AddSpeakerPayload(IEnumerable<UserError> errors) :
            base(errors)
        {
        }
    }
}
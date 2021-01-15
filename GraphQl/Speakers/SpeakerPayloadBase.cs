using System.Collections.Generic;

using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;

namespace ConferencePlanner.GraphQl.Speakers
{
    public abstract class SpeakerPayloadBase : Payload
    {
        public Speaker? Speaker { get; }

        protected SpeakerPayloadBase(Speaker speaker)
        {
            Speaker = speaker;
        }

        protected SpeakerPayloadBase(IEnumerable<UserError> errors) :
            base(errors)
        {
        }
    }
}
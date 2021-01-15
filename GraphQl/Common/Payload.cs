using System.Collections.Generic;
using System.Linq;

namespace ConferencePlanner.GraphQl.Common
{
    public abstract class Payload
    {
        public IEnumerable<UserError> Errors { get; }

        protected Payload() : this(Enumerable.Empty<UserError>())
        {
        }
        
        protected Payload(IEnumerable<UserError> errors)
        {
            Errors = errors;
        }
    }
}
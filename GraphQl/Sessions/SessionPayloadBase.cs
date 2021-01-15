using System.Collections.Generic;
using System.Linq;

using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;

namespace ConferencePlanner.GraphQl.Sessions
{
    public abstract class SessionPayloadBase : Payload
    {
        public Session Session { get; }

        protected SessionPayloadBase(Session session) : this(Enumerable.Empty<UserError>())
        {
            Session = session;
        }

        protected SessionPayloadBase(IEnumerable<UserError> errors) : 
            base(errors)
        {
            Session = default!;
        }
    }
}
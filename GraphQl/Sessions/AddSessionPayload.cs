using ConferencePlanner.GraphQl.Common;
using ConferencePlanner.GraphQl.Data;

namespace ConferencePlanner.GraphQl.Sessions
{
    public class AddSessionPayload : SessionPayloadBase
    {
        public AddSessionPayload(Session session) : base(session)
        {
        }

        public AddSessionPayload(UserError error) : base(new[] { error })
        {
        }
    }
}
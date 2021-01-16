using ConferencePlanner.GraphQl.Data;

using HotChocolate.Data.Filters;

namespace ConferencePlanner.GraphQl.Sessions
{
    public class SessionFilterInputType : FilterInputType<Session>
    {
        protected override void Configure(IFilterInputTypeDescriptor<Session> descriptor)
        {
            descriptor.Ignore(p => p.Id);
            descriptor.Ignore(p => p.TrackId);
        }
    }
}
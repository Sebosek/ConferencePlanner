using System.Reflection;

using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.Extensions;

using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace ConferencePlanner.GraphQl.Attributes
{
    public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context, 
            IObjectFieldDescriptor descriptor, 
            MemberInfo member)
        {
            descriptor.UseDbContext<ApplicationDbContext>();
        }
    }
}
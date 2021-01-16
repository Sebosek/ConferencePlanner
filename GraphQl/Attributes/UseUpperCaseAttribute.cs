using System.Reflection;

using ConferencePlanner.GraphQl.Extensions;

using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace ConferencePlanner.GraphQl.Attributes
{
    public class UseUpperCaseAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.UseUpperCase();
        }
    }
}
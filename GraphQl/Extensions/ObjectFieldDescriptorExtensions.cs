using HotChocolate.Types;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConferencePlanner.GraphQl.Extensions
{
    public static class ObjectFieldDescriptorExtensions
    {
        public static IObjectFieldDescriptor UseDbContext<T>(this IObjectFieldDescriptor descriptor)
            where T : DbContext
        {
            return descriptor.UseScopedService<T>(
                s => s.GetRequiredService<IDbContextFactory<T>>().CreateDbContext(),
                disposeAsync: (s, c) => c.DisposeAsync());
        }
    }
}
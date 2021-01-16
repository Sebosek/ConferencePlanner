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

        public static IObjectFieldDescriptor UseUpperCase(this IObjectFieldDescriptor descriptor)
        {
            return descriptor.Use(next => async context =>
            {
                await next(context);

                if (context.Result is string s)
                {
                    context.Result = s.ToUpperInvariant();
                }
            });
        }
    }
}
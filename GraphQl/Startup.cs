using AutoMapper;

using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.DataLoaders;
using ConferencePlanner.GraphQl.Sessions;
using ConferencePlanner.GraphQl.Speakers;
using ConferencePlanner.GraphQl.Tracks;
using ConferencePlanner.GraphQl.Types;

using HotChocolate.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConferencePlanner.GraphQl
{
    public class Startup
    {
        private const string CONNECTION_STRING = "Data source=./conferences.db";
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(CreateAutomapper());
            services.AddPooledDbContextFactory<ApplicationDbContext>(options => 
                options.UseSqlite(CONNECTION_STRING).UseLoggerFactory(ApplicationDbContext.DbContextLoggerFactory));
            services
                .AddGraphQLServer()
                .AddQueryType(d => d.Name(Consts.QUERY))
                    .AddTypeExtension<SpeakerQueries>()
                    .AddTypeExtension<SessionQueries>()
                    .AddTypeExtension<TrackQueries>()
                .AddMutationType(d => d.Name(Consts.MUTATION))
                    .AddTypeExtension<SpeakerMutations>()
                    .AddTypeExtension<SessionMutations>()
                    .AddTypeExtension<TrackMutations>()
                .AddType<AttendeeType>()
                .AddType<SessionType>()
                .AddType<SpeakerType>()
                .AddType<TrackType>()
                .EnableRelaySupport()
                .AddFiltering()
                .AddSorting()
                .AddDataLoader<SpeakerByIdDataLoader>()
                .AddDataLoader<SessionByIdDataLoader>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL().WithOptions(new GraphQLServerOptions
                {
                    Tool = {Enable = true}
                });
            });
        }

        private static IMapper CreateAutomapper() =>
            new MapperConfiguration(config =>
            {
                config.AddProfile<MapperProfile>();
            }).CreateMapper();
    }
}

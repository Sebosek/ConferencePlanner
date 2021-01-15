using AutoMapper;

using ConferencePlanner.GraphQl.Data;
using ConferencePlanner.GraphQl.Sessions;
using ConferencePlanner.GraphQl.Tracks;

namespace ConferencePlanner.GraphQl
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AddSpeakerInput, Speaker>();
            CreateMap<AddSessionInput, Session>();
            CreateMap<AddTrackInput, Track>();
        }
    }
}
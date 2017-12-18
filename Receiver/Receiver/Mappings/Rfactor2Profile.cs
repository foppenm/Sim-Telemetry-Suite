using AutoMapper;
using Newtonsoft.Json.Linq;
using Json = Receiver.Json;
using Model = Receiver.Models;

namespace Receiver.Mappings
{
    public class Rfactor2Profile : Profile
    {
        public Rfactor2Profile()
        {
            CreateMap<Json.Track, Model.Session>()
                .ForMember(session => session.Type, o => o.MapFrom(m => m.session))
                .ForMember(session => session.Duration, o => o.MapFrom(m => m.endET))
                .ForMember(session => session.CurrentTime, o => o.MapFrom(m => m.currentET));

            CreateMap<Json.Track, Model.Track>()
                .ForMember(track => track.Name, o => o.MapFrom(m => m.trackName))
                .ForMember(track => track.Distance, o => o.MapFrom(m => m.lapDist))
                .ForMember(track => track.Phase, o => o.MapFrom(m => m.gamePhase))
                .ForMember(track => track.SectorFlags, o => o.MapFrom(m => m.sectorFlags.ToString()))
                .ForMember(track => track.Session, o => o.MapFrom(m => m))
                .ForMember(track => track.Vehicles, o => o.Ignore());
        }
    }
}

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

            CreateMap<Json.Vehicle, Model.Vehicle>()
                .ForMember(vehicle => vehicle.Id, o => o.MapFrom(m => m.id))
                .ForMember(vehicle => vehicle.Name, o => o.MapFrom(m => m.vehicleName))
                .ForMember(vehicle => vehicle.DriverName, o => o.MapFrom(m => m.driverName))
                .ForMember(vehicle => vehicle.Position, o => o.MapFrom(m => m.Pos))
                .ForMember(vehicle => vehicle.CurrentSector1, o => o.MapFrom(m => m.currentSector1))
                .ForMember(vehicle => vehicle.CurrentSector2, o => o.MapFrom(m => m.currentSector2))
                .ForMember(vehicle => vehicle.Pit, o => o.MapFrom(m => m.inPits))
                .ForMember(vehicle => vehicle.Place, o => o.MapFrom(m => m.place))
                .ForMember(vehicle => vehicle.BestLapTime, o => o.MapFrom(m => m.best))
                .ForMember(vehicle => vehicle.Best, o => o.MapFrom(m => m.best));

            CreateMap<Json.Track, Model.Track>()
                .ForMember(track => track.Name, o => o.MapFrom(m => m.trackName))
                .ForMember(track => track.Distance, o => o.MapFrom(m => m.lapDist))
                .ForMember(track => track.Phase, o => o.MapFrom(m => m.gamePhase))
                .ForMember(track => track.SectorFlags, o => o.MapFrom(m => m.sectorFlags))
                .ForMember(track => track.Session, o => o.MapFrom(m => m))
                .ForMember(track => track.Vehicles, o => o.MapFrom(m => m.vehicles));
        }
    }
}

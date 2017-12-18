using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Receiver.Mappings;
using Model = Receiver.Models;
using Json = Receiver.Json;
using Xunit;
using System;

namespace Receiver.Tests
{
    public class MapTests
    {
        public MapTests()
        {
            // Initialize maps
            Mapper.Initialize(cfg =>
            {
                // Add profiles for simulations
                cfg.AddProfile<Rfactor2Profile>();
            });
        }

        private Json.Track GetNewTrackInstance()
        {
            return new Json.Track
            {
                trackName = "track",
                lapDist = 5000,
                gamePhase = 0,
                sectorFlags = new[] { 11, 11, 11 },
                session = 0,
                endET = 108000,
                currentET = 0,
                vehicles = new[]
                {
                    new Json.Vehicle {
                        id = 0,
                        driverName = "test",
                        vehicleName = "auto"
                    },
                    new Json.Vehicle
                    {
                        id = 100,
                        driverName = "test2",
                        vehicleName = "auto2"
                    }
                }
            };
        }

        [Fact]
        public void MapTrack_ShouldReturnFullyMappedResult()
        {
            var track = GetNewTrackInstance();
            var mappedTrack = Mapper.Map<Models.Track>(track);

            Assert.NotNull(mappedTrack);
            Assert.NotNull(mappedTrack.Vehicles);
            Assert.NotEmpty(mappedTrack.Vehicles);
            Assert.Equal(track.trackName, mappedTrack.Name);
            Assert.Equal(track.vehicles[0].driverName, mappedTrack.Vehicles[0].DriverName);
            Assert.Equal(track.vehicles[0].vehicleName, mappedTrack.Vehicles[0].Name);
        }

        [Fact]
        public void ValidateFastestLap_ShouldReturnTrue()
        {
            var generator = new TrackMapGenerator();
            var vehicle = new Model.Vehicle
            {
                DriverName = "Test",
                Id = 1,
                Laps =
                {
                    new Model.Lap {
                        Number = 1,
                        Time = new float[] { 13f, 13f, 13f },
                        Path = {  }
                    }
                },

            };

            Assert.True(ValidateFastestLap(vehicle));
        }

        private bool ValidateFastestLap(Model.Vehicle vehicle)
        {
            var track = GetNewTrackInstance();
            var mappedTrack = Mapper.Map<Models.Track>(track);

            var fastestLap = vehicle.BestLap;

            if (mappedTrack.Path.Count == 0 && fastestLap.Path.Count > 0 &&
                // do vette shit hier
                false
                )
            {

            }

            return false;


        }
    }
}

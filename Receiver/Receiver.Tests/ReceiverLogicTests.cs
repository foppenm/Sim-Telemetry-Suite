using AutoMapper;
using Receiver.Data;
using Receiver.Mappings;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Receiver.Tests
{
    public class ReceiverLogicTests
    {
        [Fact]
        public void HandleNext_StoresNewDriver()
        {
            // Arrange
            var trackMapper = new TrackMapper();
            var driverRepository = Arrange.GetGenericRepository<Driver>();
            var sut = new ReceiverLogic(trackMapper, driverRepository);

            var driverName = "Unit Test Driver 1";
            Json.Track track = new Json.Track
            {
                vehicles = new Json.Vehicle[]
                {
                    new Json.Vehicle
                    {
                        driverName = driverName
                    }
                }
            };

            // Act
            sut.HandleNext(track);
            var vehicle = driverRepository.GetAll().FirstOrDefault();

            // Assert
            Assert.NotNull(vehicle);
            Assert.Equal(1, driverRepository.GetAll().Count());
            Assert.Equal(driverName, vehicle.Name);

        }
    }
}

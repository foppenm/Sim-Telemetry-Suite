using AutoMapper;
using Newtonsoft.Json;
using Receiver.Data;
using Receiver.Mappings;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    public class ReceiverLogic
    {
        private TrackMapper _trackMapper;
        private IGenericRepository<Driver> _driverRepository;

        public ReceiverLogic(TrackMapper trackMapper, IGenericRepository<Driver> driverRepository)
        {
            _trackMapper = trackMapper;
            _driverRepository = driverRepository;
        }

        public void Start()
        {
            // Set up the udp endpoint
            var endpoint = new IPEndPoint(IPAddress.Any, 666);

            // create the Observer
            var observer = Observer.Create<Json.Track>(HandleNext, HandleException, HandleCompleted);

            Observer.Create<Json.Track>(
                (track) => HandleNext(track),
                (exception) => HandleException(exception),
                () => HandleCompleted());

            // Set up the udp stream
            var observable = UdpStream<Json.Track>(endpoint).Subscribe(observer);
        }

        private void HandleException(Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
        }

        private void HandleCompleted()
        {
            Console.WriteLine("Completed.");
        }

        public async void HandleNext(Json.Track track)
        {
            foreach (var vehicle in track.vehicles)
            {
                var foundDriver = await _driverRepository.GetByName(vehicle.driverName);
                if (foundDriver != null)
                {
                    // Do not add the driver, it already exists
                    continue;
                }

                await _driverRepository.Create(new Driver
                {
                    Name = vehicle.driverName
                });
            }
        }

        /// <summary>
        /// Set up an observable udp stream
        /// </summary>
        /// <typeparam name="T">Type of message</typeparam>
        /// <param name="endpoint">The endpoint to listen to</param>
        /// <param name="processor">Function that handles the resulting buffer</param>
        /// <returns></returns>
        private IObservable<T> UdpStream<T>(IPEndPoint endpoint)
        {
            return Observable.Using(
                () => new UdpClient(endpoint),
                (udpClient) =>
                    Observable.Defer(() => udpClient.ReceiveAsync().ToObservable())
                    .Repeat()
                    .Select((result) =>
                    {
                        var json = Encoding.UTF8.GetString(result.Buffer).Trim('\0');
                        return JsonConvert.DeserializeObject<T>(json);
                    })
            );
        }
    }
}

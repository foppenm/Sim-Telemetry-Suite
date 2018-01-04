using AutoMapper;
using System;
using System.Linq;

namespace Receiver.Mappings
{
    public class TrackMapper
    {
        private static Models.Track _track;

        public void ProcessTrackMessage(HubSender hub, Json.Track rawMessage)
        {
            //if (_track == null)
            //{
            //    // Do basic mapping
            //    _track = Mapper.Map<Models.Track>(rawMessage);
            //}

            // Update vehicles
            foreach (var jsonVehicle in rawMessage.vehicles)
            {
                var vehicle = _track.Vehicles.FirstOrDefault(v => v.Id == jsonVehicle.id);

                if (vehicle == null)
                {
                    Console.WriteLine($"{jsonVehicle.driverName} joined the server");
                    vehicle = new Models.Vehicle();
                    _track.Vehicles.Add(vehicle);
                }

                // Update vehicle
                MapVehicle(jsonVehicle, vehicle);

                // Get the current lap
                var lapNumber = jsonVehicle.totalLaps + 1;
                var currentLap =
                    vehicle.Laps.FirstOrDefault(l => l.Number == lapNumber) ??
                    new Models.Lap { Number = lapNumber };

                // Update current lap time and path
                currentLap.Time = jsonVehicle.last;
                if (!currentLap.Path.ContainsKey((int)jsonVehicle.lapDist))
                {
                    // Add the vehicle position to it's path
                    currentLap.Path.Add((int)jsonVehicle.lapDist, jsonVehicle.Pos);
                }

                // Calculate the top speed
                vehicle.TopVelocity = vehicle.Velocity > vehicle.TopVelocity ? vehicle.Velocity : vehicle.TopVelocity;

                // Watch the sectors and determine where the vehicle is
                if (vehicle.PreviousSector == Models.Sector.Sector3 &&
                    vehicle.Sector == Models.Sector.Sector1 &&
                    vehicle.NewLap == false)
                {
                    if (jsonVehicle.inPits == 1 && vehicle.Status != Models.Status.Pit)
                    {
                        Console.WriteLine($"{vehicle.DriverName}: entered the pits");
                        vehicle.Status = Models.Status.Pit;
                    }
                    else
                    {
                        vehicle.NewLap = true;
                        Console.WriteLine($"{vehicle.DriverName}: crossed start/finish");

                        // Finish the current lap
                        Console.WriteLine($"{vehicle.DriverName}: Lap {currentLap.Number}: {currentLap.TimeString}");

                        // Create a new lap
                        vehicle.Laps.Add(new Models.Lap { Number = lapNumber });
                    }
                }
                else if (vehicle.PreviousSector == Models.Sector.Sector1 &&
                    vehicle.Sector == Models.Sector.Sector2 &&
                    vehicle.NewLap == true)
                {
                    vehicle.NewLap = false;
                    Console.WriteLine($"{vehicle.DriverName}: sector 1: {currentLap.Sector1}");
                }
                else if (vehicle.PreviousSector == Models.Sector.Sector2 &&
                    vehicle.Sector == Models.Sector.Sector3 &&
                    vehicle.NewLap == false)
                {
                    Console.WriteLine($"{vehicle.DriverName}: sector 2: {currentLap.Sector2}");
                }
            }

            // Update track path with fastest vehicle path
            //var fastestVehicle = _track.Vehicles.Aggregate((left, right) => (left.BestLapTime < right.BestLapTime ? left : right));
            //if (fastestVehicle != null && fastestVehicle.BestLap != null)
            //{
            //    if (ValidateFastestLap(fastestVehicle))
            //    {
            //        _track.Path = fastestVehicle.BestLap.Path;
            //        _track.PathTime = fastestVehicle.BestLap.Sector3;
            //        Console.WriteLine($"Refreshed fastest lap with path from {fastestVehicle.DriverName}");

            //        if (hub != null)
            //        {
            //            hub.SendTrackPath(fastestVehicle.BestLap);
            //        }
            //    }
            //}

            // TODO: Remove disconnected vehicles
            // ...

            // Finally send the complete message
            //if (hub != null)
            //{
            //    hub.SendStatus(_track);
            //}
        }

        private void MapVehicle(Json.Vehicle source, Models.Vehicle destination)
        {
            if (destination == null)
            {
                destination = new Models.Vehicle();
            }

            destination.Id = source.id;
            destination.Name = source.vehicleName;
            destination.DriverName = source.driverName;
            destination.Place = source.place;

            // Set all the previous values
            destination.PreviousStatus = destination.Status;
            destination.PreviousSector = destination.Sector;
            destination.PreviousPosition = destination.Position;
            destination.PreviousVelocity = destination.Velocity;

            // Set the current values
            destination.Sector = (Models.Sector)source.sector;
            destination.Position = source.Pos;

            // * -1 to get rid of the negative
            // [2] is the speed we need here
            destination.Velocity = source.localVel[2] * -1;
        }
    }
}

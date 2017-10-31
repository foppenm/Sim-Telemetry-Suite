import Vue from "vue";
import { paper } from "paper";
import { TelemetryHub } from "./TelemetryHub";

export class App {

    constructor() {
        this.handleConnected = this.handleConnected.bind(this);
        this.handleReceive = this.handleReceive.bind(this);

        this.viewModel = new Vue({
            el: '#app',
            data: {
                track: {
                    data: {},
                    vehicles: {}
                }
            }
        });

        // Create an empty project and a view for the canvas
        let canvas = document.getElementById('myCanvas');
        paper.setup(canvas);

        this.telemetryHub = new TelemetryHub("/telemetry");
        this.telemetryHub.on("connected", this.handleConnected);
        this.telemetryHub.on("receive", this.handleReceive);

        this.offset = new paper.Point(500, 100);

        // define content to reuse
        let vehicleDefinition = new paper.Path.Circle({
            radius: 5,
            fillColor: 'blue'
        });
        this.vehicleSymbol = new paper.Symbol(vehicleDefinition);
    }

    start() {
        this.vehicles = {};
        this.vehicleIds = [];

        // Draw the view
        paper.view.draw();
    }

    handleConnected(connection) {
        console.log("Now connected to '" + connection.host + "', connection ID: '" + connection.id + "'");
    }

    handleReceive(message) {
        let app = this;

        // Process the new data
        message.Vehicles.forEach((vehicle) => {
            let position = vehicle.Position;
            let newPoint = new paper.Point(app.offset.x + position[0], app.offset.y - position[2]);

            let existing = app.vehicles[vehicle.Id];
            if (existing) {
                // update existing raw data
                existing.raw = vehicle;

                if (existing.raw.CurrentSector1 === -1 && !existing.newLap && !existing.raw.Pit) {
                    // remove segments on complete lap
                    console.log(existing.raw.DriverName + " started a flying lap!");
                    existing.newLap = true;
                    existing.path.removeSegments();
                }
                else if (existing.newLap && existing.raw.CurrentSector1 > -1) {
                    // Clear newlap flag after 1st sector time
                    existing.newLap = false;
                }
                else if (existing.raw.Pit) {
                    // remove segments on pit entry
                    console.log(existing.raw.DriverName + " entered the pits.");
                    existing.newLap = false;
                    existing.path.removeSegments();
                }

                existing.path.add(newPoint);
                existing.instance.position = newPoint;
            }
            else {
                app.vehicles[vehicle.Id] = {
                    raw: vehicle,
                    path: new paper.Path({
                        strokeColor: 'black',
                        strokeWidth: 1
                    }),
                    instance: app.vehicleSymbol.place(newPoint),
                };

                app.vehicleIds.push(vehicle.Id);
            }
        });

        // TODO: Clean up disconnected vehicles
        //app.vehicleIds.forEach((id) => {
        //message.vehicles.find( ...
        //});

        // Reorder the vehicles for the timing table
        message.Vehicles.sort(function (a, b) { return a.Place - b.Place });

        // Update the viewmodel
        Vue.set(this.viewModel.track, 'vehicles', message.Vehicles);
        Vue.set(this.viewModel.track, 'data', message);

        paper.view.draw();
    }
}
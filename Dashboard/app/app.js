import Vue from "vue";
import { paper } from "paper";
import { TelemetryHub } from "./TelemetryHub";
import * as Toastr from 'toastr';

export class App {

    constructor() {
        this.handleReceive = this.handleReceive.bind(this);
        this.handleReceiveTrackPath = this.handleReceiveTrackPath.bind(this);
        this.handleMouseWheel = this.handleMouseWheel.bind(this);
        this.handleMouseDown = this.handleMouseDown.bind(this);
        this.handleMouseMove = this.handleMouseMove.bind(this);
        this.handleMouseUp = this.handleMouseUp.bind(this);
        this.handleResize = this.handleResize.bind(this);

        // Change toastr the settings
        Toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": true,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        this.viewModel = new Vue({
            el: '#app',
            data: {
                track: {
                    data: {},
                    vehicles: []
                }
            }
        });

        // Create an empty project and a view for the canvas
        let canvas = document.getElementById('myCanvas');
        paper.setup(canvas);

        this.telemetryHub = new TelemetryHub("/telemetry");
        this.telemetryHub.on("receive", this.handleReceive);
        this.telemetryHub.on("receiveTrackPath", this.handleReceiveTrackPath);

        this.offset = new paper.Point(0, 0);

        // define content to reuse
        let vehicleDefinition = new paper.Path.Circle({
            radius: 5,
            fillColor: 'blue'
        });
        this.vehicleSymbol = new paper.Symbol(vehicleDefinition);

        // register event handlers
        paper.view.onResize = this.handleResize;

        canvas.addEventListener("mousewheel", this.handleMouseWheel, false);
        canvas.addEventListener("mousedown", this.handleMouseDown, false);
        canvas.addEventListener("mousemove", this.handleMouseMove, false);
        canvas.addEventListener("mouseup", this.handleMouseUp, false);
    }

    start() {
        this.vehicles = [];
        this.trackMap = new paper.Path({
            strokeColor: '#CCCCCC',
            strokeWidth: 10
        });

        // Draw the view
        paper.view.draw();

        this.handleResize({ delta: { x: 0, y: 0 } });
    }

    handleMouseWheel(event) {
        let minZoom = 0.1;
        let maxZoom = 2.0;
        let zoomModifier = event.deltaY / 10000;
        let newZoom = paper.view.zoom - zoomModifier;

        if (newZoom > maxZoom || newZoom < minZoom) {
            return;
        }

        paper.view.zoom -= zoomModifier;
    }

    handleMouseDown(event) {
        this.dragStart = new paper.Point(event.clientX, event.clientY);
    }

    handleMouseUp(event) {
        this.dragStart = null;
    }

    handleMouseMove(event) {
        if (!this.dragStart) {
            return;
        }

        let delta = new paper.Point(event.movementX, event.movementY);
        paper.view.translate(delta);
    }

    handleResize(event) {

        let top, right, bottom, left = 0;

        this.vehicles.forEach((vehicle) => {
            // Vehicle :
            // {
            //    id: vehicle.Id,
            //    raw: vehicle,
            //    topSpeed: 0,
            //    path: new paper.Path({
            //        strokeColor: 'black',
            //        strokeWidth: 1
            //    }),
            //    instance: app.vehicleSymbol.place(newPoint)
            // });

            //if (top > parseFloat(vehicle.path.Position[0])) {

            //}

            //if (bottom > parseFloat(vehicle.path.Position[2])) {

            //}

            //if (vehicle.path.bounds.x === 0) {
            //    return;
            //}

        });


        let center = new paper.Point(event.delta.x, event.delta.y);
        paper.view.translate(center);
    }

    handleReceiveTrackPath(message) {
        let app = this;

        //app.trackMap.removeSegments();
        for (var key in message.Path) {
            let position = message.Path[key];
            let newPoint = new paper.Point(app.offset.x + parseFloat(position[0]), app.offset.y - parseFloat(position[2]));
            app.trackMap.add(newPoint);
        }

        paper.view.draw();
    }

    handleReceive(message) {
        let app = this;

        // Set each vehicle for cleanup, unless the incoming message contains this vehicle
        app.vehicles.forEach((vehicle) => {
            vehicle.cleanup = true;
        });

        // Process the new data
        message.Vehicles.forEach((vehicle) => {
            let position = vehicle.Position;
            if (!position) { return; }

            let newPoint = new paper.Point(app.offset.x + parseFloat(position[0]), app.offset.y - parseFloat(position[2]));

            let existing = app.vehicles.find(x => x.id === vehicle.Id);
            if (existing) {
                // update existing raw data
                existing.raw = vehicle;
                existing.cleanup = false;

                if (vehicle.PreviousSector === 3 && vehicle.Sector === 1 && !vehicle.NewLap) {
                    // Driver crosses start/finish
                    Toastr.success(existing.raw.DriverName + ": " + existing.raw.LastLapTime);
                    existing.lapDoesNotCount = false;
                    existing.path.removeSegments();
                }

                if (!existing.raw.Pit) {
                    existing.path.add(newPoint);
                }
                existing.instance.position = newPoint;

                // Set 'previous' values for comparison in the next loop
                existing.previousRaw = existing.raw;
            }
            else {
                app.vehicles.push({
                    id: vehicle.Id,
                    raw: vehicle,
                    path: new paper.Path({
                        strokeColor: 'black',
                        strokeWidth: 1
                    }),
                    instance: app.vehicleSymbol.place(newPoint)
                });
            }
        });

        // Cleanup vehicles that are disconnected
        if (app.vehicles.length > 0) {
            app.vehicles.reduceRight((total, vehicle, index, arr) => {
                if (!vehicle.cleanup) {
                    return;
                }
                vehicle.instance.remove();
                vehicle.path.remove();
                arr.splice(index, 1);
            });

            // Reorder the vehicles for the timing table
            app.vehicles.sort(function (a, b) { return a.raw.Place - b.raw.Place });
        }

        // Do something with other data (non vehicle related stuff)
        //if (message.Path) {
        //    console.log(message.Path);
        //}

        // Update the viewmodel
        Vue.set(this.viewModel.track, 'vehicles', app.vehicles);
        Vue.set(this.viewModel.track, 'data', message);

        paper.view.draw();
    }
}
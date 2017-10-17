import { paper } from "paper";
import { TelemetryHub } from "./TelemetryHub";
import { TrackModel } from "./TrackModel";

export class App {

    constructor() {
        this.handleConnected = this.handleConnected.bind(this);
        this.handleReceive = this.handleReceive.bind(this);
    }

    start() {
        this.telemetryHub = new TelemetryHub("/telemetry");
        this.telemetryHub.on("connected", this.handleConnected);
        this.telemetryHub.on("receive", this.handleReceive);

        // Draw the view now:
        paper.view.draw();
    }

    onMouseDrag(event) {
        let nativeDelta = new paper.Point(
            event.offsetX - this.mouseNativeStart.x,
            event.offsetY - this.mouseNativeStart.y
        );

        // Move into view coordinates to subract delta,
        //  then back into project coords.
        view.center = view.viewToProject(
            view.projectToView(this.viewCenterStart)
                .subtract(nativeDelta));
    }

    handleConnected(connection) {
        console.log("Now connected to '" + connection.host + "', connection ID: '" + connection.id + "'");
    }

    handleReceive(message) {

        let trackInstance = new TrackModel(message);

        //var project = new paper.Project();

        var path = new paper.Path();    // Create a Paper.js Path to draw a line into it
        path.strokeColor = 'black';     // Give the stroke a color
        path.strokeWidth = 10;
        let offset = new paper.Point(500, 100);

        trackInstance.vehicles.forEach(function (vehicle) {
            let position = vehicle.Pos;
            let point = new paper.Point(offset.x + position[0], offset.y - position[2]);

            let vehicleCircle = new paper.Path.Circle(point, 2);
            vehicleCircle.style = {
                fillColor: new paper.Color(0, 0, 0),
                strokeColor: new paper.Color(0, 0, 1),
                strokeWidth: 5
            };
        });

        //path.smooth({ type: 'catmull-rom', factor: 0.1 });
        //path.closed = true;

        //let points = [
        //    [100, 50, 0],
        //    [500, 300, 0],
        //    [100, 300, 0],
        //    [500, 500, 0],
        //    [700, 50, 0],
        //    [500, 0, 0],
        //    [0, -100, 0],
        //    [100, 50, 0]
        //];

        //var path = new paper.Path();    // Create a Paper.js Path to draw a line into it
        //path.strokeColor = 'black';     // Give the stroke a color
        //path.strokeWidth = 10;

        //// Draw all points
        //let offset = 250;
        //for (var i = 0; i < points.length; i++) {
        //    let currentPoint = points[i];
        //    let point = new paper.Point(currentPoint[0] + offset, currentPoint[1] + offset);

        //    if (i === 0) {
        //        // Move to start and draw a line from there
        //        path.moveTo(point);
        //    }

        //    path.lineTo(point);
        //}

        //// draw the circle
        //let vehicle = new paper.Path.Circle(0, 100, 4);
        //vehicle.strokeColor = 'red';
        //vehicle.strokeWidth = 10;

        //vehicle.onFrame = function (event) {
        //    if (offset < path.length) {
        //        vehicle.position = path.getPointAt(offset);
        //        offset += event.delta * 50; // speed - 150px/second
        //    }
        //    else {
        //        offset = 0;
        //    }
        //}

        paper.view.draw();
    }
}
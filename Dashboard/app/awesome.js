import { paper } from "paper";

export default function () {
    let points = [
        [100, 50, 0],
        [500, 300, 0],
        [100, 300, 0],
        [500, 500, 0],
        [700, 50, 0],
        [500, 0, 0],
        [0, -100, 0],
        [100, 50, 0],
    ];

    var path = new paper.Path();    // Create a Paper.js Path to draw a line into it
    path.strokeColor = 'black';     // Give the stroke a color
    path.strokeWidth = 10;

    // Draw all points
    let offset = 250;
    for (var i = 0; i < points.length; i++) {
        let currentPoint = points[i];
        let point = new paper.Point(currentPoint[0] + offset, currentPoint[1] + offset);

        if (i === 0) {
            // Move to start and draw a line from there
            path.moveTo(point);
        }

        path.lineTo(point);
    }

    path.smooth({ type: 'catmull-rom', factor: 0.1 });
    //path.smooth({ type: 'geometric' });
    //path.smooth({ type: 'continuous' });
    path.fullySelected = true;
    path.closed = true;

    // draw the circle
    let vehicle = new paper.Path.Circle(0, 100, 4);
    vehicle.strokeColor = 'red';
    vehicle.strokeWidth = 10;

    vehicle.onFrame = function (event) {
        if (offset < path.length) {
            vehicle.position = path.getPointAt(offset);
            offset += event.delta * 150; // speed - 150px/second
        }
        else {
            offset = 0;
        }
    }

    // Draw the view now:
    paper.view.draw();
}

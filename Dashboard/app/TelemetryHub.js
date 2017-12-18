import EventEmitter from "eventemitter3";
import { HubConnection } from '@aspnet/signalr-client';

export class TelemetryHub extends EventEmitter {

    constructor(hostUrl) {
        super();

        // Set up the hub connection
        let connection = new HubConnection('/telemetry');

        connection.on("status", (message) => {
            this.emit('receive', JSON.parse(message));
        });

        connection.on("trackpath", (message) => {
            this.emit('receiveTrackPath', JSON.parse(message));
        });

        connection.on("done", (connection) => {
            this.emit('connected', connection);
        });

        connection.on("fail", (connection) => {
            console.log("Could not connect to '" + connection.host + "'");
        });

        connection.start();
    }
}

﻿import EventEmitter from "eventemitter3";

//import 'signalr/jquery.signalR.js';
import { HubConnection } from '@aspnet/signalr-client';

export class TelemetryHub extends EventEmitter {

    constructor(hostUrl) {
        super();

        //this.connection = $.hubConnection(hostUrl);
        //this.proxy = this.connection.createHubProxy("telemetryHub");

        //this.proxy.on('status', (message) => {
        //    this.emit('receive', JSON.parse(message));
        //});

        //connection.start()
        //    .done((connection) => {
        //        this.emit('connected', connection);
        //    })
        //    .fail((connection) => {
        //        console.log("Could not connect to '" + connection.host + "'");
        //    });

        // Set up the hub connection
        let connection = new HubConnection('/telemetry');

        connection.on("status", (message) => {
            this.emit('receive', JSON.parse(message));
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

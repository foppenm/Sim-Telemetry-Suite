import EventEmitter from "eventemitter3";

export class TelemetryHub extends EventEmitter {

    constructor(hostUrl) {
        super();

        this.connection = $.hubConnection(hostUrl);
        this.proxy = this.connection.createHubProxy("telemetryHub");

        this.proxy.on('status', (message) => {
            this.emit('receive', JSON.parse(message));
        });

        this.connection.start()
            .done((connection) => {
                this.emit('connected', connection);
            })
            .fail((connection) => {
                console.log("Could not connect to '" + connection.host + "'");
            });
    }
}

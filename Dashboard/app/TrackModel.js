
export class TrackModel {

    constructor(json) {
        this.application = json.application;
        this.trackName = json.trackName;
        this.vehicles = json.vehicles;
    }

}

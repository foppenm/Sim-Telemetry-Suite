# Sim-Telemetry-Suite
The Sim-Telemetry-Suite is a solution that can receive data from your simracing server and show it in a dashboard.

## Features
* Generate track map based on live driver positions
* Show live driver timing details
* More to come ...

## Technologies
Sim-Telemetry-Suite is built upon a number of technologies:
* [rFactor2 Plugin Example](https://www.studio-397.com/modding-resources/) (C++)
* .NET Core with ASP.NET Core (C#)
* ASP.NET Core SignalR (C#)
  * [Server](https://www.nuget.org/packages/Microsoft.AspNetCore.SignalR)
  * [Client](https://github.com/aspnet/SignalR)
* JavaScript libraries/frameworks
  * [Node.js](https://nodejs.org/en/) with [NPM](https://www.npmjs.com/)
  * [Paper.js](http://paperjs.org/)
  * [Vue.js](https://vuejs.org/)
  * [Foundation-sites](https://foundation.zurb.com/sites.html)
  * [JQuery](https://jquery.com/)
  * [eventemitter3](https://github.com/primus/eventemitter3)

## Contribution
Want to contribute to the project? Take a look at open issues if you like. I'm also open for code reviews and pull requests. Looking forward to your feedback and/or improvements to the project!

### Prerequisites
```
SteamCMD
rFactor2 Dedicated Server + Content (At least one track, vehicle, mod)
Visual Studio 2017
Node.js
.NET Core 2.0
```

### Installing
Section in progress...
```
Place [Bridge] output in Bin64
Place [Dashboard] output in Local IIS, or start with [dotnet run]
Start [Receiver] with [dotnet run]
```

### Running the tests
Execute the following commands from root:
```
cd .\Tests\Receiver.Tests\
dotnet xunit
```

## Authors
* **André Geuze** - *Initial work* - [andregeuze](https://github.com/andregeuze)

See also the list of [contributors](https://github.com/andregeuze/Sim-Telemetry-Suite/contributors) who participated in this project.

## License
This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments
* Thanks to Image Space Incorporated and Studio397 for delivering us the next-gen racing simulation the world needs :)
* Inspired by LiveRacers.info
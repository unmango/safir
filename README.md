# Safir Media Manager

A distributed take on media management.

# THIS IS VERY OUT OF DATE AND I HAVEN'T UPDATED IT YET

## CI/CD

Safir uses Azure DevOps for continuous integration/continuous deployment.
The project is located at <https://dev.azure.com/unmango/Safir>.
Builds can be found [here](https://dev.azure.com/unmango/Safir/_build?definitionId=2).
The YAML for the pipeline is located in `azure-pipelines.yml`.

The pipeline is currently very much a work-in-progress. I plan to have a multi-stage pipeline to build and deploy straight from the versioned pipeline here, and avoid Azure DevOps releases UI.

## Build Status

| Platform | Master | Release X |
|:-------- |:------:|:---------:|
| All      |[![Build Status](https://dev.azure.com/unmango/Safir/_apis/build/status/unmango.safir?branchName=master)](https://dev.azure.com/unmango/Safir/_build/latest?definitionId=2&branchName=master)| No release builds yet |

## Building

Safir is built using a variety of technologies, primarily .NET and Angular. Docker images will likely be available at some point. Other technologies may come and go as I experiment with them.

### Server

The server side of Safir is built using [.NET Core 3.1](https://dotnet.microsoft.com/).
The SDK can be downloaded at <https://dotnet.microsoft.com/download>.

You can check the installed version with

```bash
dotnet --version
3.1.100
```

or

```bash
dotnet --list-sdks
...
3.0.100 [C:\\Program Files\\dotnet\\sdk]
3.1.100 [C:\\Program Files\\dotnet\\sdk]
...
```

To build the entire solution, you can open `Safir.sln` in your favorite IDE and build from there.
Or `cd` into the directory containing `Safir.sln` and run

```bash
dotnet build
```

### Client

Currently, client apps are built using [Angular](https://angular.io/) and [Electron](https://electronjs.org/).
The fastest way to get up and running with the client apps is to install the lastest version of [Node](https://nodejs.org).

You can check the installed version with

```bash
node --version
v12.9.1

npm --version
6.10.2
```

To build either of the projects, `cd` into the directory of the project you intend to build that contains the `package.json`.
Then install dependencies with

```bash
npm install .
```

#### Angular

To build and run the Angular web app:

```bash
cd src/UI/Safir.Angular
npm run build
# Or to build and run
npm run start
```

#### Electron

To build and run the electron desktop app:

```bash
cd src/UI/Safir.Electron
npm run start:electron
```

## License

GNU General Public License v3.0 or later

See [COPYING](COPYING) to see the full text.

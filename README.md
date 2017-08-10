# App Metrics Reporting <img src="http://app-metrics.io/logo.png" alt="App Metrics" width="50px"/> 
[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg?style=flat-square)](http://app-metrics.io/getting-started/intro.html) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](https://opensource.org/licenses/Apache-2.0)

## What is it?

The repo contains reporting extension packages to [App Metrics](https://github.com/alhardy/AppMetrics) which provide metric reporting capabilities.

## Latest Builds, Packages & Repo Stats

|Branch|AppVeyor|Travis|Coverage|
|------|:--------:|:--------:|:--------:|
|dev|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/reporting/dev.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/reporting/branch/dev)|[![Travis](https://img.shields.io/travis/alhardy/AppMetrics.Reporters/dev.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/AppMetrics.Reporters)|[![Coveralls](https://img.shields.io/coveralls/AppMetrics/Reporting/dev.svg?style=flat-square)](https://coveralls.io/github/AppMetrics/Reporting?branch=dev)
|master|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/reporting/master.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/reporting/branch/master)| [![Travis](https://img.shields.io/travis/alhardy/AppMetrics.Reporters/master.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/AppMetrics.Reporters)| [![Coveralls](https://img.shields.io/coveralls/AppMetrics/Reporters/master.svg?style=flat-square)](https://coveralls.io/github/AppMetrics/Reporters?branch=master)|

|Package|Dev Release|Pre-Release|Release|
|------|:--------:|:--------:|:--------:|
|App.Metrics.AspNetCore.Reporting|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Reporting.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Reporting)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Reporting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Reporting/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Reporting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Reporting/)
|App.Metrics.Reporting|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Reporting.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Reporting)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Reporting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Reporting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting/)
|App.Metrics.Reporting.Abstractions|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Reporting.Abstractions.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Reporting.Abstractions)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Reporting.Abstractions.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.Abstractions/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Reporting.Abstractions.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.Abstractions/)
|App.Metrics.Reporting.Core|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Reporting.Core.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Reporting.Core)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Reporting.Core.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.Core/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Reporting.Core.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.Core/)
|App.Metrics.Reporting.Console|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Reporting.Console.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Reporting.Console)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Reporting.Console.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.Console/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Reporting.Console.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.Console/)
|App.Metrics.Reporting.TextFile|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Reporting.TextFile.svg?style=flat-square)](https://www.myget.org/feed/alhardy/package/nuget/App.Metrics.Reporting.TextFile)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Reporting.TextFile.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.TextFile/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Reporting.TextFile.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.TextFile/)
|App.Metrics.Reporting.Http|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Reporting.Http.svg?style=flat-square)](https://www.myget.org/feed/alhardy/package/nuget/App.Metrics.Reporting.Http)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Reporting.Http.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.Http/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Reporting.Http.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Reporting.Http/)|

## Other Reporters for App Metrics

- [InfluxDB](https://github.com/alhardy/AppMetrics.Extensions.InfluxDB)
- [Elasticsearch](https://github.com/alhardy/AppMetrics.Extensions.Elasticsearch)
- [Prometheus](https://github.com/alhardy/AppMetrics.Extensions.Prometheus)
- [Graphite](https://github.com/alhardy/AppMetrics.Extensions.Graphite)

## How to build

[AppVeyor](https://ci.appveyor.com/project/alhardy/reporting/branch/master) and [Travis CI](https://travis-ci.org/alhardy/AppMetrics.Reporters) builds are triggered on commits and PRs to `dev` and `master` branches.

See the following for build arguments and running locally.

|Configuration|Description|Default|Environment|Required|
|------|--------|:--------:|:--------:|:--------:|
|BuildConfiguration|The configuration to run the build, **Debug** or **Release** |*Release*|All|Optional|
|PreReleaseSuffix|The pre-release suffix for versioning nuget package artifacts e.g. `beta`|*ci*|All|Optional|
|CoverWith|**DotCover** or **OpenCover** to calculate and report code coverage, **None** to skip. When not **None**, a coverage file and html report will be generated at `./artifacts/coverage`|*OpenCover*|Windows Only|Optional|
|SkipCodeInspect|**false** to run ReSharper code inspect and report results, **true** to skip. When **true**, the code inspection html report and xml output will be generated at `./artifacts/resharper-reports`|*false*|Windows Only|Optional|
|BuildNumber|The build number to use for pre-release versions|*0*|All|Optional|


### Windows

Run `build.ps1` from the repositories root directory.

```
	.\build.ps1'
```

**With Arguments**

```
	.\build.ps1 --ScriptArgs '-BuildConfiguration=Release -PreReleaseSuffix=beta -CoverWith=OpenCover -SkipCodeInspect=false -BuildNumber=1'
```

### Linux & OSX

Run `build.sh` from the repositories root directory. Code Coverage reports are now supported on Linux and OSX, it will be skipped running in these environments.

```
	.\build.sh'
```

**With Arguments**

```
	.\build.sh --ScriptArgs '-BuildConfiguration=Release -PreReleaseSuffix=beta -BuildNumber=1'
```

## Contributing

See the [contribution guidlines](https://github.com/alhardy/AppMetrics/blob/master/CONTRIBUTING.md) in the [main repo](https://github.com/alhardy/AppMetrics) for details.

## Acknowledgements

* [DocFX](https://dotnet.github.io/docfx/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [XUnit](https://xunit.github.io/)
* [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
* [Cake](https://github.com/cake-build/cake)
* [OpenCover](https://github.com/OpenCover/opencover)

***Thanks for providing free open source licensing***

* [Jetbrains](https://www.jetbrains.com/dotnet/) 
* [AppVeyor](https://www.appveyor.com/)
* [Travis CI](https://travis-ci.org/)
* [Coveralls](https://coveralls.io/)

## License

This library is release under Apache 2.0 License ( see LICENSE ) Copyright (c) 2016 Allan Hardy

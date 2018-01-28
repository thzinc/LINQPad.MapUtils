# Quick and dirty mapping extensions for LINQPad

Tools for visualizing data on a map in [LINQPad](https://www.linqpad.net/).

## Quickstart

1. Add the `LINQPad.MapUtils` package to your query in LINQPad
2. Use `.DumpMarkers()` or `.DumpRoute()` to add markers or route lines to a map in a new result pane

```csharp
var points = new[] {
    new {
        Name = "Taco Stand",
        Lat = 1.234,
        Lon = 2.345,
    },
    new {
        Name = "Beverage Palace",
        Lat = 3.456,
        Lon = 4.567,
    },
};

points.DumpMarkers(x => (x.Lat, x.Lon), x => x.Name);
```

## Building

[![NuGet](https://img.shields.io/nuget/v/LINQPad.MapUtils.svg)](https://www.nuget.org/packages/LINQPad.MapUtils/)

This project references `LINQPad.exe` in order to work. Make sure the reference to that assembly is correct on your system.

## Code of Conduct

We are committed to fostering an open and welcoming environment. Please read our [code of conduct](CODE_OF_CONDUCT.md) before participating in or contributing to this project.

## Contributing

We welcome contributions and collaboration on this project. Please read our [contributor's guide](CONTRIBUTING.md) to understand how best to work with us.

## License and Authors

[![Daniel James logo](https://secure.gravatar.com/avatar/eaeac922b9f3cc9fd18cb9629b9e79f6.png?size=16) Daniel James](https://github.com/thzinc)

[![license](https://img.shields.io/github/license/thzinc/LINQPad.MapUtils.svg)](https://github.com/thzinc/LINQPad.MapUtils/blob/master/LICENSE)
[![GitHub contributors](https://img.shields.io/github/contributors/thzinc/LINQPad.MapUtils.svg)](https://github.com/thzinc/LINQPad.MapUtils/graphs/contributors)

This software is made available by Daniel James under the MIT license.
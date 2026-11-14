## About

`Harp.TimestampGeneratorGen3` provides an asynchronous API and reactive operators for data acquisition and control of [Harp Timestamp Generator Gen3](https://harp-tech.org/device.timestampgeneratorgen3) devices.

## How to Use

To use `Harp.TimestampGeneratorGen3` for visual reactive programming, please install this package using the [Bonsai package manager](https://bonsai-rx.org/docs/articles/packages.html).

The package can also be used from any .NET application:
```c#
using Harp.TimestampGeneratorGen3;

using var device = await Device.CreateAsync("COM3");
var whoAmI = await device.ReadWhoAmIAsync();
var deviceName = await device.ReadDeviceNameAsync();
var timestamp = await device.ReadTimestampSecondsAsync();
Console.WriteLine($"{deviceName} WhoAmI: {whoAmI} Timestamp (s): {timestamp}");
```

## Additional Documentation

For additional documentation and examples, refer to the [official Harp Timestamp Generator Gen3 documentation](https://harp-tech.org/device.timestampgeneratorgen3).

## Feedback & Contributing

`Harp.TimestampGeneratorGen3` is released as open source under the [MIT license](https://licenses.nuget.org/MIT). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/harp-tech/device.timestampgeneratorgen3).

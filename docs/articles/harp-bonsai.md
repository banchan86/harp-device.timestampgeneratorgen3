## Bonsai

Bonsai is a visual reactive programming language for building interactive experiments and processing data streams in real time. It supports a growing ecosystem of hardware and software packages that are commonly used in neuroscience. This article will cover how to set up the Timestamp Generator Gen3 in Bonsai.

>[!TIP]
> More information on Bonsai can be found in the official [documentation](https://bonsai-rx.org/docs/).

### First Steps

We will use an introductory example to connect and test the device in Bonsai. This example enables the [heartbeat](./set-clock.md) event and displays the device's current timestamp as it updates every second. We revisit this example in more detail in the "Bonsai Workflows" section.

Before beginning:
- Connect the [USB](connections.md) cable to the computer.
- Launch "Bonsai" from the Windows Start menu.
- Hover over the workflow cell below, and click on the "Copy" icon on the top right.
- Paste the workflow into Bonsai.

:::workflow
![Timestamp Generator Gen3 First Steps](../workflows/timestampgeneratorgen3-firststeps.bonsai)
:::

> [!TIP]
> The [Harp device pattern](https://harp-tech.org/articles/operators.html#device-pattern) will initialize the device, log data, and provide hooks to send commands as well as receive messages from the Timestamp Generator Gen3 using the [Harp communication protocol](https://harp-tech.org/protocol/BinaryProtocol-8bit.html). If your workflow does not look like the one above, make sure that the [Harp.TimestampGeneratorGen3](./installation.md#software-packages) package is installed.

- Click on the [`TimestampGeneratorGen3 (Device)`] operator and set the `PortName` property to the communications port for the device (e.g. COM8).
- Click on the [`TimestampGeneratorGen3 (DeviceDataWriter)`] operator and set the `Path` property for the name and location of the save file (e.g. `Data\TimestampGeneratorGen3.harp`).
- Press the "Start" button in Bonsai to run the workflow.

A [visualizer](xref:Bonsai.Design.VisualizerWindow) will automatically open when the workflow starts, displaying the device timestamp every second:

```text
345
346
```

The device is ready to use! If, instead, an error appears in Bonsai, check out the [troubleshooting](troubleshooting.md) section.

Next, we suggest going through the "Bonsai Workflows" section if you are not familiar with using Harp devices in Bonsai.

Alternatively, if you have experience with Harp devices, you can check the [register table](xref:Harp.TimestampGeneratorGen3) in the reference to access the device functionality directly.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`TimestampGeneratorGen3 (Device)`]: xref:Harp.TimestampGeneratorGen3.Device
[`TimestampGeneratorGen3 (DeviceDataWriter)`]: xref:Harp.TimestampGeneratorGen3.DeviceDataWriter

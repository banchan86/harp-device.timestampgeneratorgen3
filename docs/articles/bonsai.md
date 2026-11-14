# Bonsai

Bonsai is a visual reactive programming language for building interactive experiments and processing data streams in real time. It supports a growing ecosystem of hardware and software packages that are commonly used in neuroscience. This article will cover how to start using the Timestamp Generator Gen3 in Bonsai.

> [!TIP]
> More information on Bonsai can be found in the official [documentation](https://bonsai-rx.org/docs/).

## First steps

We will use an introductory example to get started with the device in Bonsai. This example enables the heartbeat event and displays the device's current timestamp as it updates every second. We revisit this example in more detail in the "Examples" section.

Before beginning:
- Install [Bonsai](installation.md?tabs=bonsai#software-packages) and add the `Harp.TimestampGeneratorGen3` and `Bonsai.StarterPack` packages.
- Connect the [USB](ports-connections.md) cable to the computer for power and communication.
- Launch "Bonsai" from the Windows Start menu.
- Hover over the workflow cell below, and click on the "Copy" icon on the top right.
- Paste the workflow into Bonsai.

:::workflow
![Timestamp Generator Gen3 first steps](../workflows/timestampgeneratorgen3-firststeps.bonsai)
:::

> [!TIP]
> If your workflow does not look like the one above, make sure that the [Harp.TimestampGeneratorGen3 and Bonsai.StarterPack](./installation.md#software-packages) packages are installed.

- Click on the [`TimestampGeneratorGen3 (Device)`] operator and set the `PortName` property to the communications port for the device (e.g. `COM8`).
- Click on the [`TimestampGeneratorGen3DataWriter (DeviceDataWriter)`] operator and set the `Path` property for the name and location of the save folder (e.g. `TimestampGeneratorGen3.harp`).
- Press the "Start" button in Bonsai to run the workflow.

A [visualizer](xref:Bonsai.Design.VisualizerWindow) will automatically open when the workflow starts, displaying the device timestamp every second:

```text
345
346
```

The device is ready to use! If, instead, an error appears in Bonsai, check out the [troubleshooting](errors.md) section.

Next, we suggest going through the "Examples" section if you are not familiar with using Harp devices in Bonsai.

Alternatively, if you have experience with Harp devices, you can check the [register table](xref:Harp.TimestampGeneratorGen3.Device) in the reference to access the device functionality directly.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`TimestampGeneratorGen3 (Device)`]: xref:Harp.TimestampGeneratorGen3.Device
[`TimestampGeneratorGen3DataWriter (DeviceDataWriter)`]: xref:Harp.TimestampGeneratorGen3.DeviceDataWriter

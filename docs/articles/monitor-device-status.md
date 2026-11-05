# Monitor device status

The device status can be checked physically, using the [OPTION](ports-connections.md#front-panel) button and the status indicators on the front panel. The battery charge and the connected devices are also accessible from within Bonsai when you need to log them alongside your data or use the information in a workflow.

This article covers how to check the battery charge, configure the battery rate, visualize the battery events, and check the connected devices in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Monitor device status](../workflows/monitordevicestatus-toplevel.bonsai)
:::

## Check battery charge

The [`Battery`] register holds the battery voltage, averaged by the firmware over the last 16 one-second samples. It can be read on demand at any time, even when the periodic battery events (covered in the next section) are disabled.

:::workflow
![Check battery charge](../workflows/monitordevicestatus-checkbattery.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `BatteryPayload`.
    - `Battery` - Leave at the default, the payload value is ignored by the device for a read request.
    - `MessageType` - Select `Read`.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>A</kbd>. The device replies with the current battery voltage, which the next sections show how to visualize.

## Configure the battery rate

Alternatively, you can set the [`BatteryRate`] register to make the device report the battery charge at regular intervals.

:::workflow
![Configure the battery rate](../workflows/monitordevicestatus-batteryrate.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `BatteryRatePayload`.
    - `BatteryRate` - Select the reporting rate for the event (e.g. `EverySecond`). To stop the events in the future, select `Never`.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>S</kbd>. The device starts reporting [`Battery`] events every second.

## Visualize battery events

The Timestamp Generator Gen3 reports the battery voltage as a [`Battery`] event stream, both in reply to a read request and at the configured battery rate. The workflow below will show you how to decode and visualize the battery events:

:::workflow
![Visualize battery events](../workflows/monitordevicestatus-battery.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`. This will listen to messages broadcast from the [`PublishSubject`] named `TimestampGeneratorGen3 Events` in the Harp device pattern.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedBattery`.
- Insert a [`VisualizerWindow`] operator. This will automatically open a window displaying the parsed events when the workflow starts.

Run the workflow and press <kbd>A</kbd> to request the current battery voltage on demand, or press <kbd>S</kbd> to set the battery rate and receive a new value at regular intervals. The visualizer will display the battery voltage, in volts, followed by the timestamp on the device clock.

> [!WARNING]
> **TODO**: Capture a real visualizer line (e.g. `<voltage>@<timestamp>`) from the device and add it here as a text sample.

> [!NOTE]
> If the [BATTERY](ports-connections.md#front-panel) switch is off or the battery isn't detected, a battery read returns zero and no events are reported.

## Check connected devices

Use the [`DevicesConnected`] register to check which clock output ports (`Out0`–`Out5`) currently have a device connected.

:::workflow
![Check connected devices](../workflows/monitordevicestatus-devicesconnected.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `DevicesConnectedPayload`.
    - `DevicesConnected` - Leave at the default, the payload value is ignored by the device for a read request.
    - `MessageType` - Select `Read`.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

In a separate branch:

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedDevicesConnected`.
- Insert a [`VisualizerWindow`] operator to display the reply.

Run the workflow and press <kbd>D</kbd>. The visualizer window shows the set of ports with a device connected (e.g. `Out0, Out2`), matching the **OUT** LEDs lit on the front panel.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`Battery`]: xref:Harp.TimestampGeneratorGen3.Battery
[`BatteryRate`]: xref:Harp.TimestampGeneratorGen3.BatteryRate
[`DevicesConnected`]: xref:Harp.TimestampGeneratorGen3.DevicesConnected
[`CreateMessage`]: xref:Harp.TimestampGeneratorGen3.CreateMessage
[`Parse`]: xref:Harp.TimestampGeneratorGen3.Parse
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject

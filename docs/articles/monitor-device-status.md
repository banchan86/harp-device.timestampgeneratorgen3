# Monitor device status

The device status can be checked physically, using the [OPTION](ports-connections.md#front-panel) button and the status indicators on the front panel. The battery charge and the connected devices are also accessible from within Bonsai when you need to log them alongside your data or use the information in a workflow.

This article covers how to check the battery charge, configure the battery telemetry rate, visualize the battery events, and check the connected devices in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Monitor device status](../workflows/monitordevicestatus-toplevel.bonsai)
:::

## Check battery charge

The [`Battery`] register holds the battery voltage, averaged by the firmware over the last 16 one-second samples. It can be read on demand at any time, even when the periodic battery events (covered in the next section) are disabled. The battery reading is reported as a [`Battery`] event, which we will cover in a later section.

:::workflow
![Check battery charge](../workflows/monitordevicestatus-checkbattery.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `MessageType` - Select `Read`.
    - `Payload` - Select `BatteryPayload`.
    - `Battery` - Leave at the default, the payload value is ignored by the device for a read request.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

## Configure the battery rate

Alternatively, you can configure [`BatteryRate`] to make the device report the [`Battery`] event at regular intervals. We will cover how to visualize the [`Battery`] event in the next section.

:::workflow
![Configure the battery rate](../workflows/monitordevicestatus-batteryrate.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `BatteryRatePayload`.
    - `BatteryRate` - Select the reporting rate for the event (e.g. `EverySecond`). To stop the events in the future, select `Never`.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

## Visualize battery events

The device reports the battery voltage as a [`Battery`] event stream, both in reply to a read request and at the configured battery rate. The workflow below will show you how to decode and visualize the battery events:

:::workflow
![Visualize battery events](../workflows/monitordevicestatus-battery.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedBattery`.
- Insert a [`VisualizerWindow`] operator.

Run the workflow and press <kbd>A</kbd> to request the current battery voltage on demand, or press <kbd>S</kbd> to set the battery rate and receive a new value at regular intervals. The visualizer will display:

```text
3.71@95.563344
3.71@96.563344
```

The first number is the battery voltage, in volts, and the second number is the timestamp on the device clock.

> [!NOTE]
> If the [BATTERY](ports-connections.md#front-panel) switch is off or the battery isn't detected, a battery read returns zero and no events are reported.

## Check connected devices

[`DevicesConnected`] reports which clock output ports (`Out0`–`Out5`) currently have a device connected.

:::workflow
![Check connected devices](../workflows/monitordevicestatus-devicesconnected.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `MessageType` - Select `Read`.
    - `Payload` - Select `DevicesConnectedPayload`.
    - `DevicesConnected` - Leave at the default, the payload value is ignored by the device for a read request.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

In a separate branch:

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedDevicesConnected`.
- Insert a [`VisualizerWindow`] operator to display the reply.

Run the workflow and press <kbd>D</kbd>. The visualizer will display:

```text
Out0, Out2@95.563344
```

The first part is the set of ports with a device connected, matching the **OUT** LEDs lit on the front panel, and the number is the timestamp on the device clock.

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

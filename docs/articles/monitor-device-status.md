## Monitor Device Status

The device status can be checked physically, using the [`OPTION` button](connections.md#ports-and-controls) and [indicator lights](troubleshooting.md#indicator-lights) on the front panel. The battery charge and connected devices are also accessible from within Bonsai when you need to log them alongside your data or use the information in a workflow. 

This article covers how to check the battery charge, configure the battery telemetry rate, observe the battery events, and check the connected devices from Bonsai.

The complete workflow is shown below:

:::workflow
![Monitor Device Status Top Level](../workflows/monitordevicestatus-toplevel.bonsai)
:::

### Check Battery Charge

The [`Battery`] register holds the battery voltage, averaged by the firmware over the last 16 one-second samples. It can be read on demand at any time, even when the periodic [battery events](#observe-battery-events) are disabled.

:::workflow
![Check Battery Charge](../workflows/monitordevicestatus-checkbattery.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `MessageType` - Set to `Read`.
    - `Payload` - Set to `Battery`.
    - `Battery` - Leave at the default, the payload value is ignored by the device for a read request.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

### Configure the Battery Rate

Alternatively, you can enable the [`BatteryRate`] event, which will broadcast the battery charge at regular intervals.

:::workflow
![Configure the Battery Rate](../workflows/monitordevicestatus-batteryrate.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `BatteryRate`.
    - `BatteryRate` - Set to the desired telemetry rate for the event (e.g. `EverySecond`).
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

### Observe Battery Events

To read the battery charge or battery rate events, subscribe to the device events and parse the [`Battery`] event messages. Each [`Battery`] message will carry the battery voltage, as well as the hardware timestamp.

:::workflow
![Observe Battery Events](../workflows/monitordevicestatus-battery.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`. This will listen to messages broadcast from the [`PublishSubject`] named `TimestampGeneratorGen3 Events` in the Harp device pattern.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedBattery`.
- Insert a [`VisualizerWindow`] operator. This will automatically open a window displaying the parsed events when the workflow starts.

Run the workflow and press <kbd>A</kbd> to request the current battery voltage on demand, or press <kbd>S</kbd> to set the battery rate and receive a new value at regular intervals. The battery voltage will appear in the visualizer window.

> [!NOTE]
> If the [`BATTERY` switch](connections.md#ports-and-controls) is off or the battery isn't detected, a battery read will return zero and no events will be sent.

### Check Connected Devices

use the [`DevicesConnected`] register to check which clock output ports (`Out0`–`Out5`) currently have a device connected. 

:::workflow
![Check Connected Devices](../workflows/monitordevicestatus-devicesconnected.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `MessageType` - Set to `Read`.
    - `Payload` - Set to `DevicesConnected`.
    - `DevicesConnected` - Leave at the default, the payload value is ignored by the device for a read request.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

In a separate branch:

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedDevicesConnected`.
- Insert a [`VisualizerWindow`] operator to display the reply.

Run the workflow and press <kbd>D</kbd>. The visualizer window will show the set of ports with a device connected (e.g. `Out0, Out2`), matching the `OUT` LEDs lit on the front panel.

[!INCLUDE [](version-footer.md)]

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

## Set Clock

In [generator](./connections.md#connections) mode, the Timestamp Generator Gen3 sends the current time on its onboard clock to all connected devices as a synchronization signal. Setting the clock time will also set the time for all connected devices, which is useful for resetting the timestamps for every experiment.

This article covers how to read the device clock as well as how to set it from within Bonsai.

The complete workflow is shown below:

:::workflow
![Set Clock Top Level](../workflows/setclock-toplevel.bonsai)
:::

> [!NOTE]
> You can copy the workflow from the workflow cell or find and add these operators from the Bonsai [Toolbox](https://bonsai-rx.org/docs/articles/editor.html?tabs=mouse-controls#toolbox). Reading and setting the device clock is a core functionality shared by all Harp devices. Thus, operators marked with a `(Harp)` suffix come from the `Bonsai.Harp` base library (installed automatically as a dependency) instead of the device-specific package.

> [!WARNING]
> The internal clock will also reset if the device is powered off (if it is disconnected from USB while the [BATTERY](connections.md#front-panel) switch is off or if the battery is depleted).

### Visualize Heartbeat Event

By default, the Timestamp Generator Gen3 does not produce a readout of the timestamps that it generates. However, enabling the `Heartbeat` property of the [`Device`] operator will generate a visible timestamp event that mirrors the synchronization signal that is sent out every second. To do this:

:::workflow
![Enable Heartbeat Event](../workflows/setclock-heartbeat.bonsai)
:::

- Click on the [`Device`] operator in the Harp device pattern and set the `Heartbeat` property to `Enabled`.

To visualize the heartbeat events, subscribe to the device events and parse the [`TimestampSeconds`] messages:

:::workflow
![Visualize Heartbeat Event](../workflows/setclock-visualizeheartbeat.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`. This will listen to messages broadcast from the [`PublishSubject`] named `TimestampGeneratorGen3 Events` in the Harp device pattern.
- Insert a [`Parse (Harp)`] operator and set the `Register` property to `TimestampSeconds`.
- Insert a [`VisualizerWindow`] operator. This will automatically open a window displaying the parsed events when the workflow starts.

Run the workflow, and the visualizer will start streaming the timestamp every second, like this:

```text
345
346
```

### Set Clock with KeyDown

Writing to the [`TimestampSeconds`] register sets the device's clock, in whole seconds.

> [!WARNING]
> Make sure the device clock is [unlocked](connections.md#front-panel) before setting the clock. If the clock is locked, the device will throw an error:
>```text
>Runtime error
>
>The device reported an erroneous write command. 
>Payload 0, Address: 8, Type: U32
>```

:::workflow
![Set Clock Value](../workflows/setclock-clockvalue.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage (Harp)`] operator and configure the following properties:
    - `Payload` - Set to `TimestampSeconds`.
    - `TimestampSeconds` - Set to 0 to reset the clock.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`. This will send the messages to the device.

Run the workflow and press <kbd>A</kbd> to set the clock. 

### Alternative: Set Clock with Timer

You can replace [`KeyDown`] with other operators to set the clock with other triggers in Bonsai, for instance a [`Timer`].

:::workflow
![Set Clock Timer](../workflows/setclock-timer.bonsai)
:::

- Insert a [`Timer`] operator and set the `DueTime` property to the number of seconds to wait before setting the clock (e.g. 2 seconds).
- Insert a [`CreateMessage (Harp)`] operator and configure the following properties:
    - `Payload` - Set to `TimestampSeconds`.
    - `TimestampSeconds` - Set to 0 to reset the clock.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and observe the clock being set after 2 seconds.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`TimestampSeconds`]: xref:Bonsai.Harp.TimestampSeconds
[`CreateMessage (Harp)`]: xref:Bonsai.Harp.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`Timer`]: xref:Bonsai.Reactive.Timer
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Device`]: xref:Harp.TimestampGeneratorGen3.Device
[`Parse (Harp)`]: xref:Bonsai.Harp.Parse
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow

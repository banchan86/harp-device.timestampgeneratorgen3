## Generate Timer Events

The Timestamp Generator Gen3 can broadcast a periodic counter event using the [`Timer`] register. Each event carries an incrementing counter value stamped with the device's hardware clock, which is the same clock the Timestamp Generator Gen3 distributes to every connected device. The [`Timer`] counter is useful for inspecting the device's clock, detecting dropped messages, and [timestamping non-Harp events](https://harp-tech.org/articles/message-manipulation.html#timestamping-generic-data).

This article covers how to enable the counter, observe the events in Bonsai, set the counter value, and set the hardware clock that stamps the events.

The complete workflow is shown below:

:::workflow
![Generate Timer Events Top Level](../workflows/generatetimerevents-toplevel.bonsai)
:::

### Enable Timer Events

The counter is disabled by default, so the [`TimerFrequency`] register must be enabled before any events are produced.

:::workflow
![Enable Timer Events](../workflows/generatetimerevents-frequency.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `TimerFrequency`.
    - `TimerFrequency` - Set the desired frequency (e.g  `Timer50Hz` to generate 50 events per second).
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands` to send the message to the device.

### Observe Timer Events

Each `Timer` event carries the current counter value and the hardware timestamp at which it was generated.

:::workflow
![Observe Timer Events](../workflows/generatetimerevents-events.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`. This will listen to messages broadcast from the [`PublishSubject`] named `TimestampGeneratorGen3 Events` in the Harp device pattern.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedTimer`.
- Insert a [`VisualizerWindow`] operator. This will automatically open a visualizer displaying the parsed events when the workflow starts.

Run the workflow and press <kbd>A</kbd> to enable the counter. 

The visualizer will start streaming events, similar to the following:

```text
4778@95.563344
4779@95.583344
```

The first number corresponds to the counter, and the second number is the timestamp on the device clock.

### Set Counter Value

Writing to the [`Timer`] register sets the value of the counter. This is useful for resetting the counter to zero at the start of a session.

:::workflow
![Set Counter Value](../workflows/generatetimerevents-counter.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `Timer`.
    - `Timer` - Set to 0 to reset the counter.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>S</kbd>. The counter value in the visualizer window will reset to zero.

### Set Clock Value

Writing to the [`TimestampSeconds`] register sets the device's hardware clock, in whole seconds. Since the Timestamp Generator Gen3 is the clock source for the rig, the new time is also propagated to every connected device.

:::workflow
![Set Clock Value](../workflows/generatetimerevents-clock.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage (Bonsai.Harp)`] operator and configure the following properties:
    - `Payload` - Set to `TimestampSeconds`.
    - `TimestampSeconds` - Set to 0 to reset the clock.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>D</kbd>. The timestamp attached to each event in the visualizer window will restart from zero.

> [!NOTE]
> The `TimestampSeconds` register is one of the core registers shared by all Harp devices and is not available in the device-specific `Harp.TimestampGeneratorGen3` package. This workflow instead uses the [`CreateMessage (Bonsai.Harp)`] operator from the `Bonsai.Harp` base library.

> [!NOTE]
> The hardware clock will also reset if the device is disconnected while the [`BATTERY` switch](connections.md#ports-and-controls) is off.

> [!WARNING]
> Make sure the device clock is [unlocked](connections.md#ports-and-controls) before setting the clock. If the clock is locked, the device will throw an error when it receives the write command.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`Timer`]: xref:Harp.TimestampGeneratorGen3.Timer
[`TimerFrequency`]: xref:Harp.TimestampGeneratorGen3.TimerFrequency
[`CreateMessage`]: xref:Harp.TimestampGeneratorGen3.CreateMessage
[`CreateMessage (Bonsai.Harp)`]: xref:Bonsai.Harp.CreateMessage
[`TimestampSeconds`]: xref:Bonsai.Harp.TimestampSeconds
[`Parse`]: xref:Harp.TimestampGeneratorGen3.Parse
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject

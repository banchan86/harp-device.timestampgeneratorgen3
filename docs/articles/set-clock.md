# Set clock

In generator mode, the Timestamp Generator Gen3 sends the current timestamp on its onboard clock to all connected devices as a synchronization signal, so setting the device clock also sets the clock of every connected device. This is useful for resetting the timestamps at the start of every experiment. Refer to the [connections](./ports-connections.md?tabs=generator#connections) article to set up the device in generator mode with the Harp devices it will synchronize, which we will use for the rest of these examples.

This article covers how to visualize the heartbeat events to read the device clock and how to set the device clock in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Set clock](../workflows/setclock-toplevel.bonsai)
:::

> [!NOTE]
> Reading and setting the device clock is a core functionality shared by all Harp devices. Thus, the operators marked with a `(Harp)` suffix in this article come from the `Bonsai.Harp` base library (installed automatically as a dependency) instead of the device-specific package.

> [!WARNING]
> The internal clock also resets whenever the device restarts, including after switching between generator and repeater mode with the [OPTION](ports-connections.md#front-panel) button, and if the device is powered off (if it is disconnected from USB while the [BATTERY](ports-connections.md#front-panel) switch is off or if the battery is depleted).

## Visualize heartbeat events

By default, the Timestamp Generator Gen3 does not report the timestamps it generates. Enabling the `Heartbeat` property of the [`Device`] operator makes the device report a [`TimestampSeconds`] event every second, mirroring the synchronization signal that is sent to the connected devices.

:::workflow
![Enable heartbeat events](../workflows/setclock-heartbeat.bonsai)
:::

- Click on the [`Device`] operator in the Harp device pattern and set the `Heartbeat` property to `Enabled`.

The workflow below will show you how to decode and visualize the heartbeat events:

:::workflow
![Visualize heartbeat events](../workflows/setclock-visualizeheartbeat.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `TimestampGeneratorGen3 Events`.
- Insert a [`Parse (Harp)`] operator and set the `Register` property to `TimestampSeconds`.
- Insert a [`VisualizerWindow`] operator.

Run the workflow. The visualizer will display the timestamp on the device clock every second, in whole seconds:

```text
345
346
```

## Set clock with KeyDown

Writing a value to [`TimestampSeconds`] sets the device clock, in whole seconds.

:::workflow
![Set clock with KeyDown](../workflows/setclock-clockvalue.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage (Harp)`] operator and configure the following properties:
    - `Payload` - Select `TimestampSecondsPayload`.
    - `TimestampSeconds` - Enter `0` to reset the clock.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>A</kbd> to set the clock. The timestamp in the visualizer window restarts from zero.

> [!WARNING]
> Make sure the device clock is [unlocked](ports-connections.md#front-panel) before setting the clock. If the clock is locked, the device will throw an error:
>```text
>Runtime error
>
>The device reported an erroneous write command.
>Payload: 0, Address: 8, Type: U32.
>```

## Alternative: set clock with Timer

You can replace [`KeyDown`] with other operators in Bonsai, for instance, a [`Timer (Reactive)`] operator that sets the clock after the Bonsai workflow has been running for a certain duration. 

:::workflow
![Alternative: set clock with Timer](../workflows/setclock-timer.bonsai)
:::

- Insert a [`Timer (Reactive)`] operator and configure the following properties:
    - `DueTime` - Set the number of seconds to wait after the workflow starts to set the clock (e.g. `00:00:02` for two seconds).
    - `Period` - Leave this unspecified so that the [`Timer (Reactive)`] only triggers once.
- Insert a [`CreateMessage (Harp)`] operator and configure the following properties:
    - `Payload` - Select `TimestampSecondsPayload`.
    - `TimestampSeconds` - Enter `0` to reset the clock.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow. The clock is set 2 seconds after the workflow starts.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`TimestampSeconds`]: xref:Bonsai.Harp.TimestampSeconds
[`CreateMessage (Harp)`]: xref:Bonsai.Harp.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`Timer (Reactive)`]: xref:Bonsai.Reactive.Timer
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Device`]: xref:Harp.TimestampGeneratorGen3.Device
[`Parse (Harp)`]: xref:Bonsai.Harp.Parse
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow

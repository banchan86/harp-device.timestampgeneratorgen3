## Manage Battery Charging

The Timestamp Generator Gen3 manages its internal battery automatically when connected to power.

This article covers how to adjust the default charging thresholds, control charging manually and start a battery charge/discharge cycle to extend battery life.

The complete workflow is shown below:

:::workflow
![Manage Battery Charging Top Level](../workflows/managebatterycharging-toplevel.bonsai)
:::

> [!NOTE]
> To verify that these commands are working correctly, you can [monitor the battery voltage](monitor-device-status.md).

> [!WARNING]
> Battery charging requires external power and the [BATTERY](connections.md#front-panel) switch turned on. When the device is running from its battery, or when no battery is detected, the firmware cancels any active charge or discharge command and resumes normal function.

### Set Charging Thresholds

The [`BatteryThresholdLow`] and [`BatteryThresholdHigh`] registers set the voltages (in volts) at which automatic charging starts and stops. The defaults are 3.65 V and 3.75 V respectively.

:::workflow
![Set Charging Thresholds](../workflows/managebatterycharging-thresholds.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `BatteryThresholdLow`.
    - `BatteryThresholdLow` - Set the voltage below which charging starts (e.g. 3.55).
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.
- Insert a second [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `BatteryThresholdHigh`.
    - `BatteryThresholdHigh` - Set the voltage above which charging stops (e.g. 3.85).
- Insert a second [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>A</kbd> to write both thresholds. You can confirm the values by checking the [battery charge or battery rate](monitor-device-status.md) events.

> [!NOTE]
> The firmware enforces hard safety limits regardless of the configured thresholds: charging always starts below 3.35 V and always stops above 4.1 V. Keep the automatic charging thresholds near the middle of this charge range to extend battery life.

### Control Charging Manually

To temporarily override the automatic charging thresholds and control charging manually, you can set the battery mode using the [`Config`] register. This can be useful, for instance, if you need to fully charge the battery ahead of a planned power outage.

:::workflow
![Control Charging Manually](../workflows/managebatterycharging-config.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `Config`.
    - `Config` - Set to `StartCharge` to start charging right away.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `Config`.
    - `Config` - Set to `StartDischarge` to start discharging right away.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>S</kbd> or <kbd>D</kbd> to charge or discharge the battery.

> [!NOTE]
> A manual charge or discharge will run until it reaches the battery safety limits and return to automatic control when those limits are reached.

### Run Battery Cycle

Running a full battery charge/discharge cycle occasionally will help to extend the battery life and can be activated using the [`Config`] register.

:::workflow
![Run Battery Cycle](../workflows/managebatterycharging-cycle.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `F`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `Config`.
    - `Config` - Set to `StartBatteryCycle`.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `G`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Set to `Config`.
    - `Config` - Set to `Stop` to cancel the cycle.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>F</kbd> to start the cycle. Press <kbd>G</kbd> to cancel the cycle at any time and resume automatic control. The `Stop` command can also be used to stop the manual charge/discharge commands in the previous section.

> [!TIP]
> A battery cycle can also be started from the device itself by holding the [OPTION](connections.md#front-panel) button.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`Config`]: xref:Harp.TimestampGeneratorGen3.Config
[`BatteryThresholdLow`]: xref:Harp.TimestampGeneratorGen3.BatteryThresholdLow
[`BatteryThresholdHigh`]: xref:Harp.TimestampGeneratorGen3.BatteryThresholdHigh
[`CreateMessage`]: xref:Harp.TimestampGeneratorGen3.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject

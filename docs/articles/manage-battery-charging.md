# Manage battery charging

The Timestamp Generator Gen3 manages its internal battery automatically when connected to external power.

This article covers how to adjust the charging thresholds, control charging manually, and run a battery charge/discharge cycle to extend battery life in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Manage battery charging](../workflows/managebatterycharging-toplevel.bonsai)
:::

> [!NOTE]
> To verify that these commands are working correctly, you can [monitor the battery voltage](monitor-device-status.md).

> [!WARNING]
> Battery charging requires external power and the [BATTERY](ports-connections.md#front-panel) switch turned on. When the device is running from its battery, or when no battery is detected, the firmware cancels any active charge or discharge command and resumes normal function.

## Set charging thresholds

The [`BatteryThresholdLow`] and [`BatteryThresholdHigh`] registers set the voltages (in volts) at which automatic charging starts and stops. The defaults are 3.65 V and 3.75 V respectively.

:::workflow
![Set charging thresholds](../workflows/managebatterycharging-thresholds.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `BatteryThresholdLowPayload`.
    - `BatteryThresholdLow` - Enter the voltage below which charging starts (e.g. `3.55`).
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.
- Insert a second [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `BatteryThresholdHighPayload`.
    - `BatteryThresholdHigh` - Enter the voltage above which charging stops (e.g. `3.85`).
- Insert a second [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>A</kbd> to write both thresholds. You can confirm the values by checking the [battery charge or battery rate](monitor-device-status.md) events.

> [!NOTE]
> The firmware enforces hard safety limits regardless of the configured thresholds: charging always starts below 3.35 V and always stops above 4.1 V. Keep the automatic charging thresholds near the middle of this range to extend battery life.

## Control charging manually

To temporarily override the automatic charging thresholds and control charging manually, you can set the battery mode using the [`Config`] register. This can be useful, for instance, if you need to fully charge the battery ahead of a planned power outage.

:::workflow
![Control charging manually](../workflows/managebatterycharging-config.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `ConfigPayload`.
    - `Config` - Select `StartCharge` to start charging right away.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `ConfigPayload`.
    - `Config` - Select `StartDischarge` to start discharging right away.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>S</kbd> or <kbd>D</kbd> to charge or discharge the battery.

> [!NOTE]
> A manual charge or discharge runs until it reaches the battery safety limits, and then returns to automatic control.

## Run battery cycle

Running a full battery charge/discharge cycle occasionally helps to extend the battery life. The cycle is also started with the [`Config`] register.

:::workflow
![Run battery cycle](../workflows/managebatterycharging-cycle.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `F`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `ConfigPayload`.
    - `Config` - Select `StartBatteryCycle`.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `G`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `ConfigPayload`.
    - `Config` - Select `Stop` to cancel the cycle.
- Insert a [`MulticastSubject`] operator named `TimestampGeneratorGen3 Commands`.

Run the workflow and press <kbd>F</kbd> to start the cycle. Press <kbd>G</kbd> to cancel the cycle at any time and resume automatic control. The `Stop` command also stops the manual charge and discharge commands from the previous section.

> [!TIP]
> A battery cycle can also be started from the device itself by holding the [OPTION](ports-connections.md#front-panel) button.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`Config`]: xref:Harp.TimestampGeneratorGen3.Config
[`BatteryThresholdLow`]: xref:Harp.TimestampGeneratorGen3.BatteryThresholdLow
[`BatteryThresholdHigh`]: xref:Harp.TimestampGeneratorGen3.BatteryThresholdHigh
[`CreateMessage`]: xref:Harp.TimestampGeneratorGen3.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject

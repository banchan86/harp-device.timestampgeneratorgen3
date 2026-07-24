---
uid: Harp.TimestampGeneratorGen3.Device
---

Use the [Harp device pattern](https://harp-tech.org/articles/operators.html#device-pattern) to initialize the device, log data, and send commands to and receive messages from the Timestamp Generator Gen3.

:::workflow
![Harp Device Pattern](../workflows/harp-devicepattern.bonsai)
:::

Check out the following in-depth guides to learn how to access the device functionality with the `Harp.TimestampGeneratorGen3` package:

- [Set Clock](../articles/set-clock.md)
- [Generate Timer Events](../articles/generate-timer-events.md)
- [Monitor Device Status](../articles/monitor-device-status.md)
- [Manage Battery Charging](../articles/manage-battery-charging.md)

Refer to the register table below for a complete listing of the available registers on the device.

<table>
  <thead>
    <tr><th colspan="2">TimestampGeneratorGen3</th></tr>
  </thead>
  <tbody>
    <tr><td>whoAmI</td><td>1158</td></tr>
    <tr><td>firmwareVersion</td><td>1.3</td></tr>
    <tr><td>hardwareTargets</td><td>1.2</td></tr>
  </tbody>
</table>

### Registers

| name | address | type | length | access | description | range | interfaceType |
|-|-|-|-|-|-|-|-|
| [Config](xref:Harp.TimestampGeneratorGen3.Config) | 32 | U8 |  | Write | Specifies the device configuration |  | [ConfigurationFlags](xref:Harp.TimestampGeneratorGen3.ConfigurationFlags) |
| [DevicesConnected](xref:Harp.TimestampGeneratorGen3.DevicesConnected) | 33 | U8 |  | Read | Specifies which CLK_OUT ports have devices connected. |  | [ConnectedDevices](xref:Harp.TimestampGeneratorGen3.ConnectedDevices) |
| [RepeaterStatus](xref:Harp.TimestampGeneratorGen3.RepeaterStatus) | 34 | U8 |  | Write | Check whether device is a repeater or spreading internal timestamp |  | [RepeaterFlags](xref:Harp.TimestampGeneratorGen3.RepeaterFlags) |
| [BatteryRate](xref:Harp.TimestampGeneratorGen3.BatteryRate) | 35 | U8 |  | Write | Configure how often the battery value is sent to computer |  | [BatteryRateConfiguration](xref:Harp.TimestampGeneratorGen3.BatteryRateConfiguration) |
| [Battery](xref:Harp.TimestampGeneratorGen3.Battery) | 36 | Float |  | Event | Reads the current battery charge |  | |
| [BatteryThresholdLow](xref:Harp.TimestampGeneratorGen3.BatteryThresholdLow) | 37 | Float |  | Write | Specifies the low threshold from where the battery should start to be charged |  | |
| [BatteryThresholdHigh](xref:Harp.TimestampGeneratorGen3.BatteryThresholdHigh) | 38 | Float |  | Write | Specifies the high threshold from where the battery stops being charged |  | |
| [BatteryCalibration0](xref:Harp.TimestampGeneratorGen3.BatteryCalibration0) | 39 | U16 |  | Write |  |  | |
| [BatteryCalibration1](xref:Harp.TimestampGeneratorGen3.BatteryCalibration1) | 40 | U16 |  | Write |  |  | |
| [Timer](xref:Harp.TimestampGeneratorGen3.Timer) | 41 | U32 |  | Write, Event | Unitary counter that periodically produces a value at the frequency defined by register TimerFrequency. |  | |
| [TimerFrequency](xref:Harp.TimestampGeneratorGen3.TimerFrequency) | 42 | U8 |  | Write | Frequency at which the Timer will generate new values. |  | [TimerRate](xref:Harp.TimestampGeneratorGen3.TimerRate) |

## Indicators and Errors

This article covers how to read the Timestamp Generator Gen3's status indicators and how to resolve common connection errors.

### Indicator Lights

![Harp Timestamp Generator Gen3 front panel](../images/front-panel.svg)

At power-up the device runs a short LED animation (each LED lights in sequence twice, then all LEDs blink twice) while the hardware initializes. During normal operation:

**OUT0–OUT5** - Lit when a device is connected to the corresponding clock output port. 

**REPEAT** - Lit when the device is operating as a clock [repeater](connections.md#repeater-mode); off when it is the clock generator.

**LOCK** - Lit when the device clock is locked.

**STATE** - The LED cycles on and off with a period of:

- 2 seconds when it's communicating with Bonsai
- 4 seconds when in standby
- 100 milliseconds when a catastrophic error occurs 

If the device has a low battery, the **STATE** LED will also flash rapidly (5 Hz for 2 seconds) after the startup sequence.

**ON** - Lit when the device is running from external (USB) power. Off when running from the battery. If the battery becomes depleted (≤ 3.25 V) with no external power, the `ON` LED blinks for a couple of seconds and the device turns itself off to protect the battery.

> [!NOTE]
> When running from the battery, some of the indicator LEDs will only flash briefly every few seconds to save power — the panel appearing mostly dark is normal in battery operation.


### COM Port Errors

**Q: In Bonsai, running the workflow throws an error "The port `ComX` does not exist."**

A: Either the wrong communications port in the `PortName` property in [`Device`] was selected, or the [USB](connections.md) cable is not properly connected. Try selecting a different communications port and checking the connection.

**Q: In Bonsai, running the workflow throws an error "Access to the port `ComX` is denied"**

A: Only one interface connection to the Timestamp Generator Gen3 can be opened at one time. Check that multiple instances of Bonsai are not running. Sometimes, the port can also be locked by a program that did not terminate correctly; restarting the computer fixes it.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`Device`]: xref:Harp.TimestampGeneratorGen3.Device

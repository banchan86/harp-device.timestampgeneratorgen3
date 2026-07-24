## Ports and Connections

This article will cover the ports and controls on the Timestamp Generator Gen3, as well as how to connect the device to a computer and to the Harp devices it will synchronize.

### Ports and Controls

![Harp Timestamp Generator Gen3 front panel](../images/front-panel.svg)

**EXPANSION** - Connector reserved for future expansion modules.

**OPTION** - Multi-function button. The executed action depends on how long the button is held and released, following the legend printed at the bottom. The indicator LEDs will also blink to let you know which option is being selected.

| Hold time | Action | Indicator LED |
| --- | --- | --- |
| ~ 1 s | Check the battery level (requires the **BATTERY** switch to be `ON`) | The battery level will be shown on the `OUT0`–`OUT2` LEDs for a few seconds (1 LED ≈ low, 3 LEDs ≈ well charged). <br><br> If the **BATTERY** switch is off it will flash low as the battery is disconnected. |
| ~ 3 s | Lock or unlock the device clock. | `LOCK` LED will blink and turn ON if the device is locked or turn OFF if it is unlocked.|
| ~ 6 s | Toggle between clock [generator and repeater](#connections) mode. The device will reboot into the new mode after the button is released. | The startup LED sequence will play after the reboot, then the `REPEAT` LED will toggle on or off |
| > 12 s | Start or stop a [battery maintenance cycle](manage-battery-charging.md#control-charging-manually). | `OUT` LEDs blink together (only with external power connected). |

**BATTERY** - Connects or disconnects the internal battery. With the switch `ON`, the device keeps the clock running when external power is removed. The switch also needs to be on for [charging](./manage-battery-charging.md), or to check the [battery levels](./monitor-device-status.md).

#### Status Indicators

At power-up the device runs a short LED animation (each LED lights in sequence twice, then all LEDs blink twice) while the hardware initializes. During normal operation:

**OUT0–OUT5** - On when a device is connected to the corresponding clock output port. 

**REPEAT** - On when the device is operating as a clock [repeater](connections.md#repeater-mode); off when it is the clock generator.

**LOCK** - On when the device clock is locked.

**STATE** - The LED cycles on and off with a period of:

- 2 seconds when it's communicating with Bonsai
- 4 seconds when in standby
- 100 milliseconds when a catastrophic error occurs 

If the device has a low battery, the **STATE** LED will also flash rapidly (5 Hz for 2 seconds) after the startup sequence.

**ON** - On when the device is running from external (USB) power. Off when running from the battery. If the battery becomes depleted (≤ 3.25 V) with no external power, the `ON` LED blinks for a couple of seconds and the device turns itself off to protect the battery.

> [!NOTE]
> When running from the battery, some of the indicator LEDs will only flash briefly every few seconds to save power — the panel appearing mostly dark is normal in battery operation.

![Harp Timestamp Generator Gen3 back panel](../images/back-panel.svg)

**USB (Mini-B/USB-C)** - This port connects the device to the computer for communication and also provides the external power used to run the device and charge its internal battery.

**CLKIN (Stereo Jack)** - When the device is configured as a [repeater](#repeater-mode), it receives the Harp clock from an upstream Timestamp Generator Gen3 on this port instead of generating its own.

**CLKOUT (0-6, Stereo Jack)** - These ports distribute the Harp synchronization clock to up to 6 Harp devices. Connect each port to the clock input (`CLKIN`) port of a Harp device. The device automatically detects which ports have a device connected, reported by the corresponding `OUT` LED on the front panel.

### Connections

The Timestamp Generator Gen3 can be used in either one of two modes (generator or repeater) depending on the number of timestamp generators and connected devices.

#### Generator Mode

![Harp Timestamp Generator Gen3 Generator Mode](../images/generator-mode.svg){width=600}

For synchronization of up to 6 Harp devices:
1) Connect the Timestamp Generator Gen3 to the computer over USB for communication and power.
2) Set the device to **Generator** mode (this is the default setting).
3) Connect each of its clock output ports to the clock input port of a Harp device.

#### Repeater Mode

![Harp Timestamp Generator Gen3 Repeater Mode](../images/repeater-mode.svg){width=600}

For synchronization of more than 6 Harp devices:: 
1) Connect all Timestamp Generator Gen3 units to the computer over USB for communication and power.
2) Setup one Timestamp Generator Gen3 in **Generator** mode.
3) Connect the clock output of the generator unit to the clock input of the other Timestamp Generator Gen3 units.
4) Switch the remaining Timestamp Generator Gen3 units to **Repeater** mode.
5) Connect each of the clock output ports to the clock input port of a Harp device.

> [!NOTE]
> The Timestamp Generator Gen3 is also compatible with other Harp timestamp generators and clock synchronizers.

[!INCLUDE [](version-footer.md)]

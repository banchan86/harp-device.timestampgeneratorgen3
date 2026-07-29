## Timestamp Generator Gen3

![Harp Timestamp Generator Gen3](../images/OEPS-TimestampGen.png){width=450}

### Key Features

- Distributes the Harp synchronization clock to up to 6 Harp devices.
- Option to operate as a clock **generator** (the default) or as a **repeater** that re-distributes an incoming clock, allowing several units to be daisy-chained in larger setups.
- Internal battery to prevent clock interruption during power outages.
- Programmable clock start time with lock to avoid resetting the clock by mistake.
- LED indicators for connected devices, battery life and device status.

### Specs

- Clock inputs: 1
- Clock outputs: 6
- Timestamp resolution: 32 µs
- Synchronization accuracy: 22 +/- 16 µs
- Synchronization frequency: 1 Hz
- Battery capacity: 2 Ah (lithium polymer)
- Battery runtime: up to 24h
- Battery default charging thresholds: 3.65 V to 3.75 V ([configurable](manage-battery-charging.md))
- Battery charging safety limits: 3.35 V to 4.1 V

### Hardware

| Version | Notes |
| ------- | ----- |
| 1.3 | <ul><li> USB Mini-B connector changed to USB-C </li></ul> |
| 1.2 | <ul><li> Production release </li></ul> |

### Firmware

| Version | Notes |
| ------- | ----- |
| 1.3 |  <ul><li> Raise harp core to 1.15 </li></ul> |
| 1.2 | <ul><li> Added unitary periodic counter </li><li> Implement and expose connected devices register in device.yml </li></ul> |
| 1.1 | <ul><li> Add prototype device schema and interface </li><li> Fixed the charge/discharge stop command </li><li> Update interface to use new generators </li><li> Raise harp core to 1.13 </li></ul> |
| 1.0 | <ul><li> Production release </li></ul> |

[!INCLUDE [](version-footer.md)]

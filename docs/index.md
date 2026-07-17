## Overview

The Harp [Timestamp Generator Gen3](articles/timestampgeneratorgen3-overview.md) is a clock synchronization device for the [Harp ecosystem](https://harp-tech.org/articles/about.html).

![Harp Timestamp Generator Gen3 generator mode](./images/generator-mode.svg){width=600}

In a typical behavioral rig, each Harp device timestamps its own data using an onboard clock. The Timestamp Generator Gen3 generates and distributes a shared hardware clock to every connected device, so that data streams acquired across the whole rig land on a common timeline without any post-hoc alignment.

The Timestamp Generator Gen3 provides:

- Distribution of the Harp synchronization clock to up to 6 Harp devices.
- Operation as either a clock generator or a repeater, allowing several units to be daisy-chained in larger setups.
- An internal battery that keeps the clock running when external power is lost or removed.
- [Bonsai](https://bonsai-rx.org/) integration for flexible experiment acquisition and control.

> [!NOTE]
> To consider: Worth adding a note on the previous generations and mention the other timestamp generators with links?

## Getting a Device

Assembled units are available from the [Open Ephys store](https://open-ephys.org/harp), or build your own using the hardware design files in the [Timestamp Generator Gen3](https://github.com/harp-tech/device.timestampgeneratorgen3) repository.

## Acknowledgments

Hardware design contributed by [Champalimaud Foundation](https://www.cf-hw.org/), Bonsai interface by [NeuroGEARS](https://neurogears.org/), and documentation by [Open Ephys](https://open-ephys.org/).

[!INCLUDE [](./articles/version-footer.md)]

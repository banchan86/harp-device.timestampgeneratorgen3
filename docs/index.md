## Overview

The Harp [Timestamp Generator Gen3](articles/timestampgeneratorgen3-overview.md) is a clock synchronization device for the [Harp ecosystem](https://harp-tech.org/articles/about.html).

![Harp Timestamp Generator Gen3 generator mode](./images/generator-mode.svg){width=600}

Behavioral rigs often include multiple Harp devices that acquire data simultaneously. Since each Harp device timestamps data using an onboard clock, their individual clocks must be synchronized. The Timestamp Generator Gen3 uses its onboard clock to generate and distribute the current Harp timestamp to every connected Harp device, allowing them to continuously synchronize their onboard clocks. As a result, data streams acquired across the entire rig are timestamped on a common timeline in real time and do not require any post-hoc alignment.

The Timestamp Generator Gen3 provides:

- Synchronize up to six Harp devices from a single Timestamp Generator Gen3, or expand to larger systems by daisy-chaining additional Timestamp Generator Gen3 devices as repeaters.
- Harp devices connected to the same Timestamp Generator Gen3 remain synchronized even when they are connected to different acquisition computers, enabling distributed experimental setups while maintaining a common timeline.
- An internal battery keeps the clock running when external power is lost, allowing acquisition to resume with the correct Harp timestamp once power is restored, so data acquired before and after the interruption remain on the same timeline. This is particularly useful for long-term experiments spanning multiple days.
- [Bonsai](https://bonsai-rx.org/) integration for flexible experiment acquisition and control.

> [!NOTE]
> To consider: mention and link out to other timestamp generators/clock synchronizers?

## Getting a Device

Assembled units are available from the [Open Ephys store](https://open-ephys.org/harp), or build your own using the hardware design files in the [Timestamp Generator Gen3](https://github.com/harp-tech/device.timestampgeneratorgen3) repository.

## Acknowledgments

Hardware design contributed by [Champalimaud Foundation](https://www.cf-hw.org/), Bonsai interface by [NeuroGEARS](https://neurogears.org/), and documentation by [Open Ephys](https://open-ephys.org/).

[!INCLUDE [](./articles/version-footer.md)]

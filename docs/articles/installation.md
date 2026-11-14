# Installation

This page covers the software you'll need to interact with the Timestamp Generator Gen3, as well as how to update the firmware on the device.

## Software packages

These steps are only required the first time you connect the device to a new computer, and you can install just the packages for the functionality you need.

# [Bonsai](#tab/bonsai)

[Bonsai](https://bonsai-rx.org/) is a visual reactive programming language that provides flexible and comprehensive control of the Timestamp Generator Gen3.

![Bonsai package manager](../images/installation-bonsaipackage.jpg){width=650}

- Download and install [Bonsai](https://bonsai-rx.org/docs/articles/installation.html).
- Launch Bonsai and install the `Harp.TimestampGeneratorGen3` package by searching for it in the [Bonsai package manager](https://bonsai-rx.org/docs/articles/packages.html).
- Install the `Bonsai.StarterPack` package to follow along with the examples in this user guide.

# [Python](#tab/python)

The [Harp](https://harp-tech.org/python/) library provides a Python interface for controlling Harp devices and [loading](logging-analysis.md) recorded data. To install the full library, install it in a Python environment with:

```cmd
pip install harp
```

To install the interface for the Timestamp Generator Gen3:

```cmd
pip install harp-timestampgeneratorgen3
```

To install only the package necessary for loading data:

```cmd
pip install harp-data
```

> [!NOTE]
> Substitute `uv add` for `pip install` if you are using the [uv](https://docs.astral.sh/uv/) package manager.

***

## Firmware update

New features are added and bugs are fixed with firmware updates which are published on the [release page](https://github.com/harp-tech/device.timestampgeneratorgen3/releases) in the device GitHub repository. Each release contains a `.hex` file tagged with the firmware (`fw`) and hardware (`hw`) version. For instance, `TimestampGeneratorGen3-fw1.3-harp1.15-hw1.2-ass0.hex` is firmware v1.3 for a hardware v1.2 Timestamp Generator Gen3. Download the latest firmware version that matches the [hardware version](timestampgeneratorgen3.md#hardware) of the board from the "Assets" section under each release.

> [!TIP]
> The hardware version is printed on the front panel of the device (e.g. `harp timestamp generator Gen3 v1.2`).

To update the firmware, use the device setup tool in Bonsai:

![Device setup tool showing the bootloader firmware update](../images/installation-firmwareupdate.jpg){width=650}

1. Add the [`Device`] operator in Bonsai.
2. Double-click on the [`Device`] node while the workflow is not running.
3. Select the COM port for the device.
4. Click "Bootloader".
5. Click "Open".
6. Select the downloaded `.hex` file.
7. Click "Update".

After the update, the device will reboot with the new firmware and go through the [startup LED sequence](ports-connections.md#front-panel).

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`Device`]: xref:Harp.TimestampGeneratorGen3.Device

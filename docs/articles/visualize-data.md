## Analysis

Data from the device is logged by the [`DeviceDataWriter`] operator in the Harp device pattern, which saves raw data from all device registers in the Harp binary format to the file set in its `Path` property:

:::workflow
![Harp Device Pattern](../workflows/harp-devicepattern.bonsai)
:::

> [!NOTE]
> This article assumes basic familiarity with [Python](https://www.python.org/). It requires a Python environment with [harp-python](installation.md#software-packages) and [`matplotlib`](https://matplotlib.org/) installed.

The `harp-python` library imports data stored in the Harp binary format as [pandas](https://pandas.pydata.org/) DataFrames, which can then be analyzed with any `pandas` compatible plotting or analysis library.

The following example demonstrates how to read and plot the [`Timer`](generate-timer-events.md) counter data logged by the [`DeviceDataWriter`].

```python
# Import harp-python library
import harp

# Create a device object with harp reader
device = harp.create_reader("./TimestampGeneratorGen3.harp")

# Read data from a specific register
timer_df = device.Timer.read()

# Inspect DataFrame
print(timer_df.head())

# Plot the counter value against the hardware timestamp;
# a straight line with no jumps means no events were dropped
timer_df.plot()
```

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`DeviceDataWriter`]: xref:Harp.TimestampGeneratorGen3.DeviceDataWriter

## Logging and Analysis

This article covers how data from the device is logged to disk, and how to read and plot the logged data with `harp-python`.

### Log Data

Data from the device is logged by the [`DeviceDataWriter`] operator in the Harp device pattern, which saves raw data from all device registers in the Harp binary format to the folder set in its `Path` property:

:::workflow
![Harp Device Pattern](../workflows/harp-devicepattern.bonsai)
:::

### Analyze Data

The `harp-python` library imports data stored in the Harp binary format as [pandas](https://pandas.pydata.org/) DataFrames, which can then be analyzed with any `pandas` compatible plotting or analysis library.

The following example demonstrates how to read and plot the [`Timer`](generate-timer-events.md) counter data.

> [!NOTE]
> This example requires a Python environment with [harp-python](installation.md#software-packages) and [`matplotlib`](https://matplotlib.org/) installed.

```python
# Import the harp-python and matplotlib libraries
import harp
import matplotlib.pyplot as plt

# Create a device object with harp reader, pointing to the
# folder set in the DeviceDataWriter Path property
device = harp.create_reader("./Data/TimestampGeneratorGen3.harp")

# Read data from a specific register
timer_df = device.Timer.read()

# Inspect DataFrame
print(timer_df.head())

# Plot the counter value against the hardware timestamp;
# a straight line with no jumps means no events were dropped
timer_df.plot()

# Display the plot (not required in a Jupyter notebook)
plt.show()
```

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`DeviceDataWriter`]: xref:Harp.TimestampGeneratorGen3.DeviceDataWriter

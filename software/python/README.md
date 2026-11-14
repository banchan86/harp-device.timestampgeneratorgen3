# harp-timestampgeneratorgen3

The Python interface for the [Harp Timestamp Generator Gen3](https://harp-tech.org/device.timestampgeneratorgen3) device. It carries the register classes for the device, the payload and enumeration types those registers use, and a `REGISTER_MAP` pairing every address with its register.

## Installation

```text
pip install harp-timestampgeneratorgen3
```

## Reading and writing registers

Register classes are passed to `read` and `write` on an open device transport, such as the serial transport in [Harp Python](https://harp-tech.org/python/).

```python
from harp import serial
from harp.device import client, timestampgeneratorgen3

SERIAL_PORT = "/dev/ttyUSB0"  # or "COMx" in Windows, where "x" is the serial port number

with serial.open_device(client.Device, port=SERIAL_PORT) as device:
    print("device:", timestampgeneratorgen3.DEVICE_NAME, "WhoAmI:", timestampgeneratorgen3.WHO_AM_I)
    print("battery:", device.read(timestampgeneratorgen3.Battery).payload)
```

`open_dataset` in `harp.data` takes this module as its second argument, and decodes recorded data through the same register classes.

## Feedback and contributing

`harp-timestampgeneratorgen3` is released as open source under the MIT license. Bug reports and contributions are welcome at [the GitHub repository](https://github.com/harp-tech/device.timestampgeneratorgen3).

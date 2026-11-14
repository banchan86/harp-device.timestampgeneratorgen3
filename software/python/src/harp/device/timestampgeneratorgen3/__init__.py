# This file was automatically generated and should not be edited directly.
# To make changes, edit the device metadata and regenerate the interface.

import enum
from typing import Any, ClassVar

import numpy as np
from harp.protocol import (
    AnonymousPayload,
    BitMask,
    GroupMask,
    PayloadType,
    RegisterBase,
    RegisterFloat,
    RegisterU16,
    RegisterU32,
)
from harp.device.core import REGISTER_MAP as _CORE_REGISTER_MAP


__all__ = [
    "DEVICE_NAME",
    "WHO_AM_I",
    "ConfigurationFlags",
    "ConnectedDevices",
    "RepeaterFlags",
    "BatteryRateConfiguration",
    "TimerRate",
    "ConfigPayload",
    "DevicesConnectedPayload",
    "BatteryRatePayload",
    "TimerFrequencyPayload",
    "Config",
    "DevicesConnected",
    "BatteryRate",
    "Battery",
    "BatteryThresholdLow",
    "BatteryThresholdHigh",
    "Timer",
    "TimerFrequency",
    "REGISTER_MAP",
]

DEVICE_NAME: str = "TimestampGeneratorGen3"
WHO_AM_I: int = 1158


class ConfigurationFlags(enum.IntFlag):
    """Specifies configuration flags for the device."""

    START_BATTERY_CYCLE = 0x1
    """Starts battery cycle to extend batteries life."""

    START_DISCHARGE = 0x2
    """Starts to discharge right away."""

    START_CHARGE = 0x4
    """Starts to charge right away."""

    STOP = 0x8
    """Stop any control of the battery and resume normal function."""


class ConnectedDevices(enum.IntFlag):
    """Specifies if an output port has a device connected."""

    OUT0 = 0x1
    OUT1 = 0x2
    OUT2 = 0x4
    OUT3 = 0x8
    OUT4 = 0x10
    OUT5 = 0x20


class RepeaterFlags(enum.IntFlag):
    """Specifies whether the device is a clock repeater."""

    REPEATER = 0x1


class BatteryRateConfiguration(enum.IntEnum):
    """Specifies the rate at which the battery charge event is sent."""

    EVERY_MINUTE = 0
    EVERY10_SECONDS = 1
    EVERY_SECOND = 2
    NEVER = 3


class TimerRate(enum.IntEnum):
    """Specifies the rate at which the timer counter is sent."""

    DISABLED = 0
    TIMER50_HZ = 1
    TIMER100_HZ = 2
    TIMER200_HZ = 3
    TIMER500_HZ = 4
    TIMER1000_HZ = 5


class ConfigPayload(AnonymousPayload[np.uint8]):
    """Represents the Config payload."""

    __value__: ConfigurationFlags = BitMask(enum=ConfigurationFlags)


class DevicesConnectedPayload(AnonymousPayload[np.uint8]):
    """Represents the DevicesConnected payload."""

    __value__: ConnectedDevices = BitMask(enum=ConnectedDevices)


class _RepeaterStatusPayload(AnonymousPayload[np.uint8]):
    """Represents the RepeaterStatus payload."""

    __value__: RepeaterFlags = BitMask(enum=RepeaterFlags)


class BatteryRatePayload(AnonymousPayload[np.uint8]):
    """Represents the BatteryRate payload."""

    __value__: BatteryRateConfiguration = GroupMask(enum=BatteryRateConfiguration, mask=0xFF)


class TimerFrequencyPayload(AnonymousPayload[np.uint8]):
    """Represents the TimerFrequency payload."""

    __value__: TimerRate = GroupMask(enum=TimerRate, mask=0xFF)


class Config(RegisterBase[ConfigurationFlags]):
    """Specifies the device configuration"""

    address: ClassVar[int] = 32
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = ConfigPayload


class DevicesConnected(RegisterBase[ConnectedDevices]):
    """Specifies which CLK_OUT ports have devices connected."""

    address: ClassVar[int] = 33
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = DevicesConnectedPayload


class _RepeaterStatus(RegisterBase[RepeaterFlags]):
    """Check whether device is a repeater or spreading internal timestamp"""

    address: ClassVar[int] = 34
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = _RepeaterStatusPayload


class BatteryRate(RegisterBase[BatteryRateConfiguration]):
    """Configure how often the battery value is sent to computer"""

    address: ClassVar[int] = 35
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = BatteryRatePayload


class Battery(RegisterFloat):
    """Reads the current battery charge"""

    address: ClassVar[int] = 36


class BatteryThresholdLow(RegisterFloat):
    """Specifies the low threshold from where the battery should start to be charged"""

    address: ClassVar[int] = 37


class BatteryThresholdHigh(RegisterFloat):
    """Specifies the high threshold from where the battery stops being charged"""

    address: ClassVar[int] = 38


class _BatteryCalibration0(RegisterU16):
    address: ClassVar[int] = 39


class _BatteryCalibration1(RegisterU16):
    address: ClassVar[int] = 40


class Timer(RegisterU32):
    """Unitary counter that periodically produces a value at the frequency defined by register TimerFrequency."""

    address: ClassVar[int] = 41


class TimerFrequency(RegisterBase[TimerRate]):
    """Frequency at which the Timer will generate new values."""

    address: ClassVar[int] = 42
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = TimerFrequencyPayload


REGISTER_MAP: dict[int, type[RegisterBase[Any]]] = {
    **_CORE_REGISTER_MAP,
    32: Config,
    33: DevicesConnected,
    34: _RepeaterStatus,
    35: BatteryRate,
    36: Battery,
    37: BatteryThresholdLow,
    38: BatteryThresholdHigh,
    39: _BatteryCalibration0,
    40: _BatteryCalibration1,
    41: Timer,
    42: TimerFrequency,
}

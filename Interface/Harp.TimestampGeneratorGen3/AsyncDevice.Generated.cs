using Bonsai.Harp;
using System.Threading;
using System.Threading.Tasks;

namespace Harp.TimestampGeneratorGen3
{
    /// <inheritdoc/>
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the asynchronous API to configure and interface
        /// with TimestampGeneratorGen3 devices on the specified serial port.
        /// </summary>
        /// <param name="portName">
        /// The name of the serial port used to communicate with the Harp device.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous initialization operation. The value of
        /// the <see cref="Task{TResult}.Result"/> parameter contains a new instance of
        /// the <see cref="AsyncDevice"/> class.
        /// </returns>
        public static async Task<AsyncDevice> CreateAsync(string portName, CancellationToken cancellationToken = default)
        {
            var device = new AsyncDevice(portName);
            var whoAmI = await device.ReadWhoAmIAsync(cancellationToken);
            if (whoAmI != Device.WhoAmI)
            {
                var errorMessage = string.Format(
                    "The device ID {1} on {0} was unexpected. Check whether a TimestampGeneratorGen3 device is connected to the specified serial port.",
                    portName, whoAmI);
                throw new HarpException(errorMessage);
            }

            return device;
        }
    }

    /// <summary>
    /// Represents an asynchronous API to configure and interface with TimestampGeneratorGen3 devices.
    /// </summary>
    public partial class AsyncDevice : Bonsai.Harp.AsyncDevice
    {
        internal AsyncDevice(string portName)
            : base(portName)
        {
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="Config"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<ConfigurationFlags> ReadConfigAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Config.Address), cancellationToken);
            return Config.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="Config"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ConfigurationFlags>> ReadTimestampedConfigAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Config.Address), cancellationToken);
            return Config.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="Config"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteConfigAsync(ConfigurationFlags value, CancellationToken cancellationToken = default)
        {
            var request = Config.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="DevicesConnected"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<ConnectedDevices> ReadDevicesConnectedAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DevicesConnected.Address), cancellationToken);
            return DevicesConnected.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="DevicesConnected"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ConnectedDevices>> ReadTimestampedDevicesConnectedAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DevicesConnected.Address), cancellationToken);
            return DevicesConnected.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="RepeaterStatus"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<RepeaterFlags> ReadRepeaterStatusAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(RepeaterStatus.Address), cancellationToken);
            return RepeaterStatus.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="RepeaterStatus"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<RepeaterFlags>> ReadTimestampedRepeaterStatusAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(RepeaterStatus.Address), cancellationToken);
            return RepeaterStatus.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="RepeaterStatus"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteRepeaterStatusAsync(RepeaterFlags value, CancellationToken cancellationToken = default)
        {
            var request = RepeaterStatus.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="BatteryRate"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<BatteryRateConfiguration> ReadBatteryRateAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(BatteryRate.Address), cancellationToken);
            return BatteryRate.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="BatteryRate"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<BatteryRateConfiguration>> ReadTimestampedBatteryRateAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(BatteryRate.Address), cancellationToken);
            return BatteryRate.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="BatteryRate"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteBatteryRateAsync(BatteryRateConfiguration value, CancellationToken cancellationToken = default)
        {
            var request = BatteryRate.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="Battery"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<float> ReadBatteryAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Battery.Address), cancellationToken);
            return Battery.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="Battery"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedBatteryAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Battery.Address), cancellationToken);
            return Battery.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="BatteryThresholdLow"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<float> ReadBatteryThresholdLowAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(BatteryThresholdLow.Address), cancellationToken);
            return BatteryThresholdLow.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="BatteryThresholdLow"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedBatteryThresholdLowAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(BatteryThresholdLow.Address), cancellationToken);
            return BatteryThresholdLow.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="BatteryThresholdLow"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteBatteryThresholdLowAsync(float value, CancellationToken cancellationToken = default)
        {
            var request = BatteryThresholdLow.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="BatteryThresholdHigh"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<float> ReadBatteryThresholdHighAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(BatteryThresholdHigh.Address), cancellationToken);
            return BatteryThresholdHigh.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="BatteryThresholdHigh"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedBatteryThresholdHighAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(BatteryThresholdHigh.Address), cancellationToken);
            return BatteryThresholdHigh.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="BatteryThresholdHigh"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteBatteryThresholdHighAsync(float value, CancellationToken cancellationToken = default)
        {
            var request = BatteryThresholdHigh.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="BatteryCalibration0"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<ushort> ReadBatteryCalibration0Async(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(BatteryCalibration0.Address), cancellationToken);
            return BatteryCalibration0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="BatteryCalibration0"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedBatteryCalibration0Async(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(BatteryCalibration0.Address), cancellationToken);
            return BatteryCalibration0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="BatteryCalibration0"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteBatteryCalibration0Async(ushort value, CancellationToken cancellationToken = default)
        {
            var request = BatteryCalibration0.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="BatteryCalibration1"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<ushort> ReadBatteryCalibration1Async(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(BatteryCalibration1.Address), cancellationToken);
            return BatteryCalibration1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="BatteryCalibration1"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedBatteryCalibration1Async(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(BatteryCalibration1.Address), cancellationToken);
            return BatteryCalibration1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="BatteryCalibration1"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteBatteryCalibration1Async(ushort value, CancellationToken cancellationToken = default)
        {
            var request = BatteryCalibration1.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="Timer"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<uint> ReadTimerAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt32(Timer.Address), cancellationToken);
            return Timer.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="Timer"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<uint>> ReadTimestampedTimerAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt32(Timer.Address), cancellationToken);
            return Timer.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="Timer"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteTimerAsync(uint value, CancellationToken cancellationToken = default)
        {
            var request = Timer.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the <see cref="TimerFrequency"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the register payload.
        /// </returns>
        public async Task<TimerRate> ReadTimerFrequencyAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(TimerFrequency.Address), cancellationToken);
            return TimerFrequency.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the <see cref="TimerFrequency"/> register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The task result contains
        /// the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<TimerRate>> ReadTimestampedTimerFrequencyAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(TimerFrequency.Address), cancellationToken);
            return TimerFrequency.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the <see cref="TimerFrequency"/> register.
        /// </summary>
        /// <param name="value">The value to write in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteTimerFrequencyAsync(TimerRate value, CancellationToken cancellationToken = default)
        {
            var request = TimerFrequency.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }
    }
}

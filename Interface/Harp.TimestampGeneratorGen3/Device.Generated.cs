using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.TimestampGeneratorGen3
{
    /// <summary>
    /// Generates events and processes commands for the TimestampGeneratorGen3 device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the TimestampGeneratorGen3 device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="TimestampGeneratorGen3"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 1158;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(TimestampGeneratorGen3);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(Config) },
            { 33, typeof(DevicesConnected) },
            { 34, typeof(RepeaterStatus) },
            { 35, typeof(BatteryRate) },
            { 36, typeof(Battery) },
            { 37, typeof(BatteryThresholdLow) },
            { 38, typeof(BatteryThresholdHigh) },
            { 39, typeof(BatteryCalibration0) },
            { 40, typeof(BatteryCalibration1) },
            { 41, typeof(Timer) },
            { 42, typeof(TimerFrequency) }
        };

        /// <summary>
        /// Gets the contents of the metadata file describing the <see cref="TimestampGeneratorGen3"/>
        /// device registers.
        /// </summary>
        public static readonly string Metadata = GetDeviceMetadata();

        static string GetDeviceMetadata()
        {
            var deviceType = typeof(Device);
            using var metadataStream = deviceType.Assembly.GetManifestResourceStream($"{deviceType.Namespace}.device.yml");
            using var streamReader = new System.IO.StreamReader(metadataStream);
            return streamReader.ReadToEnd();
        }
    }

    /// <summary>
    /// Represents an operator that returns the contents of the metadata file
    /// describing the <see cref="TimestampGeneratorGen3"/> device registers.
    /// </summary>
    [Description("Returns the contents of the metadata file describing the TimestampGeneratorGen3 device registers.")]
    public partial class GetDeviceMetadata : Source<string>
    {
        /// <summary>
        /// Returns an observable sequence with the contents of the metadata file
        /// describing the <see cref="TimestampGeneratorGen3"/> device registers.
        /// </summary>
        /// <returns>
        /// A sequence with a single <see cref="string"/> object representing the
        /// contents of the metadata file.
        /// </returns>
        public override IObservable<string> Generate()
        {
            return Observable.Return(Device.Metadata);
        }
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="TimestampGeneratorGen3"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of TimestampGeneratorGen3 messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="TimestampGeneratorGen3"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="TimestampGeneratorGen3"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that writes the sequence of <see cref="TimestampGeneratorGen3"/>" messages
    /// to the standard Harp storage format.
    /// </summary>
    [DefaultProperty(nameof(Path))]
    [Description("Writes the sequence of TimestampGeneratorGen3 messages to the standard Harp storage format.")]
    public partial class DeviceDataWriter : Sink<HarpMessage>, INamedElement
    {
        const string BinaryExtension = ".bin";
        const string MetadataFileName = "device.yml";
        readonly Bonsai.Harp.MessageWriter writer = new();

        string INamedElement.Name => nameof(TimestampGeneratorGen3) + "DataWriter";

        /// <summary>
        /// Gets or sets the relative or absolute path on which to save the message data.
        /// </summary>
        [Description("The relative or absolute path of the directory on which to save the message data.")]
        [Editor("Bonsai.Design.SaveFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        public string Path
        {
            get => System.IO.Path.GetDirectoryName(writer.FileName);
            set => writer.FileName = System.IO.Path.Combine(value, nameof(TimestampGeneratorGen3) + BinaryExtension);
        }

        /// <summary>
        /// Gets or sets a value indicating whether element writing should be buffered. If <see langword="true"/>,
        /// the write commands will be queued in memory as fast as possible and will be processed
        /// by the writer in a different thread. Otherwise, writing will be done in the same
        /// thread in which notifications arrive.
        /// </summary>
        [Description("Indicates whether writing should be buffered.")]
        public bool Buffered
        {
            get => writer.Buffered;
            set => writer.Buffered = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to overwrite the output file if it already exists.
        /// </summary>
        [Description("Indicates whether to overwrite the output file if it already exists.")]
        public bool Overwrite
        {
            get => writer.Overwrite;
            set => writer.Overwrite = value;
        }

        /// <summary>
        /// Gets or sets a value specifying how the message filter will use the matching criteria.
        /// </summary>
        [Description("Specifies how the message filter will use the matching criteria.")]
        public FilterType FilterType
        {
            get => writer.FilterType;
            set => writer.FilterType = value;
        }

        /// <summary>
        /// Gets or sets a value specifying the expected message type. If no value is
        /// specified, all messages will be accepted.
        /// </summary>
        [Description("Specifies the expected message type. If no value is specified, all messages will be accepted.")]
        public MessageType? MessageType
        {
            get => writer.MessageType;
            set => writer.MessageType = value;
        }

        private IObservable<TSource> WriteDeviceMetadata<TSource>(IObservable<TSource> source)
        {
            var basePath = Path;
            if (string.IsNullOrEmpty(basePath))
                return source;

            var metadataPath = System.IO.Path.Combine(basePath, MetadataFileName);
            return Observable.Create<TSource>(observer =>
            {
                Bonsai.IO.PathHelper.EnsureDirectory(metadataPath);
                if (System.IO.File.Exists(metadataPath) && !Overwrite)
                {
                    throw new System.IO.IOException(string.Format("The file '{0}' already exists.", metadataPath));
                }

                System.IO.File.WriteAllText(metadataPath, Device.Metadata);
                return source.SubscribeSafe(observer);
            });
        }

        /// <summary>
        /// Writes each Harp message in the sequence to the specified binary file, and the
        /// contents of the device metadata file to a separate text file.
        /// </summary>
        /// <param name="source">The sequence of messages to write to the file.</param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the
        /// messages to a raw binary file, and the contents of the device metadata file
        /// to a separate text file.
        /// </returns>
        public override IObservable<HarpMessage> Process(IObservable<HarpMessage> source)
        {
            return source.Publish(ps => ps.Merge(
                WriteDeviceMetadata(writer.Process(ps.GroupBy(message => message.Address)))
                .IgnoreElements()
                .Cast<HarpMessage>()));
        }

        /// <summary>
        /// Writes each Harp message in the sequence of observable groups to the
        /// corresponding binary file, where the name of each file is generated from
        /// the common group register address. The contents of the device metadata file are
        /// written to a separate text file.
        /// </summary>
        /// <param name="source">
        /// A sequence of observable groups, each of which corresponds to a unique register
        /// address.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the Harp
        /// messages in each group to the corresponding file, and the contents of the device
        /// metadata file to a separate text file.
        /// </returns>
        public IObservable<IGroupedObservable<int, HarpMessage>> Process(IObservable<IGroupedObservable<int, HarpMessage>> source)
        {
            return WriteDeviceMetadata(writer.Process(source));
        }

        /// <summary>
        /// Writes each Harp message in the sequence of observable groups to the
        /// corresponding binary file, where the name of each file is generated from
        /// the common group register name. The contents of the device metadata file are
        /// written to a separate text file.
        /// </summary>
        /// <param name="source">
        /// A sequence of observable groups, each of which corresponds to a unique register
        /// type.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the Harp
        /// messages in each group to the corresponding file, and the contents of the device
        /// metadata file to a separate text file.
        /// </returns>
        public IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<IGroupedObservable<Type, HarpMessage>> source)
        {
            return WriteDeviceMetadata(writer.Process(source));
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="TimestampGeneratorGen3"/> device.
    /// </summary>
    /// <seealso cref="Config"/>
    /// <seealso cref="DevicesConnected"/>
    /// <seealso cref="RepeaterStatus"/>
    /// <seealso cref="BatteryRate"/>
    /// <seealso cref="Battery"/>
    /// <seealso cref="BatteryThresholdLow"/>
    /// <seealso cref="BatteryThresholdHigh"/>
    /// <seealso cref="BatteryCalibration0"/>
    /// <seealso cref="BatteryCalibration1"/>
    /// <seealso cref="Timer"/>
    /// <seealso cref="TimerFrequency"/>
    [XmlInclude(typeof(Config))]
    [XmlInclude(typeof(DevicesConnected))]
    [XmlInclude(typeof(RepeaterStatus))]
    [XmlInclude(typeof(BatteryRate))]
    [XmlInclude(typeof(Battery))]
    [XmlInclude(typeof(BatteryThresholdLow))]
    [XmlInclude(typeof(BatteryThresholdHigh))]
    [XmlInclude(typeof(BatteryCalibration0))]
    [XmlInclude(typeof(BatteryCalibration1))]
    [XmlInclude(typeof(Timer))]
    [XmlInclude(typeof(TimerFrequency))]
    [Description("Filters register-specific messages reported by the TimestampGeneratorGen3 device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new Config();
        }

        string INamedElement.Name
        {
            get => $"{nameof(TimestampGeneratorGen3)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the TimestampGeneratorGen3 device.
    /// </summary>
    /// <seealso cref="Config"/>
    /// <seealso cref="DevicesConnected"/>
    /// <seealso cref="RepeaterStatus"/>
    /// <seealso cref="BatteryRate"/>
    /// <seealso cref="Battery"/>
    /// <seealso cref="BatteryThresholdLow"/>
    /// <seealso cref="BatteryThresholdHigh"/>
    /// <seealso cref="BatteryCalibration0"/>
    /// <seealso cref="BatteryCalibration1"/>
    /// <seealso cref="Timer"/>
    /// <seealso cref="TimerFrequency"/>
    [XmlInclude(typeof(Config))]
    [XmlInclude(typeof(DevicesConnected))]
    [XmlInclude(typeof(RepeaterStatus))]
    [XmlInclude(typeof(BatteryRate))]
    [XmlInclude(typeof(Battery))]
    [XmlInclude(typeof(BatteryThresholdLow))]
    [XmlInclude(typeof(BatteryThresholdHigh))]
    [XmlInclude(typeof(BatteryCalibration0))]
    [XmlInclude(typeof(BatteryCalibration1))]
    [XmlInclude(typeof(Timer))]
    [XmlInclude(typeof(TimerFrequency))]
    [XmlInclude(typeof(TimestampedConfig))]
    [XmlInclude(typeof(TimestampedDevicesConnected))]
    [XmlInclude(typeof(TimestampedRepeaterStatus))]
    [XmlInclude(typeof(TimestampedBatteryRate))]
    [XmlInclude(typeof(TimestampedBattery))]
    [XmlInclude(typeof(TimestampedBatteryThresholdLow))]
    [XmlInclude(typeof(TimestampedBatteryThresholdHigh))]
    [XmlInclude(typeof(TimestampedBatteryCalibration0))]
    [XmlInclude(typeof(TimestampedBatteryCalibration1))]
    [XmlInclude(typeof(TimestampedTimer))]
    [XmlInclude(typeof(TimestampedTimerFrequency))]
    [Description("Filters and selects specific messages reported by the TimestampGeneratorGen3 device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new Config();
        }

        string INamedElement.Name => $"{nameof(TimestampGeneratorGen3)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// TimestampGeneratorGen3 register messages.
    /// </summary>
    /// <seealso cref="Config"/>
    /// <seealso cref="DevicesConnected"/>
    /// <seealso cref="RepeaterStatus"/>
    /// <seealso cref="BatteryRate"/>
    /// <seealso cref="Battery"/>
    /// <seealso cref="BatteryThresholdLow"/>
    /// <seealso cref="BatteryThresholdHigh"/>
    /// <seealso cref="BatteryCalibration0"/>
    /// <seealso cref="BatteryCalibration1"/>
    /// <seealso cref="Timer"/>
    /// <seealso cref="TimerFrequency"/>
    [XmlInclude(typeof(Config))]
    [XmlInclude(typeof(DevicesConnected))]
    [XmlInclude(typeof(RepeaterStatus))]
    [XmlInclude(typeof(BatteryRate))]
    [XmlInclude(typeof(Battery))]
    [XmlInclude(typeof(BatteryThresholdLow))]
    [XmlInclude(typeof(BatteryThresholdHigh))]
    [XmlInclude(typeof(BatteryCalibration0))]
    [XmlInclude(typeof(BatteryCalibration1))]
    [XmlInclude(typeof(Timer))]
    [XmlInclude(typeof(TimerFrequency))]
    [Description("Formats a sequence of values as specific TimestampGeneratorGen3 register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new Config();
        }

        string INamedElement.Name => $"{nameof(TimestampGeneratorGen3)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that specifies the device configuration.
    /// </summary>
    [Description("Specifies the device configuration")]
    public partial class Config
    {
        /// <summary>
        /// Represents the address of the <see cref="Config"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="Config"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Config"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Config"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ConfigurationFlags GetPayload(HarpMessage message)
        {
            return (ConfigurationFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Config"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ConfigurationFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ConfigurationFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Config"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Config"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ConfigurationFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Config"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Config"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ConfigurationFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Config register.
    /// </summary>
    /// <seealso cref="Config"/>
    [Description("Filters and selects timestamped messages from the Config register.")]
    public partial class TimestampedConfig
    {
        /// <summary>
        /// Represents the address of the <see cref="Config"/> register. This field is constant.
        /// </summary>
        public const int Address = Config.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Config"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ConfigurationFlags> GetPayload(HarpMessage message)
        {
            return Config.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies which CLK_OUT ports have devices connected.
    /// </summary>
    [Description("Specifies which CLK_OUT ports have devices connected.")]
    public partial class DevicesConnected
    {
        /// <summary>
        /// Represents the address of the <see cref="DevicesConnected"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="DevicesConnected"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DevicesConnected"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DevicesConnected"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ConnectedDevices GetPayload(HarpMessage message)
        {
            return (ConnectedDevices)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DevicesConnected"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ConnectedDevices> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ConnectedDevices)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DevicesConnected"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DevicesConnected"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ConnectedDevices value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DevicesConnected"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DevicesConnected"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ConnectedDevices value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DevicesConnected register.
    /// </summary>
    /// <seealso cref="DevicesConnected"/>
    [Description("Filters and selects timestamped messages from the DevicesConnected register.")]
    public partial class TimestampedDevicesConnected
    {
        /// <summary>
        /// Represents the address of the <see cref="DevicesConnected"/> register. This field is constant.
        /// </summary>
        public const int Address = DevicesConnected.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DevicesConnected"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ConnectedDevices> GetPayload(HarpMessage message)
        {
            return DevicesConnected.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that check whether device is a repeater or spreading internal timestamp.
    /// </summary>
    [Description("Check whether device is a repeater or spreading internal timestamp")]
    public partial class RepeaterStatus
    {
        /// <summary>
        /// Represents the address of the <see cref="RepeaterStatus"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="RepeaterStatus"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="RepeaterStatus"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="RepeaterStatus"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static RepeaterFlags GetPayload(HarpMessage message)
        {
            return (RepeaterFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="RepeaterStatus"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<RepeaterFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((RepeaterFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="RepeaterStatus"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="RepeaterStatus"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, RepeaterFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="RepeaterStatus"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="RepeaterStatus"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, RepeaterFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// RepeaterStatus register.
    /// </summary>
    /// <seealso cref="RepeaterStatus"/>
    [Description("Filters and selects timestamped messages from the RepeaterStatus register.")]
    public partial class TimestampedRepeaterStatus
    {
        /// <summary>
        /// Represents the address of the <see cref="RepeaterStatus"/> register. This field is constant.
        /// </summary>
        public const int Address = RepeaterStatus.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="RepeaterStatus"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<RepeaterFlags> GetPayload(HarpMessage message)
        {
            return RepeaterStatus.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configure how often the battery value is sent to computer.
    /// </summary>
    [Description("Configure how often the battery value is sent to computer")]
    public partial class BatteryRate
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryRate"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="BatteryRate"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="BatteryRate"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="BatteryRate"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static BatteryRateConfiguration GetPayload(HarpMessage message)
        {
            return (BatteryRateConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="BatteryRate"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<BatteryRateConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((BatteryRateConfiguration)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="BatteryRate"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryRate"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, BatteryRateConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="BatteryRate"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryRate"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, BatteryRateConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// BatteryRate register.
    /// </summary>
    /// <seealso cref="BatteryRate"/>
    [Description("Filters and selects timestamped messages from the BatteryRate register.")]
    public partial class TimestampedBatteryRate
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryRate"/> register. This field is constant.
        /// </summary>
        public const int Address = BatteryRate.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="BatteryRate"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<BatteryRateConfiguration> GetPayload(HarpMessage message)
        {
            return BatteryRate.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reads the current battery charge.
    /// </summary>
    [Description("Reads the current battery charge")]
    public partial class Battery
    {
        /// <summary>
        /// Represents the address of the <see cref="Battery"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="Battery"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Battery"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Battery"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Battery"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Battery"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Battery"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Battery"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Battery"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Battery register.
    /// </summary>
    /// <seealso cref="Battery"/>
    [Description("Filters and selects timestamped messages from the Battery register.")]
    public partial class TimestampedBattery
    {
        /// <summary>
        /// Represents the address of the <see cref="Battery"/> register. This field is constant.
        /// </summary>
        public const int Address = Battery.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Battery"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Battery.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the low threshold from where the battery should start to be charged.
    /// </summary>
    [Description("Specifies the low threshold from where the battery should start to be charged")]
    public partial class BatteryThresholdLow
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryThresholdLow"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="BatteryThresholdLow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="BatteryThresholdLow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="BatteryThresholdLow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="BatteryThresholdLow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="BatteryThresholdLow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryThresholdLow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="BatteryThresholdLow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryThresholdLow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// BatteryThresholdLow register.
    /// </summary>
    /// <seealso cref="BatteryThresholdLow"/>
    [Description("Filters and selects timestamped messages from the BatteryThresholdLow register.")]
    public partial class TimestampedBatteryThresholdLow
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryThresholdLow"/> register. This field is constant.
        /// </summary>
        public const int Address = BatteryThresholdLow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="BatteryThresholdLow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return BatteryThresholdLow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the high threshold from where the battery stops being charged.
    /// </summary>
    [Description("Specifies the high threshold from where the battery stops being charged")]
    public partial class BatteryThresholdHigh
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryThresholdHigh"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="BatteryThresholdHigh"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="BatteryThresholdHigh"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="BatteryThresholdHigh"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="BatteryThresholdHigh"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="BatteryThresholdHigh"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryThresholdHigh"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="BatteryThresholdHigh"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryThresholdHigh"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// BatteryThresholdHigh register.
    /// </summary>
    /// <seealso cref="BatteryThresholdHigh"/>
    [Description("Filters and selects timestamped messages from the BatteryThresholdHigh register.")]
    public partial class TimestampedBatteryThresholdHigh
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryThresholdHigh"/> register. This field is constant.
        /// </summary>
        public const int Address = BatteryThresholdHigh.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="BatteryThresholdHigh"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return BatteryThresholdHigh.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that manipulates messages from register BatteryCalibration0.
    /// </summary>
    [Description("")]
    public partial class BatteryCalibration0
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryCalibration0"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="BatteryCalibration0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="BatteryCalibration0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="BatteryCalibration0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="BatteryCalibration0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="BatteryCalibration0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryCalibration0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="BatteryCalibration0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryCalibration0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// BatteryCalibration0 register.
    /// </summary>
    /// <seealso cref="BatteryCalibration0"/>
    [Description("Filters and selects timestamped messages from the BatteryCalibration0 register.")]
    public partial class TimestampedBatteryCalibration0
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryCalibration0"/> register. This field is constant.
        /// </summary>
        public const int Address = BatteryCalibration0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="BatteryCalibration0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return BatteryCalibration0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that manipulates messages from register BatteryCalibration1.
    /// </summary>
    [Description("")]
    public partial class BatteryCalibration1
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryCalibration1"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="BatteryCalibration1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="BatteryCalibration1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="BatteryCalibration1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="BatteryCalibration1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="BatteryCalibration1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryCalibration1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="BatteryCalibration1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="BatteryCalibration1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// BatteryCalibration1 register.
    /// </summary>
    /// <seealso cref="BatteryCalibration1"/>
    [Description("Filters and selects timestamped messages from the BatteryCalibration1 register.")]
    public partial class TimestampedBatteryCalibration1
    {
        /// <summary>
        /// Represents the address of the <see cref="BatteryCalibration1"/> register. This field is constant.
        /// </summary>
        public const int Address = BatteryCalibration1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="BatteryCalibration1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return BatteryCalibration1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.
    /// </summary>
    [Description("Unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.")]
    public partial class Timer
    {
        /// <summary>
        /// Represents the address of the <see cref="Timer"/> register. This field is constant.
        /// </summary>
        public const int Address = 41;

        /// <summary>
        /// Represents the payload type of the <see cref="Timer"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U32;

        /// <summary>
        /// Represents the length of the <see cref="Timer"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Timer"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static uint GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Timer"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<uint> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Timer"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Timer"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, uint value)
        {
            return HarpMessage.FromUInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Timer"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Timer"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, uint value)
        {
            return HarpMessage.FromUInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Timer register.
    /// </summary>
    /// <seealso cref="Timer"/>
    [Description("Filters and selects timestamped messages from the Timer register.")]
    public partial class TimestampedTimer
    {
        /// <summary>
        /// Represents the address of the <see cref="Timer"/> register. This field is constant.
        /// </summary>
        public const int Address = Timer.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Timer"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<uint> GetPayload(HarpMessage message)
        {
            return Timer.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that frequency at which the Timer will generate new values.
    /// </summary>
    [Description("Frequency at which the Timer will generate new values.")]
    public partial class TimerFrequency
    {
        /// <summary>
        /// Represents the address of the <see cref="TimerFrequency"/> register. This field is constant.
        /// </summary>
        public const int Address = 42;

        /// <summary>
        /// Represents the payload type of the <see cref="TimerFrequency"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="TimerFrequency"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="TimerFrequency"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static TimerRate GetPayload(HarpMessage message)
        {
            return (TimerRate)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="TimerFrequency"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<TimerRate> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((TimerRate)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="TimerFrequency"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="TimerFrequency"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, TimerRate value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="TimerFrequency"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="TimerFrequency"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, TimerRate value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// TimerFrequency register.
    /// </summary>
    /// <seealso cref="TimerFrequency"/>
    [Description("Filters and selects timestamped messages from the TimerFrequency register.")]
    public partial class TimestampedTimerFrequency
    {
        /// <summary>
        /// Represents the address of the <see cref="TimerFrequency"/> register. This field is constant.
        /// </summary>
        public const int Address = TimerFrequency.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="TimerFrequency"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<TimerRate> GetPayload(HarpMessage message)
        {
            return TimerFrequency.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// TimestampGeneratorGen3 device.
    /// </summary>
    /// <seealso cref="CreateConfigPayload"/>
    /// <seealso cref="CreateDevicesConnectedPayload"/>
    /// <seealso cref="CreateRepeaterStatusPayload"/>
    /// <seealso cref="CreateBatteryRatePayload"/>
    /// <seealso cref="CreateBatteryPayload"/>
    /// <seealso cref="CreateBatteryThresholdLowPayload"/>
    /// <seealso cref="CreateBatteryThresholdHighPayload"/>
    /// <seealso cref="CreateBatteryCalibration0Payload"/>
    /// <seealso cref="CreateBatteryCalibration1Payload"/>
    /// <seealso cref="CreateTimerPayload"/>
    /// <seealso cref="CreateTimerFrequencyPayload"/>
    [XmlInclude(typeof(CreateConfigPayload))]
    [XmlInclude(typeof(CreateDevicesConnectedPayload))]
    [XmlInclude(typeof(CreateRepeaterStatusPayload))]
    [XmlInclude(typeof(CreateBatteryRatePayload))]
    [XmlInclude(typeof(CreateBatteryPayload))]
    [XmlInclude(typeof(CreateBatteryThresholdLowPayload))]
    [XmlInclude(typeof(CreateBatteryThresholdHighPayload))]
    [XmlInclude(typeof(CreateBatteryCalibration0Payload))]
    [XmlInclude(typeof(CreateBatteryCalibration1Payload))]
    [XmlInclude(typeof(CreateTimerPayload))]
    [XmlInclude(typeof(CreateTimerFrequencyPayload))]
    [XmlInclude(typeof(CreateTimestampedConfigPayload))]
    [XmlInclude(typeof(CreateTimestampedDevicesConnectedPayload))]
    [XmlInclude(typeof(CreateTimestampedRepeaterStatusPayload))]
    [XmlInclude(typeof(CreateTimestampedBatteryRatePayload))]
    [XmlInclude(typeof(CreateTimestampedBatteryPayload))]
    [XmlInclude(typeof(CreateTimestampedBatteryThresholdLowPayload))]
    [XmlInclude(typeof(CreateTimestampedBatteryThresholdHighPayload))]
    [XmlInclude(typeof(CreateTimestampedBatteryCalibration0Payload))]
    [XmlInclude(typeof(CreateTimestampedBatteryCalibration1Payload))]
    [XmlInclude(typeof(CreateTimestampedTimerPayload))]
    [XmlInclude(typeof(CreateTimestampedTimerFrequencyPayload))]
    [Description("Creates standard message payloads for the TimestampGeneratorGen3 device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateConfigPayload();
        }

        string INamedElement.Name => $"{nameof(TimestampGeneratorGen3)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the device configuration.
    /// </summary>
    [DisplayName("ConfigPayload")]
    [Description("Creates a message payload that specifies the device configuration.")]
    public partial class CreateConfigPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the device configuration.
        /// </summary>
        [Description("The value that specifies the device configuration.")]
        public ConfigurationFlags Config { get; set; }

        /// <summary>
        /// Creates a message payload for the Config register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ConfigurationFlags GetPayload()
        {
            return Config;
        }

        /// <summary>
        /// Creates a message that specifies the device configuration.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Config register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.Config.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the device configuration.
    /// </summary>
    [DisplayName("TimestampedConfigPayload")]
    [Description("Creates a timestamped message payload that specifies the device configuration.")]
    public partial class CreateTimestampedConfigPayload : CreateConfigPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the device configuration.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Config register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.Config.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies which CLK_OUT ports have devices connected.
    /// </summary>
    [DisplayName("DevicesConnectedPayload")]
    [Description("Creates a message payload that specifies which CLK_OUT ports have devices connected.")]
    public partial class CreateDevicesConnectedPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies which CLK_OUT ports have devices connected.
        /// </summary>
        [Description("The value that specifies which CLK_OUT ports have devices connected.")]
        public ConnectedDevices DevicesConnected { get; set; }

        /// <summary>
        /// Creates a message payload for the DevicesConnected register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ConnectedDevices GetPayload()
        {
            return DevicesConnected;
        }

        /// <summary>
        /// Creates a message that specifies which CLK_OUT ports have devices connected.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DevicesConnected register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.DevicesConnected.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies which CLK_OUT ports have devices connected.
    /// </summary>
    [DisplayName("TimestampedDevicesConnectedPayload")]
    [Description("Creates a timestamped message payload that specifies which CLK_OUT ports have devices connected.")]
    public partial class CreateTimestampedDevicesConnectedPayload : CreateDevicesConnectedPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies which CLK_OUT ports have devices connected.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DevicesConnected register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.DevicesConnected.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that check whether device is a repeater or spreading internal timestamp.
    /// </summary>
    [DisplayName("RepeaterStatusPayload")]
    [Description("Creates a message payload that check whether device is a repeater or spreading internal timestamp.")]
    public partial class CreateRepeaterStatusPayload
    {
        /// <summary>
        /// Gets or sets the value that check whether device is a repeater or spreading internal timestamp.
        /// </summary>
        [Description("The value that check whether device is a repeater or spreading internal timestamp.")]
        public RepeaterFlags RepeaterStatus { get; set; }

        /// <summary>
        /// Creates a message payload for the RepeaterStatus register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public RepeaterFlags GetPayload()
        {
            return RepeaterStatus;
        }

        /// <summary>
        /// Creates a message that check whether device is a repeater or spreading internal timestamp.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the RepeaterStatus register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.RepeaterStatus.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that check whether device is a repeater or spreading internal timestamp.
    /// </summary>
    [DisplayName("TimestampedRepeaterStatusPayload")]
    [Description("Creates a timestamped message payload that check whether device is a repeater or spreading internal timestamp.")]
    public partial class CreateTimestampedRepeaterStatusPayload : CreateRepeaterStatusPayload
    {
        /// <summary>
        /// Creates a timestamped message that check whether device is a repeater or spreading internal timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the RepeaterStatus register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.RepeaterStatus.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configure how often the battery value is sent to computer.
    /// </summary>
    [DisplayName("BatteryRatePayload")]
    [Description("Creates a message payload that configure how often the battery value is sent to computer.")]
    public partial class CreateBatteryRatePayload
    {
        /// <summary>
        /// Gets or sets the value that configure how often the battery value is sent to computer.
        /// </summary>
        [Description("The value that configure how often the battery value is sent to computer.")]
        public BatteryRateConfiguration BatteryRate { get; set; }

        /// <summary>
        /// Creates a message payload for the BatteryRate register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public BatteryRateConfiguration GetPayload()
        {
            return BatteryRate;
        }

        /// <summary>
        /// Creates a message that configure how often the battery value is sent to computer.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the BatteryRate register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryRate.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configure how often the battery value is sent to computer.
    /// </summary>
    [DisplayName("TimestampedBatteryRatePayload")]
    [Description("Creates a timestamped message payload that configure how often the battery value is sent to computer.")]
    public partial class CreateTimestampedBatteryRatePayload : CreateBatteryRatePayload
    {
        /// <summary>
        /// Creates a timestamped message that configure how often the battery value is sent to computer.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the BatteryRate register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryRate.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that reads the current battery charge.
    /// </summary>
    [DisplayName("BatteryPayload")]
    [Description("Creates a message payload that reads the current battery charge.")]
    public partial class CreateBatteryPayload
    {
        /// <summary>
        /// Gets or sets the value that reads the current battery charge.
        /// </summary>
        [Description("The value that reads the current battery charge.")]
        public float Battery { get; set; }

        /// <summary>
        /// Creates a message payload for the Battery register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Battery;
        }

        /// <summary>
        /// Creates a message that reads the current battery charge.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Battery register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.Battery.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that reads the current battery charge.
    /// </summary>
    [DisplayName("TimestampedBatteryPayload")]
    [Description("Creates a timestamped message payload that reads the current battery charge.")]
    public partial class CreateTimestampedBatteryPayload : CreateBatteryPayload
    {
        /// <summary>
        /// Creates a timestamped message that reads the current battery charge.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Battery register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.Battery.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the low threshold from where the battery should start to be charged.
    /// </summary>
    [DisplayName("BatteryThresholdLowPayload")]
    [Description("Creates a message payload that specifies the low threshold from where the battery should start to be charged.")]
    public partial class CreateBatteryThresholdLowPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the low threshold from where the battery should start to be charged.
        /// </summary>
        [Description("The value that specifies the low threshold from where the battery should start to be charged.")]
        public float BatteryThresholdLow { get; set; }

        /// <summary>
        /// Creates a message payload for the BatteryThresholdLow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return BatteryThresholdLow;
        }

        /// <summary>
        /// Creates a message that specifies the low threshold from where the battery should start to be charged.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the BatteryThresholdLow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryThresholdLow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the low threshold from where the battery should start to be charged.
    /// </summary>
    [DisplayName("TimestampedBatteryThresholdLowPayload")]
    [Description("Creates a timestamped message payload that specifies the low threshold from where the battery should start to be charged.")]
    public partial class CreateTimestampedBatteryThresholdLowPayload : CreateBatteryThresholdLowPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the low threshold from where the battery should start to be charged.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the BatteryThresholdLow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryThresholdLow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the high threshold from where the battery stops being charged.
    /// </summary>
    [DisplayName("BatteryThresholdHighPayload")]
    [Description("Creates a message payload that specifies the high threshold from where the battery stops being charged.")]
    public partial class CreateBatteryThresholdHighPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the high threshold from where the battery stops being charged.
        /// </summary>
        [Description("The value that specifies the high threshold from where the battery stops being charged.")]
        public float BatteryThresholdHigh { get; set; }

        /// <summary>
        /// Creates a message payload for the BatteryThresholdHigh register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return BatteryThresholdHigh;
        }

        /// <summary>
        /// Creates a message that specifies the high threshold from where the battery stops being charged.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the BatteryThresholdHigh register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryThresholdHigh.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the high threshold from where the battery stops being charged.
    /// </summary>
    [DisplayName("TimestampedBatteryThresholdHighPayload")]
    [Description("Creates a timestamped message payload that specifies the high threshold from where the battery stops being charged.")]
    public partial class CreateTimestampedBatteryThresholdHighPayload : CreateBatteryThresholdHighPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the high threshold from where the battery stops being charged.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the BatteryThresholdHigh register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryThresholdHigh.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// for register BatteryCalibration0.
    /// </summary>
    [DisplayName("BatteryCalibration0Payload")]
    [Description("Creates a message payload for register BatteryCalibration0.")]
    public partial class CreateBatteryCalibration0Payload
    {
        /// <summary>
        /// Gets or sets the value for register BatteryCalibration0.
        /// </summary>
        [Description("The value for register BatteryCalibration0.")]
        public ushort BatteryCalibration0 { get; set; }

        /// <summary>
        /// Creates a message payload for the BatteryCalibration0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return BatteryCalibration0;
        }

        /// <summary>
        /// Creates a message for register BatteryCalibration0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the BatteryCalibration0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryCalibration0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// for register BatteryCalibration0.
    /// </summary>
    [DisplayName("TimestampedBatteryCalibration0Payload")]
    [Description("Creates a timestamped message payload for register BatteryCalibration0.")]
    public partial class CreateTimestampedBatteryCalibration0Payload : CreateBatteryCalibration0Payload
    {
        /// <summary>
        /// Creates a timestamped message for register BatteryCalibration0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the BatteryCalibration0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryCalibration0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// for register BatteryCalibration1.
    /// </summary>
    [DisplayName("BatteryCalibration1Payload")]
    [Description("Creates a message payload for register BatteryCalibration1.")]
    public partial class CreateBatteryCalibration1Payload
    {
        /// <summary>
        /// Gets or sets the value for register BatteryCalibration1.
        /// </summary>
        [Description("The value for register BatteryCalibration1.")]
        public ushort BatteryCalibration1 { get; set; }

        /// <summary>
        /// Creates a message payload for the BatteryCalibration1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return BatteryCalibration1;
        }

        /// <summary>
        /// Creates a message for register BatteryCalibration1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the BatteryCalibration1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryCalibration1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// for register BatteryCalibration1.
    /// </summary>
    [DisplayName("TimestampedBatteryCalibration1Payload")]
    [Description("Creates a timestamped message payload for register BatteryCalibration1.")]
    public partial class CreateTimestampedBatteryCalibration1Payload : CreateBatteryCalibration1Payload
    {
        /// <summary>
        /// Creates a timestamped message for register BatteryCalibration1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the BatteryCalibration1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.BatteryCalibration1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.
    /// </summary>
    [DisplayName("TimerPayload")]
    [Description("Creates a message payload that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.")]
    public partial class CreateTimerPayload
    {
        /// <summary>
        /// Gets or sets the value that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.
        /// </summary>
        [Description("The value that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.")]
        public uint Timer { get; set; }

        /// <summary>
        /// Creates a message payload for the Timer register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public uint GetPayload()
        {
            return Timer;
        }

        /// <summary>
        /// Creates a message that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Timer register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.Timer.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.
    /// </summary>
    [DisplayName("TimestampedTimerPayload")]
    [Description("Creates a timestamped message payload that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.")]
    public partial class CreateTimestampedTimerPayload : CreateTimerPayload
    {
        /// <summary>
        /// Creates a timestamped message that unitary counter that periodically produces a value at the frequency defined by register TimerFrequency.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Timer register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.Timer.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that frequency at which the Timer will generate new values.
    /// </summary>
    [DisplayName("TimerFrequencyPayload")]
    [Description("Creates a message payload that frequency at which the Timer will generate new values.")]
    public partial class CreateTimerFrequencyPayload
    {
        /// <summary>
        /// Gets or sets the value that frequency at which the Timer will generate new values.
        /// </summary>
        [Description("The value that frequency at which the Timer will generate new values.")]
        public TimerRate TimerFrequency { get; set; }

        /// <summary>
        /// Creates a message payload for the TimerFrequency register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public TimerRate GetPayload()
        {
            return TimerFrequency;
        }

        /// <summary>
        /// Creates a message that frequency at which the Timer will generate new values.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the TimerFrequency register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.TimerFrequency.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that frequency at which the Timer will generate new values.
    /// </summary>
    [DisplayName("TimestampedTimerFrequencyPayload")]
    [Description("Creates a timestamped message payload that frequency at which the Timer will generate new values.")]
    public partial class CreateTimestampedTimerFrequencyPayload : CreateTimerFrequencyPayload
    {
        /// <summary>
        /// Creates a timestamped message that frequency at which the Timer will generate new values.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the TimerFrequency register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.TimestampGeneratorGen3.TimerFrequency.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Specifies configuration flags for the device.
    /// </summary>
    [Flags]
    public enum ConfigurationFlags : byte
    {
        /// <summary>
        /// Specifies that no flags are defined.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Starts battery cycle to extend batteries life.
        /// </summary>
        [Description("Starts battery cycle to extend batteries life.")]
        StartBatteryCycle = 0x1,

        /// <summary>
        /// Starts to discharge right away.
        /// </summary>
        [Description("Starts to discharge right away.")]
        StartDischarge = 0x2,

        /// <summary>
        /// Starts to charge right away.
        /// </summary>
        [Description("Starts to charge right away.")]
        StartCharge = 0x4,

        /// <summary>
        /// Stop any control of the battery and resume normal function.
        /// </summary>
        [Description("Stop any control of the battery and resume normal function.")]
        Stop = 0x8
    }

    /// <summary>
    /// Specifies if an output port has a device connected.
    /// </summary>
    [Flags]
    public enum ConnectedDevices : byte
    {
        None = 0x0,
        Out0 = 0x1,
        Out1 = 0x2,
        Out2 = 0x4,
        Out3 = 0x8,
        Out4 = 0x10,
        Out5 = 0x20
    }

    /// <summary>
    /// Specifies whether the device is a clock repeater.
    /// </summary>
    [Flags]
    public enum RepeaterFlags : byte
    {
        None = 0x0,
        Repeater = 0x1
    }

    /// <summary>
    /// Specifies the rate at which the battery charge event is sent.
    /// </summary>
    public enum BatteryRateConfiguration : byte
    {
        EveryMinute = 0,
        Every10Seconds = 1,
        EverySecond = 2,
        Never = 3
    }

    /// <summary>
    /// Specifies the rate at which the timer counter is sent.
    /// </summary>
    public enum TimerRate : byte
    {
        Disabled = 0,
        Timer50Hz = 1,
        Timer100Hz = 2,
        Timer200Hz = 3,
        Timer500Hz = 4,
        Timer1000Hz = 5
    }

    internal static partial class PayloadMarshal
    {
        internal static T[] GetSubArray<T>(T[] array, int offset, int count)
        {
            var result = new T[count];
            Array.Copy(array, offset, result, 0, count);
            return result;
        }

        internal static byte ReadByte(ArraySegment<byte> segment) => segment.Array[segment.Offset];

        internal static sbyte ReadSByte(ArraySegment<byte> segment) => (sbyte)segment.Array[segment.Offset];

        internal static ushort ReadUInt16(ArraySegment<byte> segment) => BitConverter.ToUInt16(segment.Array, segment.Offset);

        internal static short ReadInt16(ArraySegment<byte> segment) => BitConverter.ToInt16(segment.Array, segment.Offset);

        internal static uint ReadUInt32(ArraySegment<byte> segment) => BitConverter.ToUInt32(segment.Array, segment.Offset);

        internal static int ReadInt32(ArraySegment<byte> segment) => BitConverter.ToInt32(segment.Array, segment.Offset);

        internal static ulong ReadUInt64(ArraySegment<byte> segment) => BitConverter.ToUInt64(segment.Array, segment.Offset);

        internal static long ReadInt64(ArraySegment<byte> segment) => BitConverter.ToInt64(segment.Array, segment.Offset);

        internal static float ReadSingle(ArraySegment<byte> segment) => BitConverter.ToSingle(segment.Array, segment.Offset);

        internal static string ReadUtf8String(ArraySegment<byte> segment)
        {
            var count = Array.IndexOf(segment.Array, (byte)0, segment.Offset, segment.Count) - segment.Offset;
            return System.Text.Encoding.UTF8.GetString(segment.Array, segment.Offset, count < 0 ? segment.Count : count);
        }

        internal static void Write(ArraySegment<byte> segment, byte value) => segment.Array[segment.Offset] = value;

        internal static void Write(ArraySegment<byte> segment, sbyte value) => segment.Array[segment.Offset] = (byte)value;

        internal static void Write(ArraySegment<byte> segment, ushort value)
        {
            segment.Array[segment.Offset] = (byte)value;
            segment.Array[segment.Offset + 1] = (byte)(value >> 8);
        }

        internal static void Write(ArraySegment<byte> segment, short value)
        {
            segment.Array[segment.Offset] = (byte)value;
            segment.Array[segment.Offset + 1] = (byte)(value >> 8);
        }

        internal static void Write(ArraySegment<byte> segment, uint value)
        {
            segment.Array[segment.Offset] = (byte)value;
            segment.Array[segment.Offset + 1] = (byte)(value >> 8);
            segment.Array[segment.Offset + 2] = (byte)(value >> 16);
            segment.Array[segment.Offset + 3] = (byte)(value >> 24);
        }

        internal static void Write(ArraySegment<byte> segment, int value)
        {
            segment.Array[segment.Offset] = (byte)value;
            segment.Array[segment.Offset + 1] = (byte)(value >> 8);
            segment.Array[segment.Offset + 2] = (byte)(value >> 16);
            segment.Array[segment.Offset + 3] = (byte)(value >> 24);
        }

        internal static void Write(ArraySegment<byte> segment, ulong value)
        {
            segment.Array[segment.Offset] = (byte)value;
            segment.Array[segment.Offset + 1] = (byte)(value >> 8);
            segment.Array[segment.Offset + 2] = (byte)(value >> 16);
            segment.Array[segment.Offset + 3] = (byte)(value >> 24);
            segment.Array[segment.Offset + 4] = (byte)(value >> 32);
            segment.Array[segment.Offset + 5] = (byte)(value >> 40);
            segment.Array[segment.Offset + 6] = (byte)(value >> 48);
            segment.Array[segment.Offset + 7] = (byte)(value >> 56);
        }

        internal static void Write(ArraySegment<byte> segment, long value)
        {
            segment.Array[segment.Offset] = (byte)value;
            segment.Array[segment.Offset + 1] = (byte)(value >> 8);
            segment.Array[segment.Offset + 2] = (byte)(value >> 16);
            segment.Array[segment.Offset + 3] = (byte)(value >> 24);
            segment.Array[segment.Offset + 4] = (byte)(value >> 32);
            segment.Array[segment.Offset + 5] = (byte)(value >> 40);
            segment.Array[segment.Offset + 6] = (byte)(value >> 48);
            segment.Array[segment.Offset + 7] = (byte)(value >> 56);
        }

        internal static unsafe void Write(ArraySegment<byte> segment, float value) => Write(segment, *(int*)&value);

        internal static unsafe void Write(ArraySegment<byte> segment, string value) =>
            System.Text.Encoding.UTF8.GetBytes(value, 0, Math.Min(value.Length, segment.Count), segment.Array, segment.Offset);

        internal static void Write<T>(ArraySegment<byte> segment, T[] values) where T : unmanaged
        {
            Buffer.BlockCopy(values, 0, segment.Array, segment.Offset, segment.Count);
        }

        internal static void Write<T>(ArraySegment<T> segment, T[] values)
        {
            Array.Copy(values, 0, segment.Array, segment.Offset, segment.Count);
        }
    }
}

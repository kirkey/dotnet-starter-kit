using Accounting.Domain.Events.Meter;

namespace Accounting.Domain;

/// <summary>
/// Represents a physical or smart meter installed at a service location for measuring electricity consumption and demand.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track physical meters installed at member service locations for accurate billing.
/// - Support both analog and smart meter technologies with different reading capabilities.
/// - Manage meter lifecycle from installation to retirement including maintenance schedules.
/// - Enable automated meter reading (AMR) and advanced metering infrastructure (AMI) integration.
/// - Track meter accuracy, calibration dates, and regulatory compliance requirements.
/// - Support CT/PT multipliers for high-voltage commercial and industrial installations.
/// - Monitor meter communication status and data collection reliability.
/// - Maintain meter configuration for tariff calculations and demand billing.
/// 
/// Default values:
/// - MeterNumber: required unique identifier (example: "M123456789" or serial number)
/// - MeterType: required type classification (example: "Single Phase", "Three Phase", "Smart Meter")
/// - Manufacturer: required manufacturer name (example: "Itron", "Landis+Gyr", "Sensus")
/// - Model: required model number (example: "OpenWay CENTRON", "S4x")
/// - Status: "Active" (new meters start as active)
/// - InstallationDate: required installation date for service tracking
/// - Multiplier: required positive multiplier for CT/PT calculations (default: 1.0)
/// - LastReadingDate: null (set when first reading is taken)
/// - LastReading: 0.0 (initial meter reading)
/// - ServiceLocation: required physical installation address
/// - CommunicationProtocol: optional for smart meters (example: "RF Mesh", "Cellular", "PLC")
/// 
/// Business rules:
/// - MeterNumber must be unique within the utility system
/// - Multiplier must be positive (typically 1.0 for residential, higher for CT/PT installations)
/// - Cannot delete meters with reading history
/// - Status changes require proper authorization and field verification
/// - Installation date cannot be in the future
/// - Meter readings must be sequential and increasing
/// - Communication protocol required for smart meters
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Meter.MeterCreated"/>
/// <seealso cref="Accounting.Domain.Events.Meter.MeterUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Meter.MeterInstalled"/>
/// <seealso cref="Accounting.Domain.Events.Meter.MeterRetired"/>
/// <seealso cref="Accounting.Domain.Events.Meter.MeterReadingTaken"/>
/// <seealso cref="Accounting.Domain.Events.Meter.MeterStatusChanged"/>
/// <seealso cref="Accounting.Domain.Events.Meter.MeterCommunicationLost"/>
public class Meter : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique meter number.
    /// </summary>
    public string MeterNumber { get; private set; }

    /// <summary>
    /// Meter type (Single Phase, Three Phase, Smart Meter, Analog, etc.).
    /// </summary>
    public string MeterType { get; private set; } // "Single Phase", "Three Phase", "Smart Meter", "Analog"

    /// <summary>
    /// Meter manufacturer.
    /// </summary>
    public string Manufacturer { get; private set; }

    /// <summary>
    /// Manufacturer model number.
    /// </summary>
    public string ModelNumber { get; private set; }

    /// <summary>
    /// Optional manufacturer serial number.
    /// </summary>
    public string? SerialNumber { get; private set; }

    /// <summary>
    /// Date the meter was installed.
    /// </summary>
    public DateTime InstallationDate { get; private set; }

    /// <summary>
    /// Date of the most recent reading, if any.
    /// </summary>
    public DateTime? LastReadingDate { get; private set; }

    /// <summary>
    /// Most recent reading value, if any.
    /// </summary>
    public decimal? LastReading { get; private set; }

    /// <summary>
    /// Meter multiplier (CT/PT ratio). Must be positive.
    /// </summary>
    public decimal Multiplier { get; private set; } // CT/PT ratio

    /// <summary>
    /// Operational status (Active, Inactive, Defective, Pending Installation, etc.).
    /// </summary>
    public string Status { get; private set; } // "Active", "Inactive", "Defective", "Pending Installation"

    /// <summary>
    /// Physical location description.
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// GPS coordinates string for mapping.
    /// </summary>
    public string? GpsCoordinates { get; private set; }

    /// <summary>
    /// Current member assigned to this meter.
    /// </summary>
    public DefaultIdType? MemberId { get; private set; } // Current member assigned to this meter

    /// <summary>
    /// Whether the device is a smart meter.
    /// </summary>
    public bool IsSmartMeter { get; private set; }

    /// <summary>
    /// Communication protocol (AMR, AMI, Manual).
    /// </summary>
    public string? CommunicationProtocol { get; private set; } // "AMR", "AMI", "Manual"

    /// <summary>
    /// Date of last maintenance.
    /// </summary>
    public DateTime? LastMaintenanceDate { get; private set; }

    /// <summary>
    /// Next calibration date.
    /// </summary>
    public DateTime? NextCalibrationDate { get; private set; }

    /// <summary>
    /// Accuracy class for the meter.
    /// </summary>
    public decimal? AccuracyClass { get; private set; } // Meter accuracy rating

    /// <summary>
    /// Configuration (Demand, Time of Use, Standard).
    /// </summary>
    public string? MeterConfiguration { get; private set; } // "Demand", "Time of Use", "Standard"

    private readonly List<MeterReading> _readings = new();
    /// <summary>
    /// Collection of readings captured for this meter.
    /// </summary>
    public IReadOnlyCollection<MeterReading> Readings => _readings.AsReadOnly();
    
    private Meter()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private Meter(string meterNumber, string meterType, string manufacturer,
        string modelNumber, DateTime installationDate, decimal multiplier = 1,
        string? serialNumber = null, string? location = null, string? gpsCoordinates = null,
        DefaultIdType? memberId = null, bool isSmartMeter = false, string? communicationProtocol = null,
        decimal? accuracyClass = null, string? meterConfiguration = null,
        string? description = null, string? notes = null)
    {
        MeterNumber = meterNumber.Trim();
        Name = meterNumber.Trim(); // Keep for compatibility
        MeterType = meterType.Trim();
        Manufacturer = manufacturer.Trim();
        ModelNumber = modelNumber.Trim();
        SerialNumber = serialNumber?.Trim();
        InstallationDate = installationDate;
        Multiplier = multiplier;
        Status = "Active";
        Location = location?.Trim();
        GpsCoordinates = gpsCoordinates?.Trim();
        MemberId = memberId;
        IsSmartMeter = isSmartMeter;
        CommunicationProtocol = communicationProtocol?.Trim();
        AccuracyClass = accuracyClass;
        MeterConfiguration = meterConfiguration?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new MeterCreated(Id, MeterNumber, MeterType, Manufacturer, ModelNumber, Description, Notes));
    }

    /// <summary>
    /// Create a meter with validation for type, manufacturer, model, and multiplier.
    /// </summary>
    public static Meter Create(string meterNumber, string meterType, string manufacturer,
        string modelNumber, DateTime installationDate, decimal multiplier = 1,
        string? serialNumber = null, string? location = null, string? gpsCoordinates = null,
        DefaultIdType? memberId = null, bool isSmartMeter = false, string? communicationProtocol = null,
        decimal? accuracyClass = null, string? meterConfiguration = null,
        string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(meterNumber))
            throw new ArgumentException("Meter number cannot be empty");

        if (string.IsNullOrWhiteSpace(meterType))
            throw new ArgumentException("Meter type cannot be empty");

        if (string.IsNullOrWhiteSpace(manufacturer))
            throw new ArgumentException("Manufacturer cannot be empty");

        if (string.IsNullOrWhiteSpace(modelNumber))
            throw new ArgumentException("Model number cannot be empty");

        if (multiplier <= 0)
            throw new ArgumentException("Multiplier must be positive");

        if (!IsValidMeterType(meterType))
            throw new ArgumentException($"Invalid meter type: {meterType}");

        return new Meter(meterNumber, meterType, manufacturer, modelNumber,
            installationDate, multiplier, serialNumber, location, gpsCoordinates, memberId,
            isSmartMeter, communicationProtocol, accuracyClass, meterConfiguration, description, notes);
    }

    /// <summary>
    /// Update location, mapping, assignment, communication and description fields.
    /// </summary>
    public Meter Update(string? location = null, string? gpsCoordinates = null, DefaultIdType? memberId = null,
        string? communicationProtocol = null, string? meterConfiguration = null, 
        string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (location != Location)
        {
            Location = location?.Trim();
            isUpdated = true;
        }

        if (gpsCoordinates != GpsCoordinates)
        {
            GpsCoordinates = gpsCoordinates?.Trim();
            isUpdated = true;
        }

        if (memberId != MemberId)
        {
            MemberId = memberId;
            isUpdated = true;
        }

        if (communicationProtocol != CommunicationProtocol)
        {
            CommunicationProtocol = communicationProtocol?.Trim();
            isUpdated = true;
        }

        if (meterConfiguration != MeterConfiguration)
        {
            MeterConfiguration = meterConfiguration?.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new MeterUpdated(Id, MeterNumber, MeterType, Description, Notes));
        }

        return this;
    }

    /// <summary>
    /// Update meter status with validation of allowed values.
    /// </summary>
    public Meter UpdateStatus(string status)
    {
        if (!IsValidStatus(status))
            throw new ArgumentException($"Invalid meter status: {status}");

        if (Status != status.Trim())
        {
            Status = status.Trim();
            QueueDomainEvent(new MeterStatusChanged(Id, MeterNumber, Status));
        }

        return this;
    }

    /// <summary>
    /// Update maintenance/calibration dates.
    /// </summary>
    public Meter UpdateMaintenance(DateTime? lastMaintenanceDate, DateTime? nextCalibrationDate)
    {
        bool isUpdated = false;

        if (lastMaintenanceDate != LastMaintenanceDate)
        {
            LastMaintenanceDate = lastMaintenanceDate;
            isUpdated = true;
        }

        if (nextCalibrationDate != NextCalibrationDate)
        {
            NextCalibrationDate = nextCalibrationDate;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new MeterMaintenanceUpdated(Id, MeterNumber, LastMaintenanceDate, NextCalibrationDate));
        }

        return this;
    }

    /// <summary>
    /// Append a new reading with validation that it is not less than the last reading.
    /// </summary>
    public Meter AddReading(decimal reading, DateTime readingDate, string readingType = "Actual", string? readBy = null)
    {
        if (reading < 0)
            throw new InvalidMeterReadingException();

        if (LastReading.HasValue && reading < LastReading.Value)
            throw new InvalidMeterReadingException();

        var meterReading = MeterReading.Create(Id, reading, readingDate, readingType, readBy);
        _readings.Add(meterReading);

        LastReading = reading;
        LastReadingDate = readingDate;

        QueueDomainEvent(new MeterReadingAdded(Id, MeterNumber, reading, readingDate, readingType));
        return this;
    }

    private static bool IsValidMeterType(string meterType)
    {
        var validTypes = new[] { "Single Phase", "Three Phase", "Smart Meter", "Analog", "Digital", 
            "Demand Meter", "Time of Use", "Net Meter", "Prepaid Meter" };
        return validTypes.Contains(meterType.Trim(), StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidStatus(string status)
    {
        var validStatuses = new[] { "Active", "Inactive", "Defective", "Pending Installation", 
            "Removed", "Testing", "Calibration Required" };
        return validStatuses.Contains(status.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}

/// <summary>
/// A single meter reading entry with type and optional reader info.
/// </summary>
public class MeterReading : BaseEntity
{
    /// <summary>
    /// Parent meter identifier.
    /// </summary>
    public DefaultIdType MeterId { get; private set; }

    /// <summary>
    /// Reading value captured.
    /// </summary>
    public decimal Reading { get; private set; }

    /// <summary>
    /// Timestamp when the reading was taken.
    /// </summary>
    public DateTime ReadingDate { get; private set; }

    /// <summary>
    /// Reading type: Actual, Estimated, or Customer Read.
    /// </summary>
    public string ReadingType { get; private set; } // "Actual", "Estimated", "Customer Read"

    /// <summary>
    /// Optional operator/reader identifier.
    /// </summary>
    public string? ReadBy { get; private set; }

    /// <summary>
    /// True if validated; defaults to true for Actual readings.
    /// </summary>
    public bool IsValidated { get; private set; }

    private MeterReading(DefaultIdType meterId, decimal reading, DateTime readingDate,
        string readingType, string? readBy = null)
    {
        MeterId = meterId;
        Reading = reading;
        ReadingDate = readingDate;
        ReadingType = readingType.Trim();
        ReadBy = readBy?.Trim();
        IsValidated = readingType == "Actual";
    }

    /// <summary>
    /// Create a reading; reading must be non-negative and reading type non-empty.
    /// </summary>
    public static MeterReading Create(DefaultIdType meterId, decimal reading, DateTime readingDate,
        string readingType, string? readBy = null)
    {
        if (reading < 0)
            throw new InvalidMeterReadingException();

        if (string.IsNullOrWhiteSpace(readingType))
            throw new ArgumentException("Reading type cannot be empty");

        return new MeterReading(meterId, reading, readingDate, readingType, readBy);
    }

    /// <summary>
    /// Mark the reading as validated.
    /// </summary>
    public MeterReading Validate()
    {
        if (!IsValidated)
        {
            IsValidated = true;
        }
        return this;
    }
}

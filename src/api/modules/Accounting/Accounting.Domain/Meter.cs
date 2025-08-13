using Accounting.Domain.Events.Meter;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class Meter : AuditableEntity, IAggregateRoot
{
    public string MeterNumber { get; private set; }
    public string MeterType { get; private set; } // "Single Phase", "Three Phase", "Smart Meter", "Analog"
    public string Manufacturer { get; private set; }
    public string ModelNumber { get; private set; }
    public string? SerialNumber { get; private set; }
    public DateTime InstallationDate { get; private set; }
    public DateTime? LastReadingDate { get; private set; }
    public decimal? LastReading { get; private set; }
    public decimal Multiplier { get; private set; } // CT/PT ratio
    public string Status { get; private set; } // "Active", "Inactive", "Defective", "Pending Installation"
    public string? Location { get; private set; }
    public string? GpsCoordinates { get; private set; }
    public DefaultIdType? MemberId { get; private set; } // Current member assigned to this meter
    public bool IsSmartMeter { get; private set; }
    public string? CommunicationProtocol { get; private set; } // "AMR", "AMI", "Manual"
    public DateTime? LastMaintenanceDate { get; private set; }
    public DateTime? NextCalibrationDate { get; private set; }
    public decimal? AccuracyClass { get; private set; } // Meter accuracy rating
    public string? MeterConfiguration { get; private set; } // "Demand", "Time of Use", "Standard"

    private readonly List<MeterReading> _readings = new();
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

public class MeterReading : BaseEntity
{
    public DefaultIdType MeterId { get; private set; }
    public decimal Reading { get; private set; }
    public DateTime ReadingDate { get; private set; }
    public string ReadingType { get; private set; } // "Actual", "Estimated", "Customer Read"
    public string? ReadBy { get; private set; }
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

    public static MeterReading Create(DefaultIdType meterId, decimal reading, DateTime readingDate,
        string readingType, string? readBy = null)
    {
        if (reading < 0)
            throw new InvalidMeterReadingException();

        if (string.IsNullOrWhiteSpace(readingType))
            throw new ArgumentException("Reading type cannot be empty");

        return new MeterReading(meterId, reading, readingDate, readingType, readBy);
    }

    public MeterReading Validate()
    {
        if (!IsValidated)
        {
            IsValidated = true;
        }
        return this;
    }
}

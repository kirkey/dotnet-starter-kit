namespace Accounting.Application.Meters.Responses;

/// <summary>
/// Response model representing a meter entity.
/// Contains comprehensive meter information including technical specifications, readings, and maintenance details.
/// </summary>
public class MeterResponse
{
    /// <summary>
    /// Unique identifier for the meter.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Unique meter number for identification and reference.
    /// </summary>
    public string MeterNumber { get; set; } = null!;
    
    /// <summary>
    /// Type of meter (e.g., "Electric", "Water", "Gas").
    /// </summary>
    public string MeterType { get; set; } = null!;
    
    /// <summary>
    /// Manufacturer of the meter.
    /// </summary>
    public string Manufacturer { get; set; } = null!;
    
    /// <summary>
    /// Model number of the meter.
    /// </summary>
    public string ModelNumber { get; set; } = null!;
    
    /// <summary>
    /// Serial number of the meter for identification.
    /// </summary>
    public string? SerialNumber { get; set; }
    
    /// <summary>
    /// Date when the meter was installed.
    /// </summary>
    public DateTime InstallationDate { get; set; }
    
    /// <summary>
    /// Last recorded reading from the meter.
    /// </summary>
    public decimal? LastReading { get; set; }
    
    /// <summary>
    /// Date when the last reading was taken.
    /// </summary>
    public DateTime? LastReadingDate { get; set; }
    
    /// <summary>
    /// Multiplier used for calculating actual usage from meter readings.
    /// </summary>
    public decimal Multiplier { get; set; }
    
    /// <summary>
    /// Current operational status of the meter (e.g., "Active", "Inactive", "Faulty").
    /// </summary>
    public string Status { get; set; } = null!;
    
    /// <summary>
    /// Physical location or address where the meter is installed.
    /// </summary>
    public string? Location { get; set; }
    
    /// <summary>
    /// GPS coordinates of the meter location.
    /// </summary>
    public string? GpsCoordinates { get; set; }
    
    /// <summary>
    /// Member identifier associated with this meter.
    /// </summary>
    public DefaultIdType? MemberId { get; set; }
    
    /// <summary>
    /// Indicates whether this is a smart meter with advanced capabilities.
    /// </summary>
    public bool IsSmartMeter { get; set; }
    
    /// <summary>
    /// Communication protocol used by the meter (for smart meters).
    /// </summary>
    public string? CommunicationProtocol { get; set; }
    
    /// <summary>
    /// Date when the meter was last serviced or maintained.
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }
    
    /// <summary>
    /// Scheduled date for the next calibration of the meter.
    /// </summary>
    public DateTime? NextCalibrationDate { get; set; }
    
    /// <summary>
    /// Accuracy class rating of the meter.
    /// </summary>
    public decimal? AccuracyClass { get; set; }
    
    /// <summary>
    /// Configuration settings or parameters for the meter.
    /// </summary>
    public string? MeterConfiguration { get; set; }
    
    /// <summary>
    /// Description or additional details about the meter.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Additional notes or comments about the meter.
    /// </summary>
    public string? Notes { get; set; }
}

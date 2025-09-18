 namespace Accounting.Application.Meters.Dtos;

 public class MeterDto
 {
     public DefaultIdType Id { get; set; }
     public string MeterNumber { get; set; } = default!;
     public string MeterType { get; set; } = default!;
     public string Manufacturer { get; set; } = default!;
     public string ModelNumber { get; set; } = default!;
     public string? SerialNumber { get; set; }
     public DateTime InstallationDate { get; set; }
     public decimal? LastReading { get; set; }
     public DateTime? LastReadingDate { get; set; }
     public decimal Multiplier { get; set; }
     public string Status { get; set; } = default!;
     public string? Location { get; set; }
     public string? GpsCoordinates { get; set; }
     public DefaultIdType? MemberId { get; set; }
     public bool IsSmartMeter { get; set; }
     public string? CommunicationProtocol { get; set; }
     public DateTime? LastMaintenanceDate { get; set; }
     public DateTime? NextCalibrationDate { get; set; }
     public decimal? AccuracyClass { get; set; }
     public string? MeterConfiguration { get; set; }
     public string? Description { get; set; }
     public string? Notes { get; set; }
 }
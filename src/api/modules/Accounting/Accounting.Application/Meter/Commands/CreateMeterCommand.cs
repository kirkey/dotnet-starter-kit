namespace Accounting.Application.Meter.Commands
{
    public class CreateMeterCommand : IRequest<DefaultIdType>
    {
        public string MeterNumber { get; set; } = default!;
        public string MeterType { get; set; } = default!;
        public string Manufacturer { get; set; } = default!;
        public string ModelNumber { get; set; } = default!;
        public DateTime InstallationDate { get; set; }
        public decimal Multiplier { get; set; } = 1m;
        public string? SerialNumber { get; set; }
        public string? Location { get; set; }
        public DefaultIdType? MemberId { get; set; }
        public bool IsSmartMeter { get; set; }
        public string? CommunicationProtocol { get; set; }
        public decimal? AccuracyClass { get; set; }
        public string? MeterConfiguration { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
    }
}

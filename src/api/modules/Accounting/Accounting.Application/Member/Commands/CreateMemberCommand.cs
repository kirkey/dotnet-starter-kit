namespace Accounting.Application.Member.Commands
{
    public class CreateMemberCommand : IRequest<DefaultIdType>
    {
        public string MemberNumber { get; set; } = default!;
        public string MemberName { get; set; } = default!;
        public string ServiceAddress { get; set; } = default!;
        public DateTime MembershipDate { get; set; }
        public string? MailingAddress { get; set; }
        public string? ContactInfo { get; set; }
        public string AccountStatus { get; set; } = "Active";
        public DefaultIdType? MeterId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmergencyContact { get; set; }
        public string? ServiceClass { get; set; }
        public DefaultIdType? RateScheduleId { get; set; }
        public string? RateSchedule { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
    }
}


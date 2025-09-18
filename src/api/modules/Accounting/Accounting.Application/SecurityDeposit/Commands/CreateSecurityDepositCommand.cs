namespace Accounting.Application.SecurityDeposit.Commands
{
    public class CreateSecurityDepositCommand : IRequest<DefaultIdType>
    {
        public DefaultIdType MemberId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DepositDate { get; set; }
        public string? Notes { get; set; }
    }
}


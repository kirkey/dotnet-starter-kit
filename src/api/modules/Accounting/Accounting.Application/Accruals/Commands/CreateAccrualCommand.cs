namespace Accounting.Application.Accruals.Commands
{
    public class CreateAccrualCommand : IRequest<DefaultIdType>
    {
        public string AccrualNumber { get; set; } = default!;
        public DateTime AccrualDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
    }
}


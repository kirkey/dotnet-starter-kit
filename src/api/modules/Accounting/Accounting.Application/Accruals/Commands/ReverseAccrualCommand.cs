namespace Accounting.Application.Accruals.Commands
{
    public class ReverseAccrualCommand : IRequest
    {
        public DefaultIdType Id { get; set; }
        public DateTime ReversalDate { get; set; }
    }
}


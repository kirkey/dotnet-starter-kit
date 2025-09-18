using Accounting.Application.SecurityDeposit.Commands;

namespace Accounting.Application.SecurityDeposit.Handlers
{
    public class CreateSecurityDepositHandler(IRepository<Accounting.Domain.SecurityDeposit> repository)
        : IRequestHandler<CreateSecurityDepositCommand, DefaultIdType>
    {
        public async Task<DefaultIdType> Handle(CreateSecurityDepositCommand request, CancellationToken cancellationToken)
        {
            var sd = Accounting.Domain.SecurityDeposit.Create(
                request.MemberId,
                request.Amount,
                request.DepositDate,
                request.Notes);

            await repository.AddAsync(sd, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return sd.Id;
        }
    }
}


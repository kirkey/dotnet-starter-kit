using Accounting.Application.SecurityDeposits.Commands;

namespace Accounting.Application.SecurityDeposits.Handlers;

public class CreateSecurityDepositHandler(IRepository<SecurityDeposit> repository)
    : IRequestHandler<CreateSecurityDepositCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateSecurityDepositCommand request, CancellationToken cancellationToken)
    {
        var sd = SecurityDeposit.Create(
            request.MemberId,
            request.Amount,
            request.DepositDate,
            request.Notes);

        await repository.AddAsync(sd, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return sd.Id;
    }
}

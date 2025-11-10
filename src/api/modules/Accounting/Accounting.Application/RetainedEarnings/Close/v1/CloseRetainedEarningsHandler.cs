using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.RetainedEarnings.Close.v1;

public sealed class CloseRetainedEarningsHandler(
    [FromKeyedServices("accounting:retained-earnings")] IRepository<Domain.Entities.RetainedEarnings> repository,
    ICurrentUser currentUser,
    ILogger<CloseRetainedEarningsHandler> logger)
    : IRequestHandler<CloseRetainedEarningsCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CloseRetainedEarningsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        logger.LogInformation("Closing retained earnings {Id}", request.Id);

        var re = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (re == null) throw new NotFoundException($"Retained earnings with ID {request.Id} not found");

        var closedBy = currentUser.GetUserName() ?? "Unknown";
        re.Close(closedBy);
        await repository.UpdateAsync(re, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Retained earnings FY{FiscalYear} closed by {ClosedBy}", re.FiscalYear, closedBy);
        return re.Id;
    }
}


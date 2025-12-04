using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Create.v1;

public sealed class CreateShareAccountHandler(
    [FromKeyedServices("microfinance:shareaccounts")] IRepository<ShareAccount> repository,
    [FromKeyedServices("microfinance:members")] IReadRepository<Member> memberRepository,
    [FromKeyedServices("microfinance:shareproducts")] IReadRepository<ShareProduct> shareProductRepository,
    ILogger<CreateShareAccountHandler> logger)
    : IRequestHandler<CreateShareAccountCommand, CreateShareAccountResponse>
{
    public async Task<CreateShareAccountResponse> Handle(CreateShareAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate member exists
        var member = await memberRepository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.MemberId), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.MemberId} not found.");
        }

        // Validate share product exists
        var shareProduct = await shareProductRepository.FirstOrDefaultAsync(
            new ShareProductByIdSpec(request.ShareProductId), cancellationToken).ConfigureAwait(false);

        if (shareProduct is null)
        {
            throw new NotFoundException($"Share product with ID {request.ShareProductId} not found.");
        }

        // Check for duplicate account number
        var existingAccount = await repository.FirstOrDefaultAsync(
            new ShareAccountByAccountNumberSpec(request.AccountNumber), cancellationToken).ConfigureAwait(false);

        if (existingAccount is not null)
        {
            throw new InvalidOperationException($"A share account with number '{request.AccountNumber}' already exists.");
        }

        var shareAccount = ShareAccount.Create(
            request.AccountNumber,
            request.MemberId,
            request.ShareProductId,
            request.OpenedDate,
            request.Notes
        );

        await repository.AddAsync(shareAccount, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Share account {AccountNumber} created with ID {ShareAccountId}", shareAccount.AccountNumber, shareAccount.Id);

        return new CreateShareAccountResponse(shareAccount.Id, shareAccount.AccountNumber);
    }
}

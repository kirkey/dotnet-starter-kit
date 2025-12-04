using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Create.v1;

public sealed class CreateFixedDepositHandler(
    [FromKeyedServices("microfinance:fixeddeposits")] IRepository<FixedDeposit> repository,
    [FromKeyedServices("microfinance:members")] IReadRepository<Member> memberRepository,
    ILogger<CreateFixedDepositHandler> logger)
    : IRequestHandler<CreateFixedDepositCommand, CreateFixedDepositResponse>
{
    public async Task<CreateFixedDepositResponse> Handle(CreateFixedDepositCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate member exists
        var member = await memberRepository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.MemberId), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.MemberId} not found.");
        }

        // Check for duplicate certificate number
        var existingDeposit = await repository.FirstOrDefaultAsync(
            new FixedDepositByCertificateNumberSpec(request.CertificateNumber), cancellationToken).ConfigureAwait(false);

        if (existingDeposit is not null)
        {
            throw new InvalidOperationException($"A fixed deposit with certificate number '{request.CertificateNumber}' already exists.");
        }

        var fixedDeposit = FixedDeposit.Create(
            request.CertificateNumber,
            request.MemberId,
            request.PrincipalAmount,
            request.InterestRate,
            request.TermMonths,
            request.SavingsProductId,
            request.LinkedSavingsAccountId,
            request.DepositDate,
            request.MaturityInstruction,
            request.Notes
        );

        await repository.AddAsync(fixedDeposit, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Fixed deposit {CertificateNumber} created with ID {FixedDepositId}", fixedDeposit.CertificateNumber, fixedDeposit.Id);

        return new CreateFixedDepositResponse(fixedDeposit.Id, fixedDeposit.CertificateNumber, fixedDeposit.MaturityDate);
    }
}

using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;

public sealed class GetLoanGuarantorHandler(
    [FromKeyedServices("microfinance:loanguarantors")] IRepository<LoanGuarantor> repository)
    : IRequestHandler<GetLoanGuarantorRequest, LoanGuarantorResponse>
{
    public async Task<LoanGuarantorResponse> Handle(GetLoanGuarantorRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var guarantor = await repository.FirstOrDefaultAsync(
            new LoanGuarantorByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (guarantor is null)
        {
            throw new NotFoundException($"Loan guarantor with ID {request.Id} not found.");
        }

        return new LoanGuarantorResponse(
            guarantor.Id,
            guarantor.LoanId,
            guarantor.GuarantorMemberId,
            guarantor.GuaranteedAmount,
            guarantor.Relationship,
            guarantor.GuaranteeDate,
            guarantor.ExpiryDate,
            guarantor.Status,
            guarantor.Notes
        );
    }
}

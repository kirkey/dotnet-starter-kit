using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Domain.Exceptions;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Get.v1;

public sealed class GetPayrollDeductionHandler(
    [FromKeyedServices("humanresources:payrolldeductions")] IRepository<PayrollDeduction> repository)
    : IRequestHandler<GetPayrollDeductionRequest, PayrollDeductionResponse>
{
    public async Task<PayrollDeductionResponse> Handle(GetPayrollDeductionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = deduction ?? throw new PayrollDeductionNotFoundException(request.Id);

        return deduction.Adapt<PayrollDeductionResponse>();
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Get.v1;

using Framework.Core.Persistence;
using Specifications;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for getting payroll deduction details.
/// </summary>
public sealed class GetPayrollDeductionHandler(
    [FromKeyedServices("hr:payrolldeductions")] IReadRepository<PayrollDeduction> repository)
    : IRequestHandler<GetPayrollDeductionRequest, PayrollDeductionResponse>
{
    public async Task<PayrollDeductionResponse> Handle(
        GetPayrollDeductionRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PayrollDeductionByIdSpec(request.Id);
        var deduction = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (deduction is null)
            throw new PayrollDeductionNotFoundException(request.Id);

        return new PayrollDeductionResponse(
            deduction.Id,
            deduction.PayComponentId,
            deduction.PayComponent.ComponentName,
            deduction.DeductionType,
            deduction.DeductionAmount,
            deduction.DeductionPercentage,
            deduction.IsActive,
            deduction.IsAuthorized,
            deduction.IsRecoverable,
            deduction.StartDate,
            deduction.EndDate,
            deduction.MaxDeductionLimit,
            deduction.EmployeeId,
            deduction.Employee?.FullName,
            deduction.OrganizationalUnitId,
            deduction.OrganizationalUnit?.Name,
            deduction.ReferenceNumber,
            deduction.Remarks);
    }
}


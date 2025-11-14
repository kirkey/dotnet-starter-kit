namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Search.v1;

using Framework.Core.Paging;
using Framework.Core.Persistence;
using Specifications;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for searching payroll deductions.
/// </summary>
public sealed class SearchPayrollDeductionsHandler(
    [FromKeyedServices("hr:payrolldeductions")] IReadRepository<PayrollDeduction> repository)
    : IRequestHandler<SearchPayrollDeductionsRequest, PagedList<PayrollDeductionDto>>
{
    public async Task<PagedList<PayrollDeductionDto>> Handle(
        SearchPayrollDeductionsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPayrollDeductionsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var dtos = items.Select(d => new PayrollDeductionDto(
            d.Id,
            d.PayComponent.ComponentName,
            d.DeductionType,
            d.DeductionAmount > 0 ? d.DeductionAmount : (d.DeductionAmount > 0 ? d.DeductionAmount : 0),
            d.IsActive,
            d.IsAuthorized,
            d.EmployeeId.HasValue ? "Employee" : (d.OrganizationalUnitId.HasValue ? "Area" : "Company")
        )).ToList();

        return new PagedList<PayrollDeductionDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}


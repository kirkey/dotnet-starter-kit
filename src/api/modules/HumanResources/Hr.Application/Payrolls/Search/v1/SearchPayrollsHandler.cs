using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Search.v1;

/// <summary>
/// Handler for searching payroll records.
/// </summary>
public sealed class SearchPayrollsHandler(
    [FromKeyedServices("hr:payrolls")] IReadRepository<Payroll> repository)
    : IRequestHandler<SearchPayrollsRequest, PagedList<PayrollResponse>>
{
    public async Task<PagedList<PayrollResponse>> Handle(
        SearchPayrollsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchPayrollsSpec(request);
        var records = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = records.Select(payroll => new PayrollResponse
        {
            Id = payroll.Id,
            StartDate = payroll.StartDate,
            EndDate = payroll.EndDate,
            PayFrequency = payroll.PayFrequency,
            Status = payroll.Status,
            TotalGrossPay = payroll.TotalGrossPay,
            TotalTaxes = payroll.TotalTaxes,
            TotalDeductions = payroll.TotalDeductions,
            TotalNetPay = payroll.TotalNetPay,
            EmployeeCount = payroll.EmployeeCount,
            ProcessedDate = payroll.ProcessedDate,
            PostedDate = payroll.PostedDate,
            PaidDate = payroll.PaidDate,
            JournalEntryId = payroll.JournalEntryId,
            IsLocked = payroll.IsLocked,
            Notes = payroll.Notes
        }).ToList();

        return new PagedList<PayrollResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}


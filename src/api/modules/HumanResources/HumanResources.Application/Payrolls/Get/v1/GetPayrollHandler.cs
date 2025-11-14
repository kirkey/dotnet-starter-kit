using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Get.v1;

/// <summary>
/// Handler for retrieving a payroll by ID.
/// </summary>
public sealed class GetPayrollHandler(
    [FromKeyedServices("hr:payrolls")] IReadRepository<Payroll> repository)
    : IRequestHandler<GetPayrollRequest, PayrollResponse>
{
    public async Task<PayrollResponse> Handle(
        GetPayrollRequest request,
        CancellationToken cancellationToken)
    {
        var payroll = await repository
            .FirstOrDefaultAsync(new PayrollByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (payroll is null)
            throw new Exception($"Payroll not found: {request.Id}");

        return new PayrollResponse
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
        };
    }
}


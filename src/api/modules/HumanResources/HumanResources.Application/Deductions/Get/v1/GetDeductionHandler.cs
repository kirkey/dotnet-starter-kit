namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;

using Specifications;

/// <summary>
/// Handler for getting a deduction by ID.
/// </summary>
public sealed class GetDeductionHandler(
    [FromKeyedServices("hr:deductions")] IReadRepository<Deduction> repository)
    : IRequestHandler<GetDeductionRequest, DeductionResponse>
{
    public async Task<DeductionResponse> Handle(
        GetDeductionRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new DeductionByIdSpec(request.Id);
        var deduction = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);

        if (deduction is null)
            throw new DeductionNotFoundException(request.Id);

        return new DeductionResponse(
            deduction.Id,
            deduction.DeductionName,
            deduction.DeductionType,
            deduction.RecoveryMethod,
            deduction.RecoveryFixedAmount,
            deduction.RecoveryPercentage,
            deduction.InstallmentCount,
            deduction.MaxRecoveryPercentage,
            deduction.RequiresApproval,
            deduction.IsRecurring,
            deduction.IsActive,
            deduction.GlAccountCode,
            deduction.Description);
    }
}


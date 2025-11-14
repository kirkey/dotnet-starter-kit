using FSH.Starter.WebApi.HumanResources.Application.Deductions.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;

/// <summary>
/// Handler for retrieving a deduction by ID.
/// </summary>
public sealed class GetDeductionHandler(
    [FromKeyedServices("hr:deductions")] IReadRepository<PayComponent> repository)
    : IRequestHandler<GetDeductionRequest, DeductionResponse>
{
    public async Task<DeductionResponse> Handle(
        GetDeductionRequest request,
        CancellationToken cancellationToken)
    {
        var deduction = await repository
            .FirstOrDefaultAsync(new DeductionByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (deduction is null)
            throw new Exception($"Deduction not found: {request.Id}");

        return MapToResponse(deduction);
    }

    private static DeductionResponse MapToResponse(PayComponent deduction)
    {
        return new DeductionResponse
        {
            Id = deduction.Id,
            ComponentName = deduction.ComponentName,
            ComponentType = deduction.ComponentType,
            GLAccountCode = deduction.GLAccountCode,
            IsActive = deduction.IsActive,
            IsCalculated = deduction.IsCalculated,
            Description = deduction.Description
        };
    }
}


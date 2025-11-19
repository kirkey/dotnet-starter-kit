namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions;

using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions.v1;

/// <summary>
/// Endpoint routes for managing deduction master data (loans, cash advances, uniform deductions, etc).
/// </summary>
public static class DeductionEndpoints
{
    internal static void MapDeductionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/deductions")
            .WithTags("Deductions")
            .WithGroupName("Deductions Master")
            .WithDescription("Endpoints for managing deduction types per Philippines Labor Code Art 113 (loans, cash advances, equipment, damages)");

        group.MapCreateDeductionEndpoint();
        group.MapGetDeductionEndpoint();
        group.MapSearchDeductionsEndpoint();
        group.MapUpdateDeductionEndpoint();
        group.MapDeleteDeductionEndpoint();
    }
}


namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments.v1;
            
using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Get.v1;

public static class GetBenefitEnrollmentEndpoint
{
    /// <summary>
    /// Endpoint for retrieving a specific benefit enrollment by ID.
    /// </summary>
    internal static RouteHandlerBuilder MapGetBenefitEnrollmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetBenefitEnrollmentRequest(id);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .MapToApiVersion(1)
        .RequirePermission("Permissions.BenefitEnrollments.View")
        .Produces<BenefitEnrollmentResponse>()
        .WithDescription("Retrieves detailed information about a specific benefit enrollment including status and approval details.")
        .WithSummary("Gets a benefit enrollment by ID")
        .WithName(nameof(GetBenefitEnrollmentEndpoint));
    }
}

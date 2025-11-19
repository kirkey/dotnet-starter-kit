using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments;

/// <summary>
/// Endpoint routes for managing benefit enrollments.
/// </summary>
public static class BenefitEnrollmentsEndpoints
{
    internal static IEndpointRouteBuilder MapBenefitEnrollmentsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/benefit-enrollments")
            .WithTags("Benefit Enrollments")
            .WithDescription("Endpoints for managing employee benefit enrollments with approval workflows");

        group.MapCreateBenefitEnrollmentEndpoint();
        group.MapGetBenefitEnrollmentEndpoint();
        group.MapUpdateBenefitEnrollmentEndpoint();
        group.MapSearchBenefitEnrollmentsEndpoint();
        group.MapTerminateBenefitEnrollmentEndpoint();

        return app;
    }
}


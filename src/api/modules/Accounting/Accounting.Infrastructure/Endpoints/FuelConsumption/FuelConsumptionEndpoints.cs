using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FuelConsumption;

/// <summary>
/// Endpoint configuration for Fuel Consumption module.
/// </summary>
public class FuelConsumptionEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Fuel Consumption endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/fuel-consumption").WithTags("fuel-consumption");

        // TODO: Implement FuelConsumption endpoints when Application layer is ready
    }
}


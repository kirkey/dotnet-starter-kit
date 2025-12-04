using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.Staff.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Staff.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class StaffEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/staff").WithTags("Staff");

        group.MapPost("/", async (CreateStaffCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/staff/{result.Id}", result);
        })
        .WithName("CreateStaff")
        .WithSummary("Create a new staff member")
        .Produces<CreateStaffResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetStaffRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetStaff")
        .WithSummary("Get staff member by ID")
        .Produces<StaffResponse>();

    }
}

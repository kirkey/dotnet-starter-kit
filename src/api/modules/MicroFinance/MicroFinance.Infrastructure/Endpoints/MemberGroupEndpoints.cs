using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Create.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Member Groups.
/// </summary>
public static class MemberGroupEndpoints
{
    /// <summary>
    /// Maps all Member Group endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapMemberGroupEndpoints(this IEndpointRouteBuilder app)
    {
        var memberGroupsGroup = app.MapGroup("member-groups").WithTags("member-groups");

        memberGroupsGroup.MapPost("/", async (CreateMemberGroupCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/member-groups/{response.Id}", response);
            })
            .WithName("CreateMemberGroup")
            .WithSummary("Creates a new member group")
            .Produces<CreateMemberGroupResponse>(StatusCodes.Status201Created);

        return app;
    }
}

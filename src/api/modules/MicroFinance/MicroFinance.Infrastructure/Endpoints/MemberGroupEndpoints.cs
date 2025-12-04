using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.AddMember.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Search.v1;

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

        memberGroupsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetMemberGroupRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetMemberGroup")
            .WithSummary("Gets a member group by ID")
            .Produces<MemberGroupResponse>();

        memberGroupsGroup.MapPost("/search", async (SearchMemberGroupsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchMemberGroups")
            .WithSummary("Searches member groups with filters and pagination")
            .Produces<PagedList<MemberGroupResponse>>();

        memberGroupsGroup.MapPost("/{id:guid}/members", async (Guid id, AddMemberToGroupCommand command, ISender sender) =>
            {
                if (id != command.GroupId)
                {
                    return Results.BadRequest("Group ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/group-memberships/{response.MembershipId}", response);
            })
            .WithName("AddMemberToGroup")
            .WithSummary("Adds a member to the group")
            .Produces<AddMemberToGroupResponse>(StatusCodes.Status201Created);

        return app;
    }
}

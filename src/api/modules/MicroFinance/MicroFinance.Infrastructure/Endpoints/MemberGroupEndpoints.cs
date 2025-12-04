using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.AddMember.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Dissolve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Member Groups.
/// </summary>
public class MemberGroupEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Member Group endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var memberGroupsGroup = app.MapGroup("microfinance/member-groups").WithTags("member-groups");

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

        memberGroupsGroup.MapPut("/{id:guid}", async (Guid id, UpdateMemberGroupCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateMemberGroup")
            .WithSummary("Updates a member group")
            .Produces<UpdateMemberGroupResponse>();

        memberGroupsGroup.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateMemberGroupCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ActivateMemberGroup")
            .WithSummary("Activates a member group")
            .Produces<ActivateMemberGroupResponse>();

        memberGroupsGroup.MapPost("/{id:guid}/deactivate", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new DeactivateMemberGroupCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeactivateMemberGroup")
            .WithSummary("Deactivates a member group")
            .Produces<DeactivateMemberGroupResponse>();

        memberGroupsGroup.MapPost("/{id:guid}/dissolve", async (Guid id, DissolveMemberGroupCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DissolveMemberGroup")
            .WithSummary("Dissolves a member group")
            .Produces<DissolveMemberGroupResponse>();
    }
}

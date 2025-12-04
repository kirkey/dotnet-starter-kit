using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Search.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Group Memberships.
/// </summary>
public static class GroupMembershipEndpoints
{
    /// <summary>
    /// Maps all Group Membership endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapGroupMembershipEndpoints(this IEndpointRouteBuilder app)
    {
        var membershipsGroup = app.MapGroup("group-memberships").WithTags("group-memberships");

        membershipsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetGroupMembershipRequest(id));
            return Results.Ok(response);
        })
        .WithName("GetGroupMembership")
        .WithSummary("Gets a group membership by ID")
        .Produces<GroupMembershipResponse>();

        membershipsGroup.MapPost("/search", async (SearchGroupMembershipsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("SearchGroupMemberships")
        .WithSummary("Searches group memberships with pagination")
        .Produces<PagedList<GroupMembershipResponse>>();

        membershipsGroup.MapGet("/by-group/{groupId:guid}", async (Guid groupId, ISender mediator) =>
        {
            var request = new SearchGroupMembershipsCommand
            {
                GroupId = groupId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("GetGroupMembershipsByGroup")
        .WithSummary("Gets all memberships for a group")
        .Produces<PagedList<GroupMembershipResponse>>();

        membershipsGroup.MapGet("/by-member/{memberId:guid}", async (Guid memberId, ISender mediator) =>
        {
            var request = new SearchGroupMembershipsCommand
            {
                MemberId = memberId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("GetGroupMembershipsByMember")
        .WithSummary("Gets all groups a member belongs to")
        .Produces<PagedList<GroupMembershipResponse>>();

        return app;
    }
}

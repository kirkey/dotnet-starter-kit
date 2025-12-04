using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;
using Accounting.Application.Members.Create.v1;
using Accounting.Application.Members.Get.v1;
using Accounting.Application.Members.Responses;
using Accounting.Application.Members.Update.v1;
using Accounting.Application.Members.Delete.v1;
using Accounting.Application.Members.Search.v1;
using Accounting.Application.Members.Activate.v1;
using Accounting.Application.Members.Deactivate.v1;
using Accounting.Application.Members.UpdateBalance.v1;

namespace Accounting.Infrastructure.Endpoints.Member;

/// <summary>
/// Endpoint configuration for Member module.
/// </summary>
public class MemberEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/members").WithTags("members");

        // CRUD operations
        group.MapPost("/", async (CreateMemberCommand command, ISender mediator) =>
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("CreateMember")
        .WithSummary("Create member")
        .WithDescription("Creates a new member account")
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetMemberRequest(id);
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetMember")
        .WithSummary("Get member")
        .WithDescription("Retrieves a member by ID")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPut("/{id}", async (DefaultIdType id, UpdateMemberCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateMember")
        .WithSummary("Update member")
        .WithDescription("Updates a member account")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeleteMemberCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("DeleteMember")
        .WithSummary("Delete member")
        .WithDescription("Deletes an inactive member with no balance")
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchMembersRequest request, ISender mediator) =>
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchMembers")
        .WithSummary("Search members")
        .WithDescription("Search members with filters and pagination")
        .Produces<PagedList<MemberResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Workflow operations
        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new ActivateMemberCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("ActivateMember")
        .WithSummary("Activate member")
        .WithDescription("Activates a member account")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeactivateMemberCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("DeactivateMember")
        .WithSummary("Deactivate member")
        .WithDescription("Deactivates a member account")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPut("/{id}/balance", async (DefaultIdType id, UpdateMemberBalanceCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateMemberBalance")
        .WithSummary("Update member balance")
        .WithDescription("Updates a member's current balance")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}

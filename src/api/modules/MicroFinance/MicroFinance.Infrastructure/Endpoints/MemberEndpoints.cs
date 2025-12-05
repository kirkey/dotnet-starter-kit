using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Delete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Members.
/// </summary>
public class MemberEndpoints() : CarterModule("microfinance")
{
    /// <summary>
    /// Maps all Member endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var membersGroup = app.MapGroup("microfinance/members").WithTags("members");

        membersGroup.MapPost("/", async (CreateMemberCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/members/{response.Id}", response);
            })
            .WithName("CreateMember")
            .WithSummary("Creates a new member")
            .Produces<CreateMemberResponse>(StatusCodes.Status201Created);

        membersGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetMemberRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetMember")
            .WithSummary("Gets a member by ID")
            .Produces<MemberResponse>();

        membersGroup.MapPut("/{id:guid}", async (Guid id, UpdateMemberCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateMember")
            .WithSummary("Updates a member")
            .Produces<UpdateMemberResponse>();

        membersGroup.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
            {
                await sender.Send(new DeleteMemberCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteMember")
            .WithSummary("Deletes a member")
            .Produces(StatusCodes.Status204NoContent);

        membersGroup.MapPost("/search", async ([FromBody] SearchMembersCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchMembers")
            .WithSummary("Searches members with filters and pagination")
            .Produces<PagedList<MemberResponse>>();
    }
}

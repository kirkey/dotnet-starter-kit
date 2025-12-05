using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Cancel.v1;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Complete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.IssueCertificate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Schedule.v1;
using FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Start.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class StaffTrainingEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/staff-trainings").WithTags("Staff Trainings");

        group.MapPost("/", async (ScheduleStaffTrainingCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/staff-trainings/{result.Id}", result);
        })
        .WithName("ScheduleStaffTraining")
        .WithSummary("Schedule a new staff training")
        .Produces<ScheduleStaffTrainingResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetStaffTrainingRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetStaffTraining")
        .WithSummary("Get staff training by ID")
        .Produces<StaffTrainingResponse>();

        group.MapPost("/{id:guid}/start", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new StartStaffTrainingCommand(id));
            return Results.Ok(result);
        })
        .WithName("StartStaffTraining")
        .WithSummary("Start scheduled training")
        .Produces<StartStaffTrainingResponse>();

        group.MapPost("/{id:guid}/complete", async (Guid id, CompleteTrainingRequest request, ISender sender) =>
        {
            var command = new CompleteStaffTrainingCommand(id, request.Score, request.CompletionDate);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("CompleteStaffTraining")
        .WithSummary("Complete training with results")
        .Produces<CompleteStaffTrainingResponse>();

        group.MapPost("/{id:guid}/certificate", async (Guid id, IssueCertificateCommand command, ISender sender) =>
        {
            var cmd = command with { Id = id };
            var result = await sender.Send(cmd);
            return Results.Ok(result);
        })
        .WithName("IssueTrainingCertificate")
        .WithSummary("Issue certificate for completed training")
        .Produces<IssueCertificateResponse>();

        group.MapPost("/{id:guid}/cancel", async (Guid id, CancelTrainingRequest? request, ISender sender) =>
        {
            var command = new CancelStaffTrainingCommand(id, request?.Reason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("CancelStaffTraining")
        .WithSummary("Cancel scheduled training")
        .Produces<CancelStaffTrainingResponse>();

    }
}

public sealed record CompleteTrainingRequest(decimal? Score, DateOnly? CompletionDate);
public sealed record CancelTrainingRequest(string? Reason);

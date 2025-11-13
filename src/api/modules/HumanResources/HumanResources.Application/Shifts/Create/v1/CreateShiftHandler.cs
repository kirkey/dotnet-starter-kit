namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Create.v1;

public sealed class CreateShiftHandler(
    ILogger<CreateShiftHandler> logger,
    [FromKeyedServices("hr:shifts")] IRepository<Shift> repository)
    : IRequestHandler<CreateShiftCommand, CreateShiftResponse>
{
    public async Task<CreateShiftResponse> Handle(
        CreateShiftCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var shift = Shift.Create(
            request.ShiftName,
            request.StartTime,
            request.EndTime,
            request.IsOvernight);

        if (!string.IsNullOrWhiteSpace(request.Description))
            shift.Update(description: request.Description);

        await repository.AddAsync(shift, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Shift created with ID {ShiftId}, Name {ShiftName}, Hours {WorkingHours}",
            shift.Id,
            shift.ShiftName,
            shift.WorkingHours);

        return new CreateShiftResponse(shift.Id);
    }
}


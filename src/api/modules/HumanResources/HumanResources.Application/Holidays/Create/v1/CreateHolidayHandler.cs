namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;

public sealed class CreateHolidayHandler(
    ILogger<CreateHolidayHandler> logger,
    [FromKeyedServices("hr:holidays")] IRepository<Holiday> repository)
    : IRequestHandler<CreateHolidayCommand, CreateHolidayResponse>
{
    public async Task<CreateHolidayResponse> Handle(
        CreateHolidayCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var holiday = Holiday.Create(
            request.HolidayName,
            request.HolidayDate,
            request.IsPaid,
            request.IsRecurringAnnually);

        if (!string.IsNullOrWhiteSpace(request.Description))
            holiday.SetDescription(request.Description);

        await repository.AddAsync(holiday, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Holiday created with ID {HolidayId}, Name {HolidayName}, Date {HolidayDate}",
            holiday.Id,
            holiday.HolidayName,
            holiday.HolidayDate.Date);

        return new CreateHolidayResponse(holiday.Id);
    }
}


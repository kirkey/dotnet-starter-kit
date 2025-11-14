namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;

/// <summary>
/// Handler for creating a new holiday with Philippines Labor Code compliance.
/// Sets holiday type, pay rate multiplier, and regional applicability.
/// </summary>
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

        // Create holiday with basic information
        var holiday = Holiday.Create(
            request.HolidayName,
            request.HolidayDate,
            request.IsPaid,
            request.IsRecurringAnnually);

        // Set description
        if (!string.IsNullOrWhiteSpace(request.Description))
            holiday.SetDescription(request.Description);

        // Set recurring pattern if applicable
        if (request.IsRecurringAnnually)
            holiday.SetRecurring(request.HolidayDate.Month, request.HolidayDate.Day);

        // Philippines-Specific: Set holiday type and pay rate multiplier
        holiday.SetHolidayType(request.Type, request.PayRateMultiplier);

        // Philippines-Specific: Set moveable holiday properties
        if (request.IsMoveable)
            holiday.SetMoveable(request.IsMoveable, request.MoveableRule);

        // Philippines-Specific: Set regional applicability
        holiday.SetRegionalApplicability(request.IsNationwide, request.ApplicableRegions);

        await repository.AddAsync(holiday, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Holiday created: ID {HolidayId}, Name {HolidayName}, Date {HolidayDate}, " +
            "Type {Type}, PayRate {PayRate}%, Nationwide {Nationwide}",
            holiday.Id,
            holiday.HolidayName,
            holiday.HolidayDate.Date,
            request.Type,
            request.PayRateMultiplier * 100,
            request.IsNationwide);

        return new CreateHolidayResponse(holiday.Id);
    }
}


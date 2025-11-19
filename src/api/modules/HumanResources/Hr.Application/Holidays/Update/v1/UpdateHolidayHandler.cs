namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Update.v1;

/// <summary>
/// Handler for updating holiday with Philippines Labor Code compliance.
/// Supports partial updates - only provided fields will be updated.
/// </summary>
public sealed class UpdateHolidayHandler(
    ILogger<UpdateHolidayHandler> logger,
    [FromKeyedServices("hr:holidays")] IRepository<Holiday> repository)
    : IRequestHandler<UpdateHolidayCommand, UpdateHolidayResponse>
{
    public async Task<UpdateHolidayResponse> Handle(
        UpdateHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var holiday = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (holiday is null)
            throw new HolidayNotFoundException(request.Id);

        // Update basic configuration
        if (!string.IsNullOrWhiteSpace(request.HolidayName) || request.HolidayDate.HasValue || 
            request.IsPaid.HasValue || request.IsRecurringAnnually.HasValue)
        {
            holiday.Update(
                request.HolidayName,
                request.HolidayDate,
                request.IsPaid,
                request.IsRecurringAnnually);
        }

        // Update description
        if (!string.IsNullOrWhiteSpace(request.Description))
            holiday.SetDescription(request.Description);

        // Update recurring pattern if date changed
        if (request.HolidayDate.HasValue && request.IsRecurringAnnually.GetValueOrDefault(holiday.IsRecurringAnnually))
            holiday.SetRecurring(request.HolidayDate.Value.Month, request.HolidayDate.Value.Day);

        // Philippines-Specific: Update holiday type and pay rate
        if (!string.IsNullOrWhiteSpace(request.Type) || request.PayRateMultiplier.HasValue)
        {
            var type = request.Type ?? holiday.Type;
            var payRate = request.PayRateMultiplier ?? holiday.PayRateMultiplier;
            holiday.SetHolidayType(type, payRate);
        }

        // Philippines-Specific: Update moveable properties
        if (request.IsMoveable.HasValue)
            holiday.SetMoveable(request.IsMoveable.Value, request.MoveableRule);

        // Philippines-Specific: Update regional applicability
        if (request.IsNationwide.HasValue || !string.IsNullOrWhiteSpace(request.ApplicableRegions))
        {
            var isNationwide = request.IsNationwide ?? holiday.IsNationwide;
            var regions = request.ApplicableRegions ?? holiday.ApplicableRegions;
            holiday.SetRegionalApplicability(isNationwide, regions);
        }

        await repository.UpdateAsync(holiday, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Holiday {HolidayId} updated successfully. Type: {Type}, PayRate: {PayRate}%, Nationwide: {Nationwide}",
            holiday.Id,
            holiday.Type,
            holiday.PayRateMultiplier * 100,
            holiday.IsNationwide);

        return new UpdateHolidayResponse(holiday.Id);
    }
}


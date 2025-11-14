namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Update.v1;

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

        if (!string.IsNullOrWhiteSpace(request.HolidayName) || request.HolidayDate.HasValue || 
            request.IsPaid.HasValue || request.IsRecurringAnnually.HasValue)
        {
            holiday.Update(
                request.HolidayName,
                request.HolidayDate,
                request.IsPaid,
                request.IsRecurringAnnually);
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
            holiday.SetDescription(request.Description);

        await repository.UpdateAsync(holiday, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Holiday {HolidayId} updated successfully", holiday.Id);

        return new UpdateHolidayResponse(holiday.Id);
    }
}


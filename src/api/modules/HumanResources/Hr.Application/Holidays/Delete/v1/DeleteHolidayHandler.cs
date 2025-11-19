namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Delete.v1;

public sealed class DeleteHolidayHandler(
    ILogger<DeleteHolidayHandler> logger,
    [FromKeyedServices("hr:holidays")] IRepository<Holiday> repository)
    : IRequestHandler<DeleteHolidayCommand, DeleteHolidayResponse>
{
    public async Task<DeleteHolidayResponse> Handle(
        DeleteHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var holiday = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (holiday is null)
            throw new HolidayNotFoundException(request.Id);

        await repository.DeleteAsync(holiday, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Holiday {HolidayId} deleted successfully", holiday.Id);

        return new DeleteHolidayResponse(holiday.Id);
    }
}


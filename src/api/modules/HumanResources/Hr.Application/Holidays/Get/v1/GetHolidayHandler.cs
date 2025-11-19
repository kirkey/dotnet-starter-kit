using FSH.Starter.WebApi.HumanResources.Application.Holidays.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;

public sealed class GetHolidayHandler(
    [FromKeyedServices("hr:holidays")] IReadRepository<Holiday> repository)
    : IRequestHandler<GetHolidayRequest, HolidayResponse>
{
    public async Task<HolidayResponse> Handle(
        GetHolidayRequest request,
        CancellationToken cancellationToken)
    {
        var holiday = await repository
            .FirstOrDefaultAsync(new HolidayByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (holiday is null)
            throw new HolidayNotFoundException(request.Id);

        return new HolidayResponse
        {
            Id = holiday.Id,
            HolidayName = holiday.HolidayName,
            HolidayDate = holiday.HolidayDate,
            IsPaid = holiday.IsPaid,
            IsRecurringAnnually = holiday.IsRecurringAnnually,
            Description = holiday.Description,
            IsActive = holiday.IsActive
        };
    }
}


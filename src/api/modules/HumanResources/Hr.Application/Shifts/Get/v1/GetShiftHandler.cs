using FSH.Starter.WebApi.HumanResources.Application.Shifts.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;

public sealed class GetShiftHandler(
    [FromKeyedServices("hr:shifts")] IReadRepository<Shift> repository)
    : IRequestHandler<GetShiftRequest, ShiftResponse>
{
    public async Task<ShiftResponse> Handle(
        GetShiftRequest request,
        CancellationToken cancellationToken)
    {
        var shift = await repository
            .FirstOrDefaultAsync(new ShiftByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (shift is null)
            throw new ShiftNotFoundException(request.Id);

        return new ShiftResponse
        {
            Id = shift.Id,
            ShiftName = shift.ShiftName,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            IsOvernight = shift.IsOvernight,
            BreakDurationMinutes = shift.BreakDurationMinutes,
            WorkingHours = shift.WorkingHours,
            Description = shift.Description,
            IsActive = shift.IsActive
        };
    }
}


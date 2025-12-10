// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Suspend/v1/SuspendStaffHandler.cs

using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Specifications;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Suspend.v1;

/// <summary>
/// Handler for suspending a staff member.
/// </summary>
public sealed class SuspendStaffHandler(
    [FromKeyedServices("microfinance:staff")] IRepository<Domain.Staff> repository,
    ILogger<SuspendStaffHandler> logger)
    : IRequestHandler<SuspendStaffCommand, SuspendStaffResponse>
{
    public async Task<SuspendStaffResponse> Handle(SuspendStaffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var staff = await repository.FirstOrDefaultAsync(
            new StaffByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (staff is null)
        {
            throw new NotFoundException($"Staff with ID {request.Id} not found.");
        }

        if (staff.Status == Domain.Staff.StatusSuspended)
        {
            throw new InvalidOperationException("Staff is already suspended.");
        }

        if (staff.Status == Domain.Staff.StatusTerminated || staff.Status == Domain.Staff.StatusResigned)
        {
            throw new InvalidOperationException($"Cannot suspend staff with status: {staff.Status}");
        }

        staff.Suspend(request.Reason);
        await repository.UpdateAsync(staff, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Staff {StaffId} suspended", request.Id);

        return new SuspendStaffResponse(staff.Id, staff.Status);
    }
}


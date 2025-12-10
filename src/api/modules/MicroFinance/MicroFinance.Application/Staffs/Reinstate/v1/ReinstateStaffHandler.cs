// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Reinstate/v1/ReinstateStaffHandler.cs
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Staff.Specifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Reinstate.v1;

/// <summary>
/// Handler for reinstating a suspended staff member.
/// </summary>
public sealed class ReinstateStaffHandler(
    [FromKeyedServices("microfinance:staff")] IRepository<Domain.Staff> repository,
    ILogger<ReinstateStaffHandler> logger)
    : IRequestHandler<ReinstateStaffCommand, ReinstateStaffResponse>
{
    public async Task<ReinstateStaffResponse> Handle(ReinstateStaffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var staff = await repository.FirstOrDefaultAsync(
            new StaffByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (staff is null)
        {
            throw new NotFoundException($"Staff with ID {request.Id} not found.");
        }

        if (staff.Status == Domain.Staff.StatusActive)
        {
            throw new InvalidOperationException("Staff is already active.");
        }

        if (staff.Status == Domain.Staff.StatusTerminated || staff.Status == Domain.Staff.StatusResigned)
        {
            throw new InvalidOperationException($"Cannot reinstate staff with status: {staff.Status}");
        }

        staff.Reinstate();
        await repository.UpdateAsync(staff, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Staff {StaffId} reinstated", request.Id);

        return new ReinstateStaffResponse(staff.Id, staff.Status);
    }
}


// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Update/v1/UpdateStaffHandler.cs
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Staff.Specifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Update.v1;

/// <summary>
/// Handler for updating a staff member.
/// </summary>
public sealed class UpdateStaffHandler(
    [FromKeyedServices("microfinance:staff")] IRepository<Domain.Staff> repository,
    ILogger<UpdateStaffHandler> logger)
    : IRequestHandler<UpdateStaffCommand, UpdateStaffResponse>
{
    public async Task<UpdateStaffResponse> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var staff = await repository.FirstOrDefaultAsync(
            new StaffByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (staff is null)
        {
            throw new NotFoundException($"Staff with ID {request.Id} not found.");
        }

        staff.Update(
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.Phone,
            request.AlternatePhone,
            request.DateOfBirth,
            request.Gender,
            request.NationalId,
            request.Address,
            request.City,
            request.State,
            request.Country,
            request.PostalCode,
            request.EmergencyContactName,
            request.EmergencyContactPhone,
            request.PhotoUrl,
            request.Notes);

        await repository.UpdateAsync(staff, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Staff {StaffId} updated successfully", request.Id);

        return new UpdateStaffResponse(staff.Id);
    }
}


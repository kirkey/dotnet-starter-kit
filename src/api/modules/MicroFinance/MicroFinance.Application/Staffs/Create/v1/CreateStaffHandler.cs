using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StaffEntity = FSH.Starter.WebApi.MicroFinance.Domain.Staff;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Create.v1;

public sealed class CreateStaffHandler(
    [FromKeyedServices("microfinance:staff")] IRepository<StaffEntity> repository,
    ILogger<CreateStaffHandler> logger)
    : IRequestHandler<CreateStaffCommand, CreateStaffResponse>
{
    public async Task<CreateStaffResponse> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = StaffEntity.Create(
            request.EmployeeNumber,
            request.FirstName,
            request.LastName,
            request.Email,
            request.JobTitle,
            request.Role,
            request.JoiningDate,
            request.EmploymentType,
            request.BranchId,
            request.Department,
            request.UserId);

        await repository.AddAsync(staff, cancellationToken);
        logger.LogInformation("Staff {EmployeeNumber} created with ID {Id}", staff.EmployeeNumber, staff.Id);

        return new CreateStaffResponse(staff.Id, staff.EmployeeNumber, staff.FullName, staff.Role, staff.Status);
    }
}

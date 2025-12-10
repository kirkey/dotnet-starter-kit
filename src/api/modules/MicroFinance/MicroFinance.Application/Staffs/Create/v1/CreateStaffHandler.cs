namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Create.v1;

public sealed class CreateStaffHandler(
    [FromKeyedServices("microfinance:staff")] IRepository<Staff> repository,
    ILogger<CreateStaffHandler> logger)
    : IRequestHandler<CreateStaffCommand, CreateStaffResponse>
{
    public async Task<CreateStaffResponse> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = Staff.Create(
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

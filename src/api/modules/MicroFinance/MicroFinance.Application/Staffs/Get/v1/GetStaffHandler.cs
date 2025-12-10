using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Specifications;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Get.v1;

public sealed class GetStaffHandler(
    [FromKeyedServices("microfinance:staff")] IReadRepository<Staff> repository,
    ILogger<GetStaffHandler> logger)
    : IRequestHandler<GetStaffRequest, StaffResponse>
{
    public async Task<StaffResponse> Handle(GetStaffRequest request, CancellationToken cancellationToken)
    {
        var staff = await repository.FirstOrDefaultAsync(new StaffByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Staff {request.Id} not found");

        logger.LogInformation("Retrieved staff {Id}", staff.Id);

        return new StaffResponse(
            staff.Id,
            staff.EmployeeNumber,
            staff.FirstName,
            staff.LastName,
            staff.MiddleName,
            staff.FullName,
            staff.Email,
            staff.Phone,
            staff.JobTitle,
            staff.Role,
            staff.EmploymentType,
            staff.Status,
            staff.BranchId,
            staff.Department,
            staff.JoiningDate,
            staff.ConfirmationDate,
            staff.ReportingManagerId,
            staff.ReportingTo,
            staff.CanApproveLoan,
            staff.LoanApprovalLimit);
    }
}

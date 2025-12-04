using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Staff.Specifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StaffEntity = FSH.Starter.WebApi.MicroFinance.Domain.Staff;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Get.v1;

public sealed class GetStaffHandler(
    [FromKeyedServices("microfinance:staff")] IReadRepository<StaffEntity> repository,
    ILogger<GetStaffHandler> logger)
    : IRequestHandler<GetStaffRequest, StaffResponse>
{
    public async Task<StaffResponse> Handle(GetStaffRequest request, CancellationToken cancellationToken)
    {
        var staff = await repository.FirstOrDefaultAsync(new StaffByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Staff {request.Id} not found");

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

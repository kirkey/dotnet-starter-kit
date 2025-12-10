// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Search/v1/SearchStaffSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.Staff.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Search.v1;

/// <summary>
/// Specification for searching staff members.
/// </summary>
public sealed class SearchStaffSpecs : Specification<Domain.Staff, StaffResponse>
{
    public SearchStaffSpecs(SearchStaffCommand command)
    {
        Query.OrderByDescending(s => s.CreatedOn);

        if (!string.IsNullOrEmpty(command.FirstName))
        {
            Query.Where(s => s.FirstName.Contains(command.FirstName));
        }

        if (!string.IsNullOrEmpty(command.LastName))
        {
            Query.Where(s => s.LastName.Contains(command.LastName));
        }

        if (!string.IsNullOrEmpty(command.EmployeeNumber))
        {
            Query.Where(s => s.EmployeeNumber.Contains(command.EmployeeNumber));
        }

        if (!string.IsNullOrEmpty(command.Email))
        {
            Query.Where(s => s.Email.Contains(command.Email));
        }

        if (command.BranchId.HasValue)
        {
            Query.Where(s => s.BranchId == command.BranchId.Value);
        }

        if (!string.IsNullOrEmpty(command.Role))
        {
            Query.Where(s => s.Role == command.Role);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(s => s.Status == command.Status);
        }

        if (!string.IsNullOrEmpty(command.Department))
        {
            Query.Where(s => s.Department == command.Department);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(s => new StaffResponse(
            s.Id,
            s.EmployeeNumber,
            s.FirstName,
            s.LastName,
            s.MiddleName,
            s.FullName,
            s.Email,
            s.Phone,
            s.JobTitle,
            s.Role,
            s.EmploymentType,
            s.Status,
            s.BranchId,
            s.Department,
            s.JoiningDate,
            s.ConfirmationDate,
            s.ReportingManagerId,
            s.ReportingTo,
            s.CanApproveLoan,
            s.LoanApprovalLimit));
    }
}


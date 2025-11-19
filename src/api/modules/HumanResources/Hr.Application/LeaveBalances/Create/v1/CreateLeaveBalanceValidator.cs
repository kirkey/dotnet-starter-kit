namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Create.v1;

public class CreateLeaveBalanceValidator : AbstractValidator<CreateLeaveBalanceCommand>
{
    public CreateLeaveBalanceValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required");

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty().WithMessage("Leave type ID is required");

        RuleFor(x => x.Year)
            .GreaterThan(2000).WithMessage("Year must be greater than 2000")
            .LessThan(DateTime.Now.Year + 10).WithMessage("Year cannot be more than 10 years in the future");

        RuleFor(x => x.OpeningBalance)
            .GreaterThanOrEqualTo(0).WithMessage("Opening balance cannot be negative");
    }
}


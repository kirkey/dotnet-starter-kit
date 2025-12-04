using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Create.v1;

public sealed class CreateAgentBankingCommandValidator : AbstractValidator<CreateAgentBankingCommand>
{
    public CreateAgentBankingCommandValidator()
    {
        RuleFor(x => x.AgentCode)
            .NotEmpty().WithMessage("Agent code is required")
            .MaximumLength(32).WithMessage("Agent code cannot exceed 32 characters");

        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("Business name is required")
            .MaximumLength(128).WithMessage("Business name cannot exceed 128 characters");

        RuleFor(x => x.ContactName)
            .NotEmpty().WithMessage("Contact name is required")
            .MaximumLength(128).WithMessage("Contact name cannot exceed 128 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(32).WithMessage("Phone number cannot exceed 32 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(512).WithMessage("Address cannot exceed 512 characters");

        RuleFor(x => x.CommissionRate)
            .GreaterThanOrEqualTo(0).WithMessage("Commission rate must be non-negative")
            .LessThanOrEqualTo(100).WithMessage("Commission rate cannot exceed 100%");

        RuleFor(x => x.DailyTransactionLimit)
            .GreaterThan(0).WithMessage("Daily transaction limit must be positive");

        RuleFor(x => x.MonthlyTransactionLimit)
            .GreaterThan(0).WithMessage("Monthly transaction limit must be positive")
            .GreaterThanOrEqualTo(x => x.DailyTransactionLimit)
            .WithMessage("Monthly limit must be greater than or equal to daily limit");
    }
}

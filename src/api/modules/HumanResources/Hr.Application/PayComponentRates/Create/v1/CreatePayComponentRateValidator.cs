namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Create.v1;

public class CreatePayComponentRateValidator : AbstractValidator<CreatePayComponentRateCommand>
{
    public CreatePayComponentRateValidator()
    {
        // Basic validation - detailed validation can happen in domain entity creation
        RuleFor(cmd => cmd.MinAmount)
            .GreaterThanOrEqualTo(0m).WithMessage("Minimum amount cannot be negative.");

        RuleFor(cmd => cmd.Year)
            .GreaterThanOrEqualTo(2000).WithMessage("Year must be 2000 or later.")
            .LessThanOrEqualTo(2100).WithMessage("Year must be 2100 or earlier.");
    }
}




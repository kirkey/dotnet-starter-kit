namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;

public class CreateHolidayValidator : AbstractValidator<CreateHolidayCommand>
{
    public CreateHolidayValidator()
    {
        RuleFor(x => x.HolidayName)
            .NotEmpty().WithMessage("Holiday name is required.")
            .MaximumLength(100).WithMessage("Holiday name must not exceed 100 characters.");

        RuleFor(x => x.HolidayDate)
            .GreaterThanOrEqualTo(DateTime.Today.AddYears(-1)).WithMessage("Holiday date cannot be more than 1 year in the past.")
            .LessThanOrEqualTo(DateTime.Today.AddYears(5)).WithMessage("Holiday date cannot be more than 5 years in the future.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


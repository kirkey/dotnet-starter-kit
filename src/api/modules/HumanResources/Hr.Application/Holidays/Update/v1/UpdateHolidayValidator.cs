namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Update.v1;

public class UpdateHolidayValidator : AbstractValidator<UpdateHolidayCommand>
{
    public UpdateHolidayValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.HolidayName)
            .MaximumLength(128).WithMessage("Holiday name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.HolidayName));

        RuleFor(x => x.HolidayDate)
            .GreaterThanOrEqualTo(DateTime.Today.AddYears(-1)).WithMessage("Holiday date cannot be more than 1 year in the past.")
            .LessThanOrEqualTo(DateTime.Today.AddYears(5)).WithMessage("Holiday date cannot be more than 5 years in the future.")
            .When(x => x.HolidayDate.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Update.v1;

/// <summary>
/// Validator for UpdatePayComponentCommand.
/// </summary>
public class UpdatePayComponentValidator : AbstractValidator<UpdatePayComponentCommand>
{
    public UpdatePayComponentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Pay component ID is required.");

        RuleFor(x => x.ComponentName)
            .MaximumLength(128).WithMessage("Component name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ComponentName));

        RuleFor(x => x.GlAccountCode)
            .MaximumLength(64).WithMessage("GL account code must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.GlAccountCode));

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


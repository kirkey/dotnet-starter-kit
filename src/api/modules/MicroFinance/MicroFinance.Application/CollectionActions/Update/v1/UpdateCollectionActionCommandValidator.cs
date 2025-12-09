using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Update.v1;

/// <summary>
/// Validator for UpdateCollectionActionCommand.
/// </summary>
public class UpdateCollectionActionCommandValidator : AbstractValidator<UpdateCollectionActionCommand>
{
    public UpdateCollectionActionCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Collection action ID is required.");

        RuleFor(c => c.Outcome)
            .MaximumLength(CollectionActionConstants.OutcomeMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.Outcome));

        RuleFor(c => c.Description)
            .MaximumLength(CollectionActionConstants.DescriptionMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.Description));

        RuleFor(c => c.ContactMethod)
            .MaximumLength(CollectionActionConstants.ContactMethodMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.ContactMethod));

        RuleFor(c => c.PhoneNumberCalled)
            .MaximumLength(CollectionActionConstants.PhoneNumberMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.PhoneNumberCalled));

        RuleFor(c => c.ContactPerson)
            .MaximumLength(CollectionActionConstants.ContactPersonMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.ContactPerson));

        RuleFor(c => c.PromisedAmount)
            .GreaterThan(0)
            .When(c => c.PromisedAmount.HasValue)
            .WithMessage("Promised amount must be greater than 0.");

        RuleFor(c => c.DurationMinutes)
            .GreaterThan(0)
            .When(c => c.DurationMinutes.HasValue)
            .WithMessage("Duration must be greater than 0 minutes.");

        RuleFor(c => c.Latitude)
            .InclusiveBetween(-90, 90)
            .When(c => c.Latitude.HasValue)
            .WithMessage("Latitude must be between -90 and 90.");

        RuleFor(c => c.Longitude)
            .InclusiveBetween(-180, 180)
            .When(c => c.Longitude.HasValue)
            .WithMessage("Longitude must be between -180 and 180.");

        RuleFor(c => c.Notes)
            .MaximumLength(CollectionActionConstants.NotesMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.Notes));
    }
}

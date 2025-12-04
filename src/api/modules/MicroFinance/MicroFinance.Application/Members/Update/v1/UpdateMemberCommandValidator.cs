using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Update.v1;

/// <summary>
/// Validator for UpdateMemberCommand.
/// </summary>
public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(m => m.Id)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(m => m.FirstName)
            .MinimumLength(Member.FirstNameMinLength)
            .MaximumLength(Member.FirstNameMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.FirstName));

        RuleFor(m => m.LastName)
            .MinimumLength(Member.LastNameMinLength)
            .MaximumLength(Member.LastNameMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.LastName));

        RuleFor(m => m.MiddleName)
            .MaximumLength(Member.MiddleNameMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.MiddleName));

        RuleFor(m => m.Email)
            .MaximumLength(Member.EmailMaxLength)
            .EmailAddress()
            .When(m => !string.IsNullOrWhiteSpace(m.Email));

        RuleFor(m => m.PhoneNumber)
            .MaximumLength(Member.PhoneNumberMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.PhoneNumber));

        RuleFor(m => m.Gender)
            .MaximumLength(Member.GenderMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.Gender));

        RuleFor(m => m.Address)
            .MaximumLength(Member.AddressMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.Address));

        RuleFor(m => m.City)
            .MaximumLength(Member.CityMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.City));

        RuleFor(m => m.State)
            .MaximumLength(Member.StateMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.State));

        RuleFor(m => m.PostalCode)
            .MaximumLength(Member.PostalCodeMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.PostalCode));

        RuleFor(m => m.Country)
            .MaximumLength(Member.CountryMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.Country));

        RuleFor(m => m.NationalId)
            .MaximumLength(Member.NationalIdMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.NationalId));

        RuleFor(m => m.Occupation)
            .MaximumLength(Member.OccupationMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.Occupation));

        RuleFor(m => m.MonthlyIncome)
            .GreaterThanOrEqualTo(0)
            .When(m => m.MonthlyIncome.HasValue);

        RuleFor(m => m.Notes)
            .MaximumLength(Member.NotesMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.Notes));
    }
}

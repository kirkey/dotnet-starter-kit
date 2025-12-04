using FluentValidation;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Create.v1;

/// <summary>
/// Validator for CreateMemberCommand with comprehensive validation rules.
/// </summary>
public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator(
        [FromKeyedServices("microfinance:members")] IReadRepository<Member> repository)
    {
        RuleFor(m => m.MemberNumber)
            .NotEmpty()
            .WithMessage("Member number is required.")
            .MaximumLength(Member.MemberNumberMaxLength)
            .MustAsync(async (memberNumber, ct) =>
            {
                if (string.IsNullOrWhiteSpace(memberNumber)) return true;
                var existing = await repository.FirstOrDefaultAsync(
                    new MemberByMemberNumberSpec(memberNumber.Trim()), ct).ConfigureAwait(false);
                return existing is null;
            })
            .WithMessage("A member with this member number already exists.");

        RuleFor(m => m.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(Member.FirstNameMinLength)
            .WithMessage($"First name must be at least {Member.FirstNameMinLength} characters.")
            .MaximumLength(Member.FirstNameMaxLength);

        RuleFor(m => m.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(Member.LastNameMinLength)
            .WithMessage($"Last name must be at least {Member.LastNameMinLength} characters.")
            .MaximumLength(Member.LastNameMaxLength);

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
            .When(m => m.MonthlyIncome.HasValue)
            .WithMessage("Monthly income must be a non-negative value.");

        RuleFor(m => m.Notes)
            .MaximumLength(Member.NotesMaxLength)
            .When(m => !string.IsNullOrWhiteSpace(m.Notes));
    }
}

using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Create.v1;

public sealed class CreateMemberGroupCommandValidator : AbstractValidator<CreateMemberGroupCommand>
{
    public CreateMemberGroupCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(MemberGroup.CodeMaxLength)
            .WithMessage($"Code must not exceed {MemberGroup.CodeMaxLength} characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(MemberGroup.NameMinLength)
            .MaximumLength(MemberGroup.NameMaxLength)
            .WithMessage($"Name must be between {MemberGroup.NameMinLength} and {MemberGroup.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(MemberGroup.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description must not exceed {MemberGroup.DescriptionMaxLength} characters.");

        RuleFor(x => x.MeetingLocation)
            .MaximumLength(MemberGroup.MeetingLocationMaxLength)
            .When(x => !string.IsNullOrEmpty(x.MeetingLocation))
            .WithMessage($"Meeting location must not exceed {MemberGroup.MeetingLocationMaxLength} characters.");

        RuleFor(x => x.MeetingFrequency)
            .MaximumLength(MemberGroup.MeetingFrequencyMaxLength)
            .When(x => !string.IsNullOrEmpty(x.MeetingFrequency))
            .WithMessage($"Meeting frequency must not exceed {MemberGroup.MeetingFrequencyMaxLength} characters.");

        RuleFor(x => x.MeetingDay)
            .MaximumLength(MemberGroup.MeetingDayMaxLength)
            .When(x => !string.IsNullOrEmpty(x.MeetingDay))
            .WithMessage($"Meeting day must not exceed {MemberGroup.MeetingDayMaxLength} characters.");
    }
}

using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Create.v1;

public sealed class CreateLoanCollateralCommandValidator : AbstractValidator<CreateLoanCollateralCommand>
{
    public CreateLoanCollateralCommandValidator()
    {
        RuleFor(x => x.LoanId)
            .NotEmpty()
            .WithMessage("Loan ID is required.");

        RuleFor(x => x.CollateralType)
            .NotEmpty()
            .MaximumLength(LoanCollateral.CollateralTypeMaxLength)
            .Must(BeValidCollateralType)
            .WithMessage($"Collateral type must be one of: {LoanCollateral.TypeRealEstate}, {LoanCollateral.TypeVehicle}, {LoanCollateral.TypeEquipment}, {LoanCollateral.TypeInventory}, {LoanCollateral.TypeSavingsDeposit}, {LoanCollateral.TypeJewelry}, {LoanCollateral.TypeLivestock}, {LoanCollateral.TypeOther}.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(LoanCollateral.DescriptionMaxLength)
            .WithMessage($"Description is required and must not exceed {LoanCollateral.DescriptionMaxLength} characters.");

        RuleFor(x => x.EstimatedValue)
            .GreaterThan(0)
            .WithMessage("Estimated value must be greater than 0.");

        RuleFor(x => x.ForcedSaleValue)
            .GreaterThan(0)
            .When(x => x.ForcedSaleValue.HasValue)
            .WithMessage("Forced sale value must be greater than 0 if provided.");

        RuleFor(x => x.ForcedSaleValue)
            .LessThanOrEqualTo(x => x.EstimatedValue)
            .When(x => x.ForcedSaleValue.HasValue)
            .WithMessage("Forced sale value cannot exceed estimated value.");

        RuleFor(x => x.Location)
            .MaximumLength(LoanCollateral.LocationMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Location))
            .WithMessage($"Location must not exceed {LoanCollateral.LocationMaxLength} characters.");

        RuleFor(x => x.DocumentReference)
            .MaximumLength(LoanCollateral.DocumentReferenceMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.DocumentReference))
            .WithMessage($"Document reference must not exceed {LoanCollateral.DocumentReferenceMaxLength} characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(LoanCollateral.NotesMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage($"Notes must not exceed {LoanCollateral.NotesMaxLength} characters.");
    }

    private static bool BeValidCollateralType(string type) =>
        type is LoanCollateral.TypeRealEstate
            or LoanCollateral.TypeVehicle
            or LoanCollateral.TypeEquipment
            or LoanCollateral.TypeInventory
            or LoanCollateral.TypeSavingsDeposit
            or LoanCollateral.TypeJewelry
            or LoanCollateral.TypeLivestock
            or LoanCollateral.TypeOther;
}

namespace FSH.Starter.WebApi.MicroFinance.Application;

/// <summary>
/// Provides metadata for the MicroFinance application module.
/// Used for assembly registration in MediatR and FluentValidation.
/// </summary>
public static class MicroFinanceMetadata
{
    /// <summary>Gets the module name.</summary>
    public static string Name { get; set; } = "MicroFinanceApplication";
}


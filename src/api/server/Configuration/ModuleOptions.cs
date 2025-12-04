namespace FSH.Starter.WebApi.Host.Configuration;

/// <summary>
/// Configuration options for enabling/disabling modules.
/// </summary>
public class ModuleOptions
{
    /// <summary>
    /// Gets or sets the section name in appsettings.
    /// </summary>
    public const string SectionName = "ModuleOptions";

    /// <summary>
    /// Gets or sets a value indicating whether the Catalog module is enabled.
    /// </summary>
    public bool EnableCatalog { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the Todo module is enabled.
    /// </summary>
    public bool EnableTodo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the Accounting module is enabled.
    /// </summary>
    public bool EnableAccounting { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the Store module is enabled.
    /// </summary>
    public bool EnableStore { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the Human Resources module is enabled.
    /// </summary>
    public bool EnableHumanResources { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the Messaging module is enabled.
    /// </summary>
    public bool EnableMessaging { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the MicroFinance module is enabled.
    /// </summary>
    public bool EnableMicroFinance { get; set; } = false;
}


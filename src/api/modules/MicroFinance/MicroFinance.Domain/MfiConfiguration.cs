using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents MFI configuration settings stored as key-value pairs.
/// Provides runtime-configurable system parameters without code deployment.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Store system-wide configuration parameters</description></item>
///   <item><description>Enable runtime configuration changes without deployment</description></item>
///   <item><description>Maintain environment-specific settings</description></item>
///   <item><description>Support feature flags and toggles</description></item>
///   <item><description>Store integration credentials and endpoints</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Configuration settings control system behavior across modules:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Loan Settings</strong>: Max loan amount, interest caps, grace periods</description></item>
///   <item><description><strong>Savings Settings</strong>: Minimum balances, withdrawal limits</description></item>
///   <item><description><strong>Notifications</strong>: SMS gateway config, email settings</description></item>
///   <item><description><strong>Integrations</strong>: API keys, webhook URLs</description></item>
///   <item><description><strong>Security</strong>: Password policies, session timeouts</description></item>
/// </list>
/// <para><strong>Configuration Categories:</strong></para>
/// <list type="bullet">
///   <item><description><strong>General</strong>: Organization name, currency, locale</description></item>
///   <item><description><strong>Loan</strong>: Lending parameters and limits</description></item>
///   <item><description><strong>Savings</strong>: Savings product defaults</description></item>
///   <item><description><strong>Accounting</strong>: GL codes, fiscal year settings</description></item>
///   <item><description><strong>Notification</strong>: Communication settings</description></item>
///   <item><description><strong>Security</strong>: Authentication and authorization</description></item>
/// </list>
/// </remarks>
public sealed class MfiConfiguration : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int KeyMaxLength = 128;
    public const int ValueMaxLength = 2048;
    public const int CategoryMaxLength = 64;
    public const int DescriptionMaxLength = 512;
    public const int DataTypeMaxLength = 32;
    
    // Categories
    public const string CategoryGeneral = "General";
    public const string CategoryLoan = "Loan";
    public const string CategorySavings = "Savings";
    public const string CategoryAccounting = "Accounting";
    public const string CategoryNotification = "Notification";
    public const string CategorySecurity = "Security";
    public const string CategoryIntegration = "Integration";
    public const string CategoryReporting = "Reporting";
    
    // Data Types
    public const string DataTypeString = "String";
    public const string DataTypeNumber = "Number";
    public const string DataTypeBoolean = "Boolean";
    public const string DataTypeDate = "Date";
    public const string DataTypeJson = "Json";

    public string Key { get; private set; } = default!;
    public string Value { get; private set; } = default!;
    public string Category { get; private set; } = CategoryGeneral;
    public string DataType { get; private set; } = DataTypeString;
    public string? Description { get; private set; }
    public bool IsEncrypted { get; private set; }
    public bool IsEditable { get; private set; } = true;
    public bool RequiresRestart { get; private set; }
    public string? DefaultValue { get; private set; }
    public string? ValidationRules { get; private set; }
    public Guid? BranchId { get; private set; }
    public int DisplayOrder { get; private set; }

    private MfiConfiguration() { }

    public static MfiConfiguration Create(
        string key,
        string value,
        string category,
        string dataType = DataTypeString,
        string? description = null,
        bool isEditable = true,
        string? defaultValue = null)
    {
        var config = new MfiConfiguration
        {
            Key = key,
            Value = value,
            Category = category,
            DataType = dataType,
            Description = description,
            IsEditable = isEditable,
            DefaultValue = defaultValue ?? value
        };

        config.QueueDomainEvent(new MfiConfigurationCreated(config));
        return config;
    }

    public MfiConfiguration UpdateValue(string newValue)
    {
        if (!IsEditable)
            throw new InvalidOperationException("This configuration is not editable.");

        Value = newValue;
        QueueDomainEvent(new MfiConfigurationUpdated(Id, Key, newValue));
        return this;
    }

    public MfiConfiguration SetBranchSpecific(Guid branchId)
    {
        BranchId = branchId;
        return this;
    }

    public MfiConfiguration MarkAsEncrypted()
    {
        IsEncrypted = true;
        return this;
    }

    public MfiConfiguration ResetToDefault()
    {
        if (DefaultValue is not null)
        {
            Value = DefaultValue;
        }
        return this;
    }

    public MfiConfiguration Update(
        string? description = null,
        string? validationRules = null,
        int? displayOrder = null)
    {
        if (description is not null) Description = description;
        if (validationRules is not null) ValidationRules = validationRules;
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;

        return this;
    }

    public T GetTypedValue<T>()
    {
        return DataType switch
        {
            DataTypeBoolean => (T)(object)bool.Parse(Value),
            DataTypeNumber => (T)(object)decimal.Parse(Value),
            DataTypeDate => (T)(object)DateOnly.Parse(Value),
            _ => (T)(object)Value
        };
    }
}

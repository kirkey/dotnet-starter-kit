using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FSH.Framework.Core.Persistence;
public class ModuleDatabaseOptions
{
    public string Provider { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}

public class DatabaseOptions : IValidatableObject
{
    public string Provider { get; set; } = "mysql";
    public string ConnectionString { get; set; } = string.Empty;
    public string JobsConnectionString { get; set; } = string.Empty;

    // Module-specific overrides: key is the module name (e.g. "accounting", "store")
    // keep the collection read-only to satisfy analyzers while still binding via configuration
    public IDictionary<string, ModuleDatabaseOptions> ModuleConnectionStrings { get; } = new Dictionary<string, ModuleDatabaseOptions>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Require either a global connection string or at least one module connection defined
        if (string.IsNullOrEmpty(ConnectionString) && ModuleConnectionStrings.Count == 0)
        {
            yield return new ValidationResult("connection string cannot be empty; provide a global ConnectionString or at least one ModuleConnectionStrings entry.", new[] { nameof(ConnectionString) });
        }
    }
}

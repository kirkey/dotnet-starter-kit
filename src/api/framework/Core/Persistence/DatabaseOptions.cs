﻿using System.ComponentModel.DataAnnotations;

namespace FSH.Framework.Core.Persistence;
public class DatabaseOptions : IValidatableObject
{
    public string Provider { get; set; } = "mysql";
    public string ConnectionString { get; set; } = string.Empty;
    public string JobsConnectionString { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ConnectionString))
        {
            yield return new ValidationResult("connection string cannot be empty.", [nameof(ConnectionString)]);
        }
    }
}

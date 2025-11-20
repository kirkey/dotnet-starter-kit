﻿using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain;

/// <summary>
/// Represents a brand entity in the catalog domain.
/// A brand is used to categorize and group products.
/// </summary>
public class Brand : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the brand name field. (2^8 = 256)
    /// </summary>
    public const int NameMaxLength = 256;

    /// <summary>
    /// Maximum length for the brand description field. (2^11 = 2048)
    /// </summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>
    /// Maximum length for the brand notes field. (2^12 = 4096)
    /// </summary>
    public const int NotesMaxLength = 4096;

    /// <summary>
    /// Minimum length for the brand name field.
    /// </summary>
    public const int NameMinLength = 2;

    private Brand() { }

    private Brand(DefaultIdType id, string name, string? description, string? notes)
    {
        Id = id;
        ValidateAndSetName(name);
        ValidateAndSetDescription(description);
        ValidateAndSetNotes(notes);
        
        QueueDomainEvent(new BrandCreated { Brand = this });
    }

    /// <summary>
    /// Creates a new Brand instance using the factory method pattern.
    /// </summary>
    /// <param name="name">The brand name (required, 2-256 characters).</param>
    /// <param name="description">Optional brand description (max 2048 characters).</param>
    /// <param name="notes">Optional internal notes (max 4096 characters).</param>
    /// <returns>A new Brand instance.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public static Brand Create(string name, string? description = null, string? notes = null)
    {
        return new Brand(DefaultIdType.NewGuid(), name, description, notes);
    }

    /// <summary>
    /// Updates the brand information with validation and change detection.
    /// Only raises domain events if actual changes are detected.
    /// </summary>
    /// <param name="name">The updated brand name.</param>
    /// <param name="description">The updated brand description.</param>
    /// <param name="notes">The updated brand notes.</param>
    /// <returns>The updated Brand instance.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public Brand Update(string? name, string? description, string? notes)
    {
        // Track original values for event
        string originalName = Name;
        string? originalDescription = Description;
        string? originalNotes = Notes;
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(name))
        {
            string trimmedName = name.Trim();
            if (!string.Equals(Name, trimmedName, StringComparison.OrdinalIgnoreCase))
            {
                ValidateAndSetName(trimmedName);
                hasChanges = true;
            }
        }

        // Handle description update (including null/empty to clear)
        string? trimmedDescription = description?.Trim();
        if (!string.Equals(Description, trimmedDescription, StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetDescription(trimmedDescription);
            hasChanges = true;
        }

        // Handle notes update (including null/empty to clear)
        string? trimmedNotes = notes?.Trim();
        if (!string.Equals(Notes, trimmedNotes, StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetNotes(trimmedNotes);
            hasChanges = true;
        }

        // Only raise event if changes were detected
        if (hasChanges)
        {
            QueueDomainEvent(new BrandUpdated { Brand = this });
        }

        return this;
    }

    /// <summary>
    /// Validates and sets the brand name.
    /// </summary>
    /// <param name="name">The name to validate and set.</param>
    /// <exception cref="ArgumentException">Thrown when name is invalid.</exception>
    private void ValidateAndSetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Brand name cannot be empty.", nameof(name));
        }

        string trimmedName = name.Trim();

        if (trimmedName.Length < NameMinLength)
        {
            throw new ArgumentException($"Brand name must be at least {NameMinLength} characters.", nameof(name));
        }

        if (trimmedName.Length > NameMaxLength)
        {
            throw new ArgumentException($"Brand name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        Name = trimmedName;
    }

    /// <summary>
    /// Validates and sets the brand description.
    /// </summary>
    /// <param name="description">The description to validate and set.</param>
    /// <exception cref="ArgumentException">Thrown when description is invalid.</exception>
    private void ValidateAndSetDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
            return;
        }

        string trimmedDescription = description.Trim();

        if (trimmedDescription.Length > DescriptionMaxLength)
        {
            throw new ArgumentException($"Brand description cannot exceed {DescriptionMaxLength} characters.", nameof(description));
        }

        Description = trimmedDescription;
    }

    /// <summary>
    /// Validates and sets the brand notes.
    /// </summary>
    /// <param name="notes">The notes to validate and set.</param>
    /// <exception cref="ArgumentException">Thrown when notes are invalid.</exception>
    private void ValidateAndSetNotes(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            Notes = null;
            return;
        }

        string trimmedNotes = notes.Trim();

        if (trimmedNotes.Length > NotesMaxLength)
        {
            throw new ArgumentException($"Brand notes cannot exceed {NotesMaxLength} characters.", nameof(notes));
        }

        Notes = trimmedNotes;
    }
}



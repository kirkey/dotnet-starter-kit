﻿using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Todo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Todo.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the TodoItem entity.
/// Defines table schema, column constraints, indexes, and relationships.
/// </summary>
internal sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    /// <summary>
    /// Configures the TodoItem entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        
        // Configure string properties with lengths from domain constants
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(TodoItem.NameMaxLength);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(TodoItem.DescriptionMaxLength);
        
        builder.Property(x => x.Notes)
            .IsRequired()
            .HasMaxLength(TodoItem.NotesMaxLength);
        
        // Add indexes for frequently queried fields
        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Todos_Name");
    }
}

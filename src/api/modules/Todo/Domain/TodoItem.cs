﻿using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Todo.Domain.Events;

namespace FSH.Starter.WebApi.Todo.Domain;
public sealed class TodoItem : AuditableEntity, IAggregateRoot
{
    private TodoItem() { }

    private TodoItem(string name, string description, string notes)
    {
        Name = name;
        Description = description;
        Notes = notes;

        QueueDomainEvent(new TodoItemCreated(Id, Name, Description, Notes));
        TodoMetrics.Created.Add(1);
    }

    public static TodoItem Create(string name, string description, string note) =>
        new(name, description, note);

    public TodoItem Update(string? name, string? description, string? note)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && !string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(note) && !string.Equals(Notes, note, StringComparison.OrdinalIgnoreCase))
        {
            Notes = note;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new TodoItemUpdated(this));
            TodoMetrics.Updated.Add(1);
        }

        return this;
    }
}

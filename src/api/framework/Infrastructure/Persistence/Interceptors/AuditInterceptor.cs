﻿using System.Collections.ObjectModel;
using FSH.Framework.Core.Audit;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Framework.Infrastructure.Identity.Audit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FSH.Framework.Infrastructure.Persistence.Interceptors;
public class AuditInterceptor(ICurrentUser currentUser, TimeProvider timeProvider, IPublisher publisher) : SaveChangesInterceptor
{

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        await PublishAuditTrailsAsync(eventData).ConfigureAwait(false);
        return await base.SavingChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);
    }

    private async Task PublishAuditTrailsAsync(DbContextEventData eventData)
    {
        if (eventData.Context == null) return;
        eventData.Context.ChangeTracker.DetectChanges();
        var trails = new List<TrailDto>();
        var utcNow = timeProvider.GetUtcNow();
        foreach (var entry in eventData.Context.ChangeTracker.Entries<IAuditable>().Where(x => x.State is EntityState.Added or EntityState.Deleted or EntityState.Modified).ToList())
        {
            var userId = currentUser.GetUserId();
            var userName = currentUser.GetUserName();
            var trail = new TrailDto
            {
                Id = DefaultIdType.NewGuid(),
                TableName = entry.Entity.GetType().Name,
                UserId = userId,
                UserName = userName,
                DateTime = utcNow
            };

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    continue;
                }
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    trail.KeyValues[propertyName] = property.CurrentValue ?? string.Empty;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        trail.Type = TrailType.Create;
                        trail.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        trail.Type = TrailType.Delete;
                        trail.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            if (entry.Entity is ISoftDeletable && property.OriginalValue == null && property.CurrentValue != null)
                            {
                                trail.ModifiedProperties.Add(propertyName);
                                trail.Type = TrailType.Delete;
                                trail.OldValues[propertyName] = property.OriginalValue;
                                trail.NewValues[propertyName] = property.CurrentValue;
                            }
                            else if (property.OriginalValue?.Equals(property.CurrentValue) == false)
                            {
                                trail.ModifiedProperties.Add(propertyName);
                                trail.Type = TrailType.Update;
                                trail.OldValues[propertyName] = property.OriginalValue;
                                trail.NewValues[propertyName] = property.CurrentValue;
                            }
                            else
                            {
                                property.IsModified = false;
                            }
                        }
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            trails.Add(trail);
        }
        if (trails.Count == 0) return;
        var auditTrails = new Collection<AuditTrail>();
        foreach (var trail in trails)
        {
            auditTrails.Add(trail.ToAuditTrail());
        }
        await publisher.Publish(new AuditPublishedEvent(auditTrails)).ConfigureAwait(false);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
    
        var utcNow = timeProvider.GetUtcNow();
        var userId = currentUser.GetUserId();
        var userName = currentUser.GetUserName();
    
        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = utcNow;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedByUserName = userName;
                }

                entry.Entity.LastModifiedOn = utcNow;
                entry.Entity.LastModifiedBy = userId;
                entry.Entity.LastModifiedByUserName = userName;
            }
    
            if (entry is { State: EntityState.Deleted, Entity: ISoftDeletable softDelete })
            {
                softDelete.DeletedBy = userId;
                softDelete.DeletedByUserName = userName;
                softDelete.DeletedOn = utcNow;
                entry.State = EntityState.Modified;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}

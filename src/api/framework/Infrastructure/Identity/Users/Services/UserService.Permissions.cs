﻿using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.Authorization;

namespace FSH.Framework.Infrastructure.Identity.Users.Services;
internal sealed partial class UserService
{
    public async Task<List<string>?> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var permissions = await cache.GetOrSetAsync(
            GetPermissionCacheKey(userId),
            async () =>
            {
                var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);

                _ = user ?? throw new UnauthorizedException();

                var userRoles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
                var permissions = new List<string>();
                foreach (var role in await roleManager.Roles
                             .Where(r => userRoles.Contains(r.Name!))
                             .ToListAsync(cancellationToken).ConfigureAwait(false))
                {
                    permissions.AddRange(await db.RoleClaims
                        .Where(rc => rc.RoleId == role.Id && rc.ClaimType == FshClaims.Permission)
                        .Select(rc => rc.ClaimValue!)
                        .ToListAsync(cancellationToken).ConfigureAwait(false));
                }
                return permissions.Distinct().ToList();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return permissions;
    }

    public static string GetPermissionCacheKey(string userId)
    {
        return $"perm:{userId}";
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default)
    {
        var permissions = await GetPermissionsAsync(userId, cancellationToken).ConfigureAwait(false);

        return permissions?.Contains(permission) ?? false;
    }

    public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken)
    {
        return cache.RemoveAsync(GetPermissionCacheKey(userId), cancellationToken);
    }
}

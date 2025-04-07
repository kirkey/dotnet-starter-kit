using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Identity.Roles;
using FSH.Framework.Core.Identity.Roles.Features.CreateOrUpdateRole;
using FSH.Framework.Core.Identity.Roles.Features.UpdatePermissions;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Framework.Infrastructure.Identity.Persistence;
using FSH.Framework.Infrastructure.Identity.RoleClaims;
using FSH.Framework.Infrastructure.Tenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Authorization;

namespace FSH.Framework.Infrastructure.Identity.Roles;

public class RoleService(RoleManager<FshRole> roleManager,
    IdentityDbContext context,
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    ICurrentUser currentUser) : IRoleService
{
    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        return await Task.Run(() => roleManager.Roles
            .Select(role => new RoleDto { Id = role.Id, Name = role.Name!, Description = role.Description })
            .ToList()).ConfigureAwait(false);
    }

    public async Task<RoleDto?> GetRoleAsync(string id)
    {
        FshRole? role = await roleManager.FindByIdAsync(id).ConfigureAwait(false);

        _ = role ?? throw new NotFoundException("role not found");

        return new RoleDto { Id = role.Id, Name = role.Name!, Description = role.Description };
    }

    public async Task<RoleDto> CreateOrUpdateRoleAsync(CreateOrUpdateRoleCommand command)
    {
        FshRole? role = await roleManager.FindByIdAsync(command.Id).ConfigureAwait(false);

        if (role != null)
        {
            role.Name = command.Name;
            role.Description = command.Description;
            await roleManager.UpdateAsync(role).ConfigureAwait(false);
        }
        else
        {
            role = new FshRole(command.Name, command.Description);
            await roleManager.CreateAsync(role).ConfigureAwait(false);
        }

        return new RoleDto { Id = role.Id, Name = role.Name!, Description = role.Description };
    }

    public async Task DeleteRoleAsync(string id)
    {
        FshRole? role = await roleManager.FindByIdAsync(id).ConfigureAwait(false);

        _ = role ?? throw new NotFoundException("role not found");

        await roleManager.DeleteAsync(role).ConfigureAwait(false);
    }

    public async Task<RoleDto> GetWithPermissionsAsync(string id, CancellationToken cancellationToken)
    {
        var role = await GetRoleAsync(id).ConfigureAwait(false);
        _ = role ?? throw new NotFoundException("role not found");

        role.Permissions = await context.RoleClaims
            .Where(c => c.RoleId == id && c.ClaimType == FshClaims.Permission)
            .Select(c => c.ClaimValue!)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return role;
    }

    public async Task<string> UpdatePermissionsAsync(UpdatePermissionsCommand request)
    {
        var role = await roleManager.FindByIdAsync(request.RoleId).ConfigureAwait(false);
        _ = role ?? throw new NotFoundException("role not found");
        if (role.Name == FshRoles.Admin)
        {
            throw new FshException("operation not permitted");
        }

        if (multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id != TenantConstants.Root.Id)
        {
            // Remove Root Permissions if the Role is not created for Root Tenant.
            request.Permissions.RemoveAll(u => u.StartsWith("Permissions.Root.", StringComparison.InvariantCultureIgnoreCase));
        }

        var currentClaims = await roleManager.GetClaimsAsync(role).ConfigureAwait(false);

        // Remove permissions that were previously selected
        foreach (var claim in currentClaims.Where(c => !request.Permissions.Exists(p => p == c.Value)))
        {
            var result = await roleManager.RemoveClaimAsync(role, claim).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new FshException("operation failed", errors);
            }
        }

        // Add all permissions that were not previously selected
        foreach (string permission in request.Permissions.Where(c => !currentClaims.Any(p => p.Value == c)))
        {
            if (!string.IsNullOrEmpty(permission))
            {
                context.RoleClaims.Add(new FshRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = FshClaims.Permission,
                    ClaimValue = permission,
                    CreatedBy = currentUser.GetUserId().ToString()
                });
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        return "permissions updated";
    }
}

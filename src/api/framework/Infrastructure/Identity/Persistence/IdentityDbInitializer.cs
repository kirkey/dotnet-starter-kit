namespace FSH.Framework.Infrastructure.Identity.Persistence;
internal sealed class IdentityDbInitializer(
    ILogger<IdentityDbInitializer> logger,
    IdentityDbContext context,
    RoleManager<FshRole> roleManager,
    UserManager<FshUser> userManager,
    TimeProvider timeProvider,
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    IOptions<OriginOptions> originSettings) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for identity module", context.TenantInfo?.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedRolesAsync().ConfigureAwait(false);
        await SeedAdminUserAsync().ConfigureAwait(false);
    }

    private async Task SeedRolesAsync()
    {
        foreach (string roleName in FshRoles.DefaultRoles)
        {
            if (await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName).ConfigureAwait(false)
                is not { } role)
            {
                // create role
                role = new FshRole(roleName, $"{roleName} Role for {multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id} Tenant");
                await roleManager.CreateAsync(role).ConfigureAwait(false);
            }

            // Assign permissions
            if (roleName == FshRoles.Basic)
            {
                await AssignPermissionsToRoleAsync(context, FshPermissions.Basic, role).ConfigureAwait(false);
            }
            else if (roleName == FshRoles.Admin)
            {
                await AssignPermissionsToRoleAsync(context, FshPermissions.Admin, role).ConfigureAwait(false);

                if (multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id == TenantConstants.Root.Id)
                {
                    await AssignPermissionsToRoleAsync(context, FshPermissions.Root, role).ConfigureAwait(false);
                }
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(IdentityDbContext dbContext, IReadOnlyList<FshPermission> permissions, FshRole role)
    {
        var currentClaims = await roleManager.GetClaimsAsync(role).ConfigureAwait(false);
        var newClaims = permissions
            .Where(permission => !currentClaims.Any(c => c.Type == FshClaims.Permission && c.Value == permission.Name))
            .Select(permission => new FshRoleClaim
            {
                RoleId = role.Id,
                ClaimType = FshClaims.Permission,
                ClaimValue = permission.Name,
                CreatedBy = "application",
                CreatedOn = timeProvider.GetUtcNow()
            })
            .ToList();

        foreach (var claim in newClaims)
        {
            logger.LogInformation("Seeding {Role} Permission '{Permission}' for '{TenantId}' Tenant.", role.Name, claim.ClaimValue, multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
            await dbContext.RoleClaims.AddAsync(claim).ConfigureAwait(false);
        }

        // Save changes to the database context
        if (newClaims.Count != 0)
        {
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

    }

    private async Task SeedAdminUserAsync()
    {
        if (string.IsNullOrWhiteSpace(multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id) || string.IsNullOrWhiteSpace(multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail))
        {
            return;
        }

        if (await userManager.Users.FirstOrDefaultAsync(u => u.Email == multiTenantContextAccessor.MultiTenantContext.TenantInfo!.AdminEmail).ConfigureAwait(false)
            is not { } adminUser)
        {
            string adminUserName = $"{multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id.Trim()}.{FshRoles.Admin}".ToUpperInvariant();
            adminUser = new FshUser
            {
                FirstName = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id.Trim().ToUpperInvariant(),
                LastName = FshRoles.Admin,
                Email = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = (multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail!).ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                ImageUrl = new Uri(originSettings.Value.OriginUrl! + TenantConstants.Root.DefaultProfilePicture),
                IsActive = true
            };

            logger.LogInformation("Seeding Default Admin User for '{TenantId}' Tenant.", multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
            var password = new PasswordHasher<FshUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, TenantConstants.DefaultPassword);
            await userManager.CreateAsync(adminUser).ConfigureAwait(false);
        }

        // Assign role to user
        if (!await userManager.IsInRoleAsync(adminUser, FshRoles.Admin).ConfigureAwait(false))
        {
            logger.LogInformation("Assigning Admin Role to Admin User for '{TenantId}' Tenant.", multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
            await userManager.AddToRoleAsync(adminUser, FshRoles.Admin).ConfigureAwait(false);
        }
    }
}

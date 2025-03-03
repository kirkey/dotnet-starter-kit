using System.Security.Claims;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Starter.Shared.Authorization;

namespace FSH.Framework.Infrastructure.Identity.Users.Services;
public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private DefaultIdType _userId = DefaultIdType.Empty;

    public DefaultIdType GetUserId()
    {
        return IsAuthenticated()
            ? DefaultIdType.Parse(_user?.GetUserId() ?? DefaultIdType.Empty.ToString())
            : _userId;
    }

    public string? GetUserName() =>
        IsAuthenticated()
            ? _user!.GetFullName()
            : string.Empty;
    
    public string? GetUserEmail() =>
        IsAuthenticated()
            ? _user!.GetEmail()
            : string.Empty;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public string? GetTenant() =>
        IsAuthenticated() ? _user?.GetTenant() : string.Empty;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new FshException("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != DefaultIdType.Empty)
        {
            throw new FshException("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = DefaultIdType.Parse(userId);
        }
    }
}

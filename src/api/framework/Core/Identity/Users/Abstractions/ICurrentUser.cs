﻿using System.Security.Claims;

namespace FSH.Framework.Core.Identity.Users.Abstractions;
public interface ICurrentUser
{
    string? Name { get; }

    DefaultIdType GetUserId();

    string? GetUserName();
    
    string? GetUserEmail();

    string? GetTenant();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}

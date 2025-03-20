using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Accounts.Exceptions;

internal sealed class AccountForbiddenException(string code)
    : ForbiddenException($"account with code {code} already exists.");

using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.ChartOfAccounts.Exceptions;

internal sealed class ChartOfAccountForbiddenException(string code)
    : ForbiddenException($"account with code {code} already exists.");

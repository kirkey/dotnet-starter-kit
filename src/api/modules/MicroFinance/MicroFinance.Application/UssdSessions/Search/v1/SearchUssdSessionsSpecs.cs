// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/UssdSessions/Search/v1/SearchUssdSessionsSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Search.v1;

/// <summary>
/// Specification for searching USSD sessions.
/// </summary>
public sealed class SearchUssdSessionsSpecs : Specification<UssdSession, UssdSessionResponse>
{
    public SearchUssdSessionsSpecs(SearchUssdSessionsCommand command)
    {
        Query.OrderByDescending(s => s.StartedAt);

        if (!string.IsNullOrEmpty(command.PhoneNumber))
        {
            Query.Where(s => s.PhoneNumber.Contains(command.PhoneNumber));
        }

        if (command.MemberId.HasValue)
        {
            Query.Where(s => s.MemberId == command.MemberId.Value);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(s => s.Status == command.Status);
        }

        if (!string.IsNullOrEmpty(command.CurrentOperation))
        {
            Query.Where(s => s.CurrentOperation == command.CurrentOperation);
        }

        if (command.StartedFrom.HasValue)
        {
            var fromDate = command.StartedFrom.Value.ToDateTime(TimeOnly.MinValue);
            Query.Where(s => s.StartedAt >= fromDate);
        }

        if (command.StartedTo.HasValue)
        {
            var toDate = command.StartedTo.Value.ToDateTime(TimeOnly.MaxValue);
            Query.Where(s => s.StartedAt <= toDate);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(s => new UssdSessionResponse(
            s.Id,
            s.SessionId,
            s.PhoneNumber,
            s.ServiceCode,
            s.MemberId,
            s.WalletId,
            s.Status,
            s.CurrentMenu,
            s.Language,
            s.CurrentOperation,
            s.SessionData,
            s.MenuLevel,
            s.StepCount,
            s.StartedAt,
            s.EndedAt,
            s.LastActivityAt,
            s.SessionTimeoutSeconds,
            s.LastInput,
            s.LastOutput,
            s.IsAuthenticated,
            s.ErrorMessage));
    }
}


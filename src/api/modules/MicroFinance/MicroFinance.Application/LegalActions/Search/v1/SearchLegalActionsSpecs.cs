// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/LegalActions/Search/v1/SearchLegalActionsSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Search.v1;

/// <summary>
/// Specification for searching legal actions.
/// </summary>
public sealed class SearchLegalActionsSpecs : Specification<LegalAction, LegalActionResponse>
{
    public SearchLegalActionsSpecs(SearchLegalActionsCommand command)
    {
        Query.OrderByDescending(l => l.InitiatedDate);

        if (command.LoanId.HasValue)
        {
            Query.Where(l => l.LoanId == command.LoanId.Value);
        }

        if (command.MemberId.HasValue)
        {
            Query.Where(l => l.MemberId == command.MemberId.Value);
        }

        if (command.CollectionCaseId.HasValue)
        {
            Query.Where(l => l.CollectionCaseId == command.CollectionCaseId.Value);
        }

        if (!string.IsNullOrEmpty(command.ActionType))
        {
            Query.Where(l => l.ActionType == command.ActionType);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(l => l.Status == command.Status);
        }

        if (!string.IsNullOrEmpty(command.CourtName))
        {
            Query.Where(l => l.CourtName != null && l.CourtName.Contains(command.CourtName));
        }

        if (command.InitiatedDateFrom.HasValue)
        {
            Query.Where(l => l.InitiatedDate >= command.InitiatedDateFrom.Value);
        }

        if (command.InitiatedDateTo.HasValue)
        {
            Query.Where(l => l.InitiatedDate <= command.InitiatedDateTo.Value);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(l => new LegalActionResponse(
            l.Id,
            l.CollectionCaseId,
            l.LoanId,
            l.MemberId,
            l.CaseReference,
            l.ActionType,
            l.Status,
            l.InitiatedDate,
            l.FiledDate,
            l.NextHearingDate,
            l.JudgmentDate,
            l.ClosedDate,
            l.CourtName,
            l.LawyerName,
            l.ClaimAmount,
            l.JudgmentAmount,
            l.AmountRecovered,
            l.LegalCosts,
            l.CourtFees,
            l.JudgmentSummary));
    }
}


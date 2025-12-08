using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Search.v1;

public class SearchCreditBureauInquiriesSpecs : EntitiesByPaginationFilterSpec<CreditBureauInquiry, CreditBureauInquirySummaryResponse>
{
    public SearchCreditBureauInquiriesSpecs(SearchCreditBureauInquiriesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.InquiryDate, !command.HasOrderBy())
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.InquiryNumber.Contains(command.InquiryNumber!), !string.IsNullOrWhiteSpace(command.InquiryNumber))
            .Where(x => x.BureauName == command.BureauName, !string.IsNullOrWhiteSpace(command.BureauName))
            .Where(x => x.Purpose == command.Purpose, !string.IsNullOrWhiteSpace(command.Purpose))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.InquiryDate >= command.InquiryDateFrom!.Value, command.InquiryDateFrom.HasValue)
            .Where(x => x.InquiryDate <= command.InquiryDateTo!.Value, command.InquiryDateTo.HasValue)
            .Where(x => x.RequestedByUserId == command.RequestedByUserId!.Value, command.RequestedByUserId.HasValue);
}

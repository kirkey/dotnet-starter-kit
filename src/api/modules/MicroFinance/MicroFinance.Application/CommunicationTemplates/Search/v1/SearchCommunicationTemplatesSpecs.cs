using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Search.v1;

public class SearchCommunicationTemplatesSpecs : EntitiesByPaginationFilterSpec<CommunicationTemplate, CommunicationTemplateSummaryResponse>
{
    public SearchCommunicationTemplatesSpecs(SearchCommunicationTemplatesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.Code.Contains(command.Code!), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.Channel == command.Channel, !string.IsNullOrWhiteSpace(command.Channel))
            .Where(x => x.Category == command.Category, !string.IsNullOrWhiteSpace(command.Category))
            .Where(x => x.Language == command.Language, !string.IsNullOrWhiteSpace(command.Language))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.RequiresApproval == command.RequiresApproval!.Value, command.RequiresApproval.HasValue);
}

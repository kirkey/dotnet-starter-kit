using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;

public sealed class SearchBranchesSpec : Specification<Branch>
{
    public SearchBranchesSpec(SearchBranchesCommand command)
    {
        Query.OrderBy(b => b.Code);

        if (!string.IsNullOrWhiteSpace(command.Code))
            Query.Where(b => b.Code.Contains(command.Code));

        if (!string.IsNullOrWhiteSpace(command.Name))
            Query.Where(b => b.Name.Contains(command.Name));

        if (!string.IsNullOrWhiteSpace(command.BranchType))
            Query.Where(b => b.BranchType == command.BranchType);

        if (!string.IsNullOrWhiteSpace(command.Status))
            Query.Where(b => b.Status == command.Status);

        if (!string.IsNullOrWhiteSpace(command.City))
            Query.Where(b => b.City != null && b.City.Contains(command.City));

        if (!string.IsNullOrWhiteSpace(command.State))
            Query.Where(b => b.State != null && b.State.Contains(command.State));

        if (command.ParentBranchId.HasValue)
            Query.Where(b => b.ParentBranchId == command.ParentBranchId.Value);

        if (command.Filter is not null)
        {
            Query.Skip((command.Filter.PageNumber - 1) * command.Filter.PageSize)
                 .Take(command.Filter.PageSize);
        }
    }
}

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Search.v1;

public class SearchSalesOrdersCommandValidator : AbstractValidator<SearchSalesOrdersCommand>
{
    public SearchSalesOrdersCommandValidator()
    {
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
        RuleFor(x => x.OrderNumber).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.OrderNumber));
        RuleFor(x => x.Status).MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Status));
        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate!.Value)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
    }
}


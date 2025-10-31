using Accounting.Infrastructure.Endpoints.InterCompanyTransactions.v1;

namespace Accounting.Infrastructure.Endpoints.InterCompanyTransactions;

public static class InterCompanyTransactionsEndpoints
{
    internal static IEndpointRouteBuilder MapInterCompanyTransactionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/intercompany-transactions")
            .WithTags("InterCompany Transactions")
            .WithDescription("Endpoints for managing inter-company transactions")
            .MapToApiVersion(1);

        group.MapInterCompanyTransactionCreateEndpoint();
        group.MapInterCompanyTransactionGetEndpoint();
        group.MapInterCompanyTransactionSearchEndpoint();

        return app;
    }
}


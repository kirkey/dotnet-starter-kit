namespace FSH.Framework.Infrastructure.Behaviours;
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next().ConfigureAwait(false);
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        return failures.Count > 0 ? throw new ValidationException(failures) : await next().ConfigureAwait(false);
    }
}

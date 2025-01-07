using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Catalog.Domain.Exceptions;
public sealed class ProductNotFoundException(Guid id) : NotFoundException($"product with id {id} not found");

using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Catalog.Domain.Exceptions;
public sealed class ProductNotFoundException(DefaultIdType id) : NotFoundException($"product with id {id} not found");

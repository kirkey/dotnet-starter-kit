using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Catalog.Domain.Exceptions;
public sealed class BrandNotFoundException(DefaultIdType id) : NotFoundException($"brand with id {id} not found");

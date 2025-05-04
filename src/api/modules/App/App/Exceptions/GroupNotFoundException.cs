using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.App.Exceptions;

internal sealed class GroupNotFoundException(DefaultIdType id)
    : NotFoundException($"app group with id {id} not found");

using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.App.Exceptions;

internal sealed class GroupExistingException(string codeName)
    : CustomException($"app group with code/name {codeName} already exists");

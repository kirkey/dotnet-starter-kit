using System.Collections.ObjectModel;
using System.Net;

namespace FSH.Framework.Core.Exceptions;

public class ConflictException(string message)
    : FshException(message, new Collection<string>(), HttpStatusCode.Conflict);

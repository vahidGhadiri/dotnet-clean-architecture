using System;

namespace API.Presentation.Middlewares.Timeout;

public sealed class RequestTimeoutOptions
{
    public TimeSpan Timeout { get; set; }
}

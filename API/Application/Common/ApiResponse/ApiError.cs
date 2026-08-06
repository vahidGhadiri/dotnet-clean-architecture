namespace API.Application.Common;

public sealed record ApiError(
    string Message,
    string Code,
    string? Detail = null
);

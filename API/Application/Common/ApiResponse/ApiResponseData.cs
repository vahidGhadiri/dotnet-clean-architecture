namespace API.Application.Common;

public sealed record ApiResponseData<TData>(
    ApiError? Error,
    bool Success,
    TData? Data
);

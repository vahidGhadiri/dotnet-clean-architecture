namespace API.Application.Common;

public enum ServiceErrorType
{
    BadRequest,
    Unauthorized,
    NotFound,
    Conflict
}

public sealed record ServiceResult<T>(
    bool Success,
    T? Data,
    ServiceErrorType? ErrorType,
    string? ErrorCode,
    string? ErrorMessage,
    string? ErrorDetail
)
{
    public static ServiceResult<T> Ok(T data) =>
        new ServiceResult<T>(true, data, null, null, null, null);

    public static ServiceResult<T> Fail(
        ServiceErrorType type,
        string errorCode,
        string errorMessage,
        string? errorDetail = null
    ) =>
        new ServiceResult<T>(false, default, type, errorCode, errorMessage, errorDetail);
}
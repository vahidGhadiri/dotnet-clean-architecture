namespace API.Application.Common;

public sealed class ApiResponse<T>(ApiResponseData<T> response)
{
    public ApiError? Error => response.Error;
    public bool Success => response.Success;
    public T? Data => response.Data;

    public static ApiResponse<T> Ok(T data) => new ApiResponse<T>(
        new ApiResponseData<T>(
            Success: true,
            Error: null,
            Data: data
        )
    );

    public static ApiResponse<T> Fail(ApiError error) => new ApiResponse<T>(
        new ApiResponseData<T>(
            Error: error,
            Success: false,
            Data: default
        ));
}

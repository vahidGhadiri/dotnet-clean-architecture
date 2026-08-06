namespace API.Presentation.Controllers.Common;

using Microsoft.AspNetCore.Mvc;
using API.Application.Common;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult ApiResult<T>(ServiceResult<T> result)
    {
        if (result.Success) return Ok(ApiResponse<T>.Ok(result.Data!));

        var error = new ApiError(result.ErrorMessage ?? "Bad request", result.ErrorCode ?? "bad_request");

        return result.ErrorType switch
        {
            ServiceErrorType.Conflict => Conflict(ApiResponse<object>.Fail(error)),
            ServiceErrorType.Unauthorized => Unauthorized(ApiResponse<object>.Fail(error)),
            ServiceErrorType.NotFound => NotFound(ApiResponse<object>.Fail(error)),
            _ => BadRequest(ApiResponse<object>.Fail(error))
        };
    }
}
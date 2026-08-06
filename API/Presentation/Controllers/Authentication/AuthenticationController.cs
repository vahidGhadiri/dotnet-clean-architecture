namespace API.Presentation.Controllers.Authentication;

using API.Application.Authentication.UseCases;
using API.Presentation.Controllers.Common;
using API.Application.Authentication.Dtos;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

[ApiVersion(1.0)]
public class AuthenticationController(
    RegisterUserUseCase registerUserUseCase,
    RefreshTokenUseCase refreshTokenUseCase,
    LoginUserUseCase loginUserUseCase
) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(
        [FromBody] RegisterDto registerDto,
        CancellationToken cancellationToken
    ) => ApiResult(await registerUserUseCase.Handle(registerDto, cancellationToken));

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(
        [FromBody] LoginDto loginDto,
        CancellationToken cancellationToken
    )
        => ApiResult(await loginUserUseCase.Handle(loginDto, cancellationToken));

    [HttpPost("refresh")]
    public async Task<ActionResult<UserDto>> Refresh(
        [FromBody] RefreshDto refreshDto,
        CancellationToken cancellationToken
    )
        => ApiResult(await refreshTokenUseCase.Handle(refreshDto, cancellationToken));
}
namespace API.Application.Authentication.UseCases;

using API.Domain.Users;
using API.Application.Authentication.Dtos;
using API.Application.Authentication.Ports;
using API.Application.Common.Ports;
using API.Application.Authentication.Mappings;
using API.Application.Common;

public class LoginUserUseCase(
    IPasswordService passwordService,
    IUserRepository repository,
    ITokenService tokenService,
    IUnitOfWork unitOfWork
)
{
    public async Task<ServiceResult<UserDto>> Handle(LoginDto dto, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmail(dto.email, cancellationToken);

        if (user is null || !passwordService.VerifyPassword(
                passwordHash: user.PasswordHash,
                passwordSalt: user.PasswordSalt,
                password: dto.password
            ))
        {
            return InvalidCredentials();
        }


        SetRefreshToken(user);
        await repository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult<UserDto>.Ok(user.ToDto(tokenService));
    }

    private static ServiceResult<UserDto> InvalidCredentials()
        => ServiceResult<UserDto>.Fail(ServiceErrorType.Unauthorized,
            errorMessage: "Invalid email or password",
            errorCode: "invalid_credentials"
        );

    private void SetRefreshToken(AppUser user)
    {
        user.RefreshToken = tokenService.CreateRefreshToken();
        user.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);
    }
}
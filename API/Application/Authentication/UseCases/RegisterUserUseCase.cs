namespace API.Application.Authentication.UseCases;

using API.Domain.Users;
using API.Application.Authentication.Dtos;
using API.Application.Authentication.Ports;
using API.Application.Common.Ports;
using API.Application.Authentication.Mappings;
using API.Application.Common;

public class RegisterUserUseCase(
    IPasswordService passwordService,
    IUserRepository repository,
    ITokenService tokenService,
    IUnitOfWork unitOfWork
)
{
    public async Task<ServiceResult<UserDto>> Handle(RegisterDto dto, CancellationToken cancellationToken)
    {
        if (await repository.IsEmailTaken(dto.Email, cancellationToken))
            return ServiceResult<UserDto>.Fail(ServiceErrorType.Conflict,
                errorMessage: "Email already exists",
                errorCode: "email_exists"
            );

        var user = new AppUser
        {
            PasswordHash = passwordService.HashPassword(dto.Password, out var salt),
            DisplayName = dto.DisplayName,
            PasswordSalt = salt,
            Email = dto.Email
        };

        SetRefreshToken(user);
        await repository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult<UserDto>.Ok(user.ToDto(tokenService));
    }

    private void SetRefreshToken(AppUser user)
    {
        user.RefreshToken = tokenService.CreateRefreshToken();
        user.RefreshTokenExpires = DateTime.Now.AddDays(7);
    }
}
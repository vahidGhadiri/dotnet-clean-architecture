namespace API.Application.Account.UseCases;

using API.Application.Authentication.Mappings;
using API.Application.Authentication.Ports;
using API.Application.Authentication.Dtos;
using API.Application.Account.Ports;
using API.Application.Account.Dtos;
using API.Application.Common.Ports;
using API.Application.Common;

public class UpdatePasswordUseCase(
    IPasswordService passwordService,
    ICurrentUserService currentUser,
    ITokenService tokenService,
    IUserRepository repository,
    IUnitOfWork unitOfWork
)
{
    public async Task<ServiceResult<UserDto>> Handle(UpdatePasswordDto dto, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is null)
            return ServiceResult<UserDto>.Fail(ServiceErrorType.Unauthorized, "no_user", "User is not authenticated");

        var user = await repository.GetById(currentUser.UserId, cancellationToken);
        if (user is null)
            return ServiceResult<UserDto>.Fail(ServiceErrorType.NotFound, "not_found", "User not found");

        if (!passwordService.VerifyPassword(dto.OldPassword, user.PasswordHash, user.PasswordSalt))
            return ServiceResult<UserDto>.Fail(ServiceErrorType.BadRequest, "wrong_password",
                "Current password doesn't match");

        user.PasswordHash = passwordService.HashPassword(dto.NewPassword, out var salt);
        user.PasswordSalt = salt;

        await repository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult<UserDto>.Ok(user.ToDto(tokenService));
    }
}
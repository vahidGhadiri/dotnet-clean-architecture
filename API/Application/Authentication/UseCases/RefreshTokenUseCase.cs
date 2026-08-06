namespace API.Application.Authentication.UseCases;

using API.Domain.Users;
using API.Application.Authentication.Dtos;
using API.Application.Authentication.Ports;
using API.Application.Common.Ports;
using API.Application.Authentication.Mappings;
using API.Application.Common;

public class RefreshTokenUseCase(IUserRepository repository, ITokenService tokenService, IUnitOfWork unitOfWork)
{
    public async Task<ServiceResult<UserDto>> Handle(RefreshDto dto, CancellationToken cancellationToken)
    {
        var user = await repository.GetByRefreshToken(dto.RefreshToken, cancellationToken);
        if (user is null || user.RefreshTokenExpires < DateTime.Now)
            return ServiceResult<UserDto>.Fail(ServiceErrorType.Unauthorized, "invalid_refresh_token",
                "Invalid or expired refresh token");

        SetRefreshToken(user);
        await repository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult<UserDto>.Ok(user.ToDto(tokenService));
    }

    private void SetRefreshToken(AppUser user)
    {
        user.RefreshToken = tokenService.CreateRefreshToken();
        user.RefreshTokenExpires = DateTime.Now.AddDays(7);
    }
}
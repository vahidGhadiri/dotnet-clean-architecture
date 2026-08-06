using API.Domain.Users;
using API.Application.Authentication.Dtos;
using API.Application.Authentication.Ports;

namespace API.Application.Authentication.Mappings;

public static class AppUserExtensions
{
    public static UserDto ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            AccessToken = tokenService.CreateAccessToken(user),
            RefreshToken = user.RefreshToken,
            DisplayName = user.DisplayName,
            ImageUrl = user.ImageUrl,
            Email = user.Email,
            Id = user.Id,
        };
    }
}
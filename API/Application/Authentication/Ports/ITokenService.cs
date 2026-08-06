namespace API.Application.Authentication.Ports;

using API.Domain.Users;

public interface ITokenService
{
    public string CreateAccessToken(AppUser user);
    public string CreateRefreshToken();
}
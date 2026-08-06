namespace API.Application.Authentication.Ports;

public interface IPasswordService
{
    bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    byte[] HashPassword(string password, out byte[] salt);
}
namespace API.Infrastructure.Services;

using API.Application.Authentication.Ports;
using System.Security.Cryptography;
using System.Text;

public class PasswordService : IPasswordService
{
    public byte[] HashPassword(string password, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;

        return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }


    public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(
            Encoding.UTF8.GetBytes(password)
        );

        return computedHash.SequenceEqual(passwordHash);
    }
}
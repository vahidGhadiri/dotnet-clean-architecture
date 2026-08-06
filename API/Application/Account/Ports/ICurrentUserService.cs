namespace API.Application.Account.Ports;

public interface ICurrentUserService
{
    string? UserId { get; }
}

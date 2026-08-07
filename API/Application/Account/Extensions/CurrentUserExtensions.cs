namespace API.Application.Account.Extensions;

using API.Application.Account.Ports;

public static class CurrentUserExtensions
{
    public static string RequireUserId(this ICurrentUserService currentUserService)
    {
        return string.IsNullOrWhiteSpace(currentUserService.UserId)
            ? throw new UnauthorizedAccessException("not_authenticated")
            : currentUserService.UserId;
    }
}
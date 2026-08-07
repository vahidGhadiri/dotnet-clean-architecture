using API.Application.Account.Extensions;

namespace API.Application.Account.UseCases;

using API.Application.Members.Mappings;
using API.Application.Account.Ports;
using API.Application.Common.Ports;
using API.Application.Members.Dtos;
using API.Application.Common;

public class GetCurrentUserUseCase(
    ICurrentUserService currentUser,
    IUserRepository repository
)
{
    public async Task<ServiceResult<MemberDto>> Handle(CancellationToken cancellationToken)
    {
        var userId = currentUser.RequireUserId();

        var user = await repository.GetById(userId, cancellationToken);
        if (user is null)
            return ServiceResult<MemberDto>.Fail(ServiceErrorType.NotFound,
                errorMessage: "User not found",
                errorCode: "not_found"
            );

        return ServiceResult<MemberDto>.Ok(user.ToMemberDto());
    }
}
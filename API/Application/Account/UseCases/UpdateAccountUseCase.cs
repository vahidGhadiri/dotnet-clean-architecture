using API.Application.Account.Extensions;

namespace API.Application.Account.UseCases;

using API.Application.Members.Mappings;
using API.Application.Account.Ports;
using API.Application.Account.Dtos;
using API.Application.Common.Ports;
using API.Application.Members.Dtos;
using API.Application.Common;

public class UpdateAccountUseCase(
    ICurrentUserService currentUser,
    IUserRepository repository,
    IUnitOfWork unitOfWork
)
{
    public async Task<ServiceResult<MemberDto>> Handle(UpdateAccountDto dto, CancellationToken cancellationToken)
    {
        var userId = currentUser.RequireUserId();

        var user = await repository.GetById(userId, cancellationToken);
        if (user is null)
            return ServiceResult<MemberDto>.Fail(ServiceErrorType.NotFound,
                errorMessage: "User not found",
                errorCode: "not_found"
            );

        user.DisplayName = dto.DisplayName ?? user.DisplayName;
        user.Description = dto.Description ?? user.Description;
        user.BirthDate = dto.BirthDate ?? user.BirthDate;
        user.ImageUrl = dto.ImageUrl ?? user.ImageUrl;
        user.Country = dto.Country ?? user.Country;
        user.Gender = dto.Gender ?? user.Gender;
        user.City = dto.City ?? user.City;

        await repository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult<MemberDto>.Ok(user.ToMemberDto());
    }
}
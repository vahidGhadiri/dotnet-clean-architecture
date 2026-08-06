namespace API.Application.Members.UseCases;

using API.Application.Members.Mappings;
using API.Application.Common.Ports;
using API.Application.Members.Dtos;
using API.Application.Common;

public class GetMemberUseCase(IUserRepository repository)
{
    public async Task<ServiceResult<MemberDto>> Handle(string id, CancellationToken cancellationToken)
    {
        var member = await repository.GetByIdWithPhotos(id, cancellationToken);

        if (member is null)
        {
            return ServiceResult<MemberDto>.Fail(ServiceErrorType.NotFound,
                errorMessage: "member not found",
                errorCode: "not_found"
            );
        }

        return ServiceResult<MemberDto>.Ok(member.ToMemberDto());
    }
}
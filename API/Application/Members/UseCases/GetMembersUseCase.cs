namespace API.Application.Members.UseCases;

using API.Application.Members.Mappings;
using API.Application.Common.Ports;
using API.Application.Members.Dtos;
using API.Application.Common;

public class GetMembersUseCase(IUserRepository repository)
{
    public async Task<ServiceResult<IReadOnlyList<MemberDto>>> Handle(CancellationToken cancellationToken)
    {
        var members = await repository.GetAllWithPhotos(cancellationToken);

        var memberDto = members
            .Select(member => member.ToMemberDto())
            .ToList();

        return ServiceResult<IReadOnlyList<MemberDto>>.Ok(memberDto);
    }
}
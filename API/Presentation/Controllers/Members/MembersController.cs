namespace API.Presentation.Controllers.Members;

using API.Presentation.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using API.Application.Members.UseCases;
using API.Application.Members.Dtos;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

[Authorize]
[ApiVersion(1.0)]
public class MembersController(
    GetMemberPhotosUseCase getMemberPhotosUse,
    GetMembersUseCase getMembersUseCase,
    GetMemberUseCase getMemberUseCase
) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MemberDto>>> GetMembers(CancellationToken cancellationToken)
        => ApiResult(await getMembersUseCase.Handle(cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetMember(string id, CancellationToken cancellationToken)
        => ApiResult(await getMemberUseCase.Handle(id, cancellationToken));

    [HttpGet("{id}/photos")]
    public async Task<ActionResult<IReadOnlyList<PhotoDto>>> GetMemberPhotos(string id, CancellationToken cancellationToken)
        => ApiResult(await getMemberPhotosUse.Handle(id, cancellationToken));
}
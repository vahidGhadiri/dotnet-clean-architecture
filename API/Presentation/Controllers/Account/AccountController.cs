namespace API.Presentation.Controllers.Account;

using API.Presentation.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using API.Application.Account.UseCases;
using API.Application.Account.Dtos;
using API.Application.Members.Dtos;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

[ApiVersion(1.0)]
public class AccountController(
    GetCurrentUserUseCase getCurrentUserUseCase,
    UpdatePasswordUseCase updatePasswordUseCase,
    UpdateAccountUseCase updateAccountUseCase,
    UploadPhotoUseCase uploadPhotoUseCase,
    DeletePhotoUseCase deletePhotoUseCase
) : BaseApiController
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<MemberDto>> GetCurrentAccount(CancellationToken cancellationToken)
        => ApiResult(await getCurrentUserUseCase.Handle(cancellationToken));

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<MemberDto>> UpdateAccount(
        [FromBody] UpdateAccountDto accountInfo,
        CancellationToken cancellationToken
    ) => ApiResult(await updateAccountUseCase.Handle(accountInfo, cancellationToken));

    [Authorize]
    [HttpPatch("password")]
    public async Task<ActionResult> UpdatePassword(
        [FromBody] UpdatePasswordDto password,
        CancellationToken cancellationToken
    ) => ApiResult(await updatePasswordUseCase.Handle(password, cancellationToken));

    [Authorize]
    [HttpPost("photos")]
    public async Task<ActionResult<PhotoDto>> UploadPhoto(
        IFormFile file,
        CancellationToken cancellationToken
    ) => ApiResult(await uploadPhotoUseCase.Handle(file, cancellationToken));

    [Authorize]
    [HttpDelete("photos/{photoId}")]
    public async Task<ActionResult<bool>> DeletePhoto(
        int photoId,
        CancellationToken cancellationToken
    ) => ApiResult(await deletePhotoUseCase.Handle(photoId, cancellationToken));
}
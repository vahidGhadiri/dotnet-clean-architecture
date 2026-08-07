using API.Application.Account.Extensions;

namespace API.Application.Account.UseCases;

using API.Application.Common;
using API.Application.Common.Ports;
using API.Domain.Photos;
using API.Application.Account.Ports;
using API.Application.Members.Dtos;

public class UploadPhotoUseCase(
    IFileStorage fileStorage,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser
)
{
    public async Task<ServiceResult<PhotoDto>> Handle(IFormFile file, CancellationToken cancellationToken)
    {
        var userId = currentUser.RequireUserId();
        var user = await userRepository.GetById(userId, cancellationToken);
        if (user is null)
            return ServiceResult<PhotoDto>.Fail(ServiceErrorType.NotFound,
                errorMessage: "User not found",
                errorCode: "not_found"
            );

        var key = $"{userId}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        await using var stream = file.OpenReadStream();
        var url = await fileStorage.UploadAsync(key, stream, file.ContentType, cancellationToken);

        var photo = new Photo
        {
            Url = url,
            UserId = userId
        };

        user.Photos.Add(photo);
        await userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult<PhotoDto>.Ok(new PhotoDto
        {
            Id = photo.Id,
            Url = photo.Url
        });
    }
}
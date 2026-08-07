using API.Application.Account.Extensions;

namespace API.Application.Account.UseCases;

using API.Application.Common;
using API.Application.Common.Ports;
using API.Application.Account.Ports;

public class DeletePhotoUseCase(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser
)
{
    public async Task<ServiceResult<bool>> Handle(int photoId, CancellationToken cancellationToken)
    {
        var userId = currentUser.RequireUserId();

        var user = await userRepository.GetByIdWithPhotos(userId, cancellationToken);
        if (user is null)
            return ServiceResult<bool>.Fail(ServiceErrorType.NotFound,
                errorMessage: "User not found",
                errorCode: "not_found"
            );

        var photo = user.Photos.FirstOrDefault(photo => photo.Id == photoId);
        if (photo is null)
            return ServiceResult<bool>.Fail(ServiceErrorType.NotFound,
                errorMessage: "Photo not found",
                errorCode: "not_found"
            );

        user.Photos.Remove(photo);
        await userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.Ok(true);
    }
}
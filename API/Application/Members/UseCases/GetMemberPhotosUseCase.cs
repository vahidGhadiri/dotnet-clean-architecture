namespace API.Application.Members.UseCases;

using API.Application.Common.Ports;
using API.Application.Common;

public class GetMemberPhotosUseCase(IUserRepository userRepository)
{
    public async Task<ServiceResult<IReadOnlyList<PhotoDto>>> Handle(string memberId, CancellationToken cancellationToken)
    {
        var photos = await userRepository.GetPhotos(memberId, cancellationToken);

        var photoDtos = photos
            .Select(photo => photo.ToPhotoDto())
            .ToList();

        return ServiceResult<IReadOnlyList<PhotoDto>>.Ok(photoDtos);
    }
}

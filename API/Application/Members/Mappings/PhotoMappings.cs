using API.Domain.Photos;

public static class PhotoMappings
{
    public static PhotoDto ToPhotoDto(this Photo photo) => new PhotoDto
    {
        Url = photo.Url,
        Id = photo.Id
    };
}

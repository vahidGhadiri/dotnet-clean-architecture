using API.Domain.Users;

namespace API.Domain.Photos;

public class Photo
{
    public required string Url { get; set; }
    public int Id { get; set; }

    // Nav property
    public AppUser User { get; set; } = null!;
    public string UserId { get; set; } = null!;
}

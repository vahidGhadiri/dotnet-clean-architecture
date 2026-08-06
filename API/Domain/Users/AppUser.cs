namespace API.Domain.Users;

using API.Domain.Photos;

public class AppUser
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public required byte[] PasswordHash { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; init; }

    public string? RefreshToken { get; set; }
    public string? ImageUrl { get; set; }

    public Gender? Gender { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Description { get; set; }
    public DateOnly? BirthDate { get; set; }

    // Nav property
    public List<Photo> Photos { get; set; } = [];
}

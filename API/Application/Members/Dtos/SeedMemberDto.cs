using System.ComponentModel.DataAnnotations;
using API.Domain.Users;

namespace API.Application.Members.Dtos;

public class SeedMemberDto
{
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    [EmailAddress] public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public required Gender? Gender { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly BirthDate { get; set; }
    public required string Id { get; set; }
    public string? ImageUrl { get; set; }
}
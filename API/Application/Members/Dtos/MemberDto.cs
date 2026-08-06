namespace API.Application.Members.Dtos;

using API.Domain.Users;

public class MemberDto
{
    public required string DisplayName { get; set; }
    public string? Description { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateTime LastActive { get; set; }
    public required string Id { get; set; }
    public string? ImageUrl { get; set; }
    public string? Country { get; set; }
    public Gender? Gender { get; set; }
    public string? City { get; set; }
}
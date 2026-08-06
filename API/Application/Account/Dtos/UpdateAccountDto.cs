namespace API.Application.Account.Dtos;

using API.Domain.Users;

public class UpdateAccountDto
{
    public DateOnly? BirthDate { get; set; }
    public string? Description { get; set; }
    public string? DisplayName { get; set; }
    public string? ImageUrl { get; set; }
    public Gender? Gender { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
}
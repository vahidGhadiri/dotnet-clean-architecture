using System.ComponentModel.DataAnnotations;

namespace API.Application.Authentication.Dtos;

public class RegisterDto
{
    [Required] [MinLength(4)] public required string Password { get; set; } = "";
    [Required] [EmailAddress] public required string Email { get; set; } = "";
    [Required] public required string DisplayName { get; set; } = ""; 
}
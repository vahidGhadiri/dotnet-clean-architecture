using System.ComponentModel.DataAnnotations;

namespace API.Application.Authentication.Dtos;

public class LoginDto
{
    public required string password { get; set; }
    public required string email { get; set; }
}
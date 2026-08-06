namespace API.Application.Account.Dtos;

public class UpdatePasswordDto
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
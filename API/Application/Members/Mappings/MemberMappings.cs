namespace API.Application.Members.Mappings;

using API.Application.Members.Dtos;
using API.Domain.Users;

public static class MemberMappings
{
    public static MemberDto ToMemberDto(this AppUser user) => new()
    {
        DisplayName = user.DisplayName,
        Description = user.Description,
        LastActive = user.LastActive,
        BirthDate = user.BirthDate,
        ImageUrl = user.ImageUrl,
        Country = user.Country,
        Gender = user.Gender,
        City = user.City,
        Id = user.Id,
    };
}
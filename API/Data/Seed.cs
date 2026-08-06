using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using API.Application.Members.Dtos;
using API.Domain.Photos;
using API.Domain.Users;
using API.Infrastructure.Data;

namespace API.Data;

public static class Seed
{
    public static async Task SeedMembers(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;
        var memberData = await File.ReadAllTextAsync("Data/SeedData/members.json");
        var members = JsonSerializer.Deserialize<List<SeedMemberDto>>(memberData);

        if (members == null)
        {
            Console.WriteLine("No member in seed data");
            return;
        }

        var photosByMember = await LoadPhotosByMember();

        foreach (var member in members)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email,
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                CreatedAt = member.CreatedAt,
                LastActive = member.LastActive,
                Gender = member.Gender,
                Country = member.Country,
                City = member.City,
                Description = member.Description,
                BirthDate = member.BirthDate,
            };

            var photos = photosByMember.GetValueOrDefault(member.Id);
            if (photos is null or { Count: 0 })
            {
                user.Photos.Add(new Photo
                {
                    Url = member.ImageUrl!,
                    UserId = member.Id,
                });
            }
            else
            {
                foreach (var photo in photos)
                {
                    user.Photos.Add(new Photo
                    {
                        Url = photo.Url,
                        UserId = photo.UserId,
                    });
                }
            }

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }

    private static async Task<Dictionary<string, List<PhotoSeedDto>>> LoadPhotosByMember()
    {
        var photoData = await File.ReadAllTextAsync("Data/SeedData/photos.json");
        var photos = JsonSerializer.Deserialize<List<PhotoSeedDto>>(photoData);

        return photos?
            .GroupBy(photo => photo.UserId)
            .ToDictionary(group => group.Key, group => group.ToList())
            ?? [];
    }
}

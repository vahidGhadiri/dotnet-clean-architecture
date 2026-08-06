namespace API.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using API.Domain.Photos;
using API.Domain.Users;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Photo> Photos => Set<Photo>();
}
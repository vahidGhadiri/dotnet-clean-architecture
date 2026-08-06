namespace API.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using API.Application.Common.Ports;
using API.Infrastructure.Data;
using API.Domain.Photos;
using API.Domain.Users;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<AppUser?> GetByEmail(
        string email,
        CancellationToken cancellationToken = default
    ) => await context.Users.SingleOrDefaultAsync(user => user.Email.ToLower() == email.ToLower(), cancellationToken);


    public async Task<AppUser?> GetByRefreshToken(
        string refreshToken,
        CancellationToken cancellationToken = default
    ) => await context.Users.SingleOrDefaultAsync(user => user.RefreshToken == refreshToken, cancellationToken);


    public async Task<IReadOnlyList<AppUser>> GetAllWithPhotos(CancellationToken cancellationToken = default)
        => await context.Users
            .Include(user => user.Photos)
            .ToListAsync(cancellationToken);


    public async Task<bool> IsEmailTaken(
        string email,
        CancellationToken cancellationToken = default
    ) => await context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower(), cancellationToken);


    public async Task<IReadOnlyList<AppUser>> GetAll(CancellationToken cancellationToken = default)
        => await context.Users.ToListAsync(cancellationToken);


    public async Task<AppUser?> GetById(
        string id,
        CancellationToken cancellationToken = default
    ) => await context.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);


    public async Task<AppUser?> GetByIdWithPhotos(
        string id,
        CancellationToken cancellationToken = default
    ) => await context.Users
        .Include(user => user.Photos)
        .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);


    public async Task<IReadOnlyList<Photo>> GetPhotos(
        string userId,
        CancellationToken cancellationToken = default
    ) => await context.Photos
        .Where(photo => photo.UserId == userId)
        .ToListAsync(cancellationToken);


    public Task Add(AppUser user)
    {
        context.Users.Add(user);
        return Task.CompletedTask;
    }


    public Task Update(AppUser user)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }


    public Task Remove(AppUser user)
    {
        context.Users.Remove(user);
        return Task.CompletedTask;
    }
}
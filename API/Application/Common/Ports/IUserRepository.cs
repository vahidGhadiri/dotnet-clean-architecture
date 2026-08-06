namespace API.Application.Common.Ports;

using API.Domain.Photos;
using API.Domain.Users;

public interface IUserRepository : IRepository<AppUser>
{
    Task<AppUser?> GetByRefreshToken(string refreshToken, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Photo>> GetPhotos(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AppUser>> GetAllWithPhotos(CancellationToken cancellationToken = default);
    Task<AppUser?> GetByIdWithPhotos(string id, CancellationToken cancellationToken = default);
    Task<AppUser?> GetByEmail(string email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailTaken(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AppUser>> GetAll(CancellationToken cancellationToken = default);
    Task<AppUser?> GetById(string id, CancellationToken cancellationToken = default);
}
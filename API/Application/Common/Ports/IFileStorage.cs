namespace API.Application.Common.Ports;

public interface IFileStorage
{
    Task<string> UploadAsync(string key, Stream fileStream, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> GetAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}

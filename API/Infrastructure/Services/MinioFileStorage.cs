namespace API.Infrastructure.Services;

using API.Application.Common.Ports;
using Minio;
using Minio.DataModel.Args;

public class MinioFileStorage(IMinioClient minioClient, string bucketName) : IFileStorage
{
    public async Task<string> UploadAsync(string key, Stream fileStream, string contentType, CancellationToken cancellationToken = default)
    {
        var bucketExists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName), cancellationToken);

        if (!bucketExists)
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName), cancellationToken);

        await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType),
            cancellationToken);

        return $"/api/v1/files/{key}";
    }

    public async Task<Stream> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();

        await minioClient.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                }),
            cancellationToken);

        return memoryStream;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key),
            cancellationToken);
    }
}

using System.Text.Json.Serialization;

namespace API.Application.Members.Dtos;

public class PhotoSeedDto
{
    public int Id { get; set; }
    public required string Url { get; set; }
    [JsonPropertyName("MemberId")] public required string UserId { get; set; }
}
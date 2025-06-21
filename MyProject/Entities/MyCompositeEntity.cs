// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Bson.Serialization.Attributes;

namespace MyProject.Entities;

[BsonIgnoreExtraElements]
public record MyCompositeEntity
{
  [BsonElement("name")]
  public required string Name { get; set; }

  [BsonElement("displayName")]
  public string DisplayName { get; set; } = string.Empty;
}
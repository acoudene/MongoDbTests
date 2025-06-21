// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyProject.Entities;

public record MyMainEntity
{
  public ObjectId Id { get; set; }

  public List<MySubEntity> MySubs { get; set; } = Enumerable.Empty<MySubEntity>().ToList();
}

// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Bson;

namespace MyProject.Entities;

public record MyMainEntity
{
  public ObjectId Id { get; set; }

  public List<MySubEntity> MySubs { get; set; } = Enumerable.Empty<MySubEntity>().ToList();

  public MySpecialEntity? MySpecialProperty { get; set; }

  public List<MyJsonEntity> MyJsonProperties { get; set; } = Enumerable.Empty<MyJsonEntity>().ToList();

  public IList<MyPolyEntity> MyPolys { get; set; } = Enumerable.Empty<MyPolyEntity>().ToList();

  //public List<Dictionary<string, string>> MyPolys { get; set; } = Enumerable.Empty<Dictionary<string, string>>().ToList();
}

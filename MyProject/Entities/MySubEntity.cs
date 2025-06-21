// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Bson.Serialization.Attributes;

namespace MyProject.Entities;

[BsonIgnoreExtraElements]
//[BsonDiscriminator("mySub", Required = true, RootClass = true)]
public record MySubEntity : MySubEntityBase
{
  //[BsonElement("myComposite")]
  //public required MyCompositeEntity MyCompositeEntity { get; set; }

  [BsonElement("result")]
  public string? Result { get; set; }
}
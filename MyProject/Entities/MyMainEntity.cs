// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyProject.Entities;

[BsonIgnoreExtraElements]
//[BsonDiscriminator("myMain", Required = true)]
public record MyMainEntity /*: IObjectIdMongoDbEntity*/
{
  [BsonId]
  [BsonElement("_id")]
  [BsonRepresentation(representation: BsonType.ObjectId)]
  //[BsonIgnoreIfDefault]
  public ObjectId ObjectId { get; set; }

  [BsonElement("uuid")]
  //[BsonGuidRepresentation(GuidRepresentation.Standard)]
  public required Guid Id { get; set; }

  [BsonElement("mySubs")]
  public List<MySubEntity> MySubs { get; set; } = Enumerable.Empty<MySubEntity>().ToList();
}

// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Bson.Serialization.Attributes;

namespace MyProject.Entities;

[BsonIgnoreExtraElements]
//[BsonDiscriminator("field", Required = true, RootClass = true)]
public abstract record MySubEntityBase
{
 
}
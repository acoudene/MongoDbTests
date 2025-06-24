// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.EntityFrameworkCore.Extensions;
using MyProject.Entities;
using System.Linq.Expressions;

namespace MyProject.DbContexts;

public class MyDbContext : DbContext
{
  public DbSet<MyMainEntity> MyMains { get; init; }

  public MyDbContext(DbContextOptions options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder
      .Entity<MyMainEntity>(e =>
      {
        e.ToCollection("myMains");

        e.HasDiscriminator()
        .HasValue<MyMainEntity>("myMain");

        e.OwnsMany(p => p.MySubs)
        .HasElementName("mySubs")
        .Property(p => p.Result)
        .HasElementName("result");

        Expression<Func<MySpecialEntity?, Dictionary<string, string>>> polyToBson = poly => new Dictionary<string, string>()
        {
          { nameof(MyPolyEntity.Name), (poly != null) ? poly.Name : string.Empty }
        };        
        Expression<Func<Dictionary<string, string>, MySpecialEntity?>> bsonToPoly = doc => new MySpecialEntity
        {
            Name = (doc.ContainsKey("name")) ? doc["name"] : string.Empty
        };


        e.Property(p => p.MySpecialProperty)
        .HasElementName("mySpecial")
        .HasConversion(
          polyToBson,
          bsonToPoly);

      });

  }
}

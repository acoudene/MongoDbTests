// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using MyProject.Entities;
using System.Linq.Expressions;
using System.Text.Json;

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

        /// If using HasOne risk of:
        /// System.NotSupportedException : The MongoDB EF Core Provider now uses transactions to ensure all updates in a SaveChanges operation are applied together or not at all. Your current MongoDB server configuration does not support transactions and you should consider switching to a replica set or load balanced configuration. If you are sure you do not need save consistency or optimistic concurrency you can disable transactions by setting 'Database.AutoTransactionBehavior = AutoTransactionBehavior.Never' on your DbContext.
        /// ----System.NotSupportedException : Standalone servers do not support transactions.
        e.OwnsMany(p => p.MyJsonProperties)
          .HasElementName("myJsonProperties")
          .Property(p => p.Result)
          .HasElementName("result")
          .HasConversion(
           v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
           v => JsonSerializer.Deserialize<object>($"\"{v}\"", (JsonSerializerOptions?)null));
      });
  }
}

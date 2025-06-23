// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using MyProject.Entities;

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

        //Func<MyPolyEntity, Dictionary<string, string>> polyToDic = poly =>
        //{
        //  return new Dictionary<string, string>()
        //  {
        //    { "name", poly.Name }
        //  };
        //};

        //Func<Dictionary<string, string>, MyPolyEntity> dicToPoly = dic =>
        //{
        //  return new MyPolyEntity
        //  {
        //    Name = dic.TryGetValue("name", out string? name) ? name : string.Empty
        //  };
        //};

        e.Property(p => p.MyPolys)
        .HasElementName("myPolys")
        .HasConversion(
          polies => polies,
          dics => dics)
        ;

        //e.OwnsMany(p => p.MyPolys/*)*/
        //.HasElementName("myPolys")
        //.Property(p => p.Name)
        //.HasElementName("name"); 
      });

  }
}

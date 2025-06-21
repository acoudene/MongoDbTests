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

        e.OwnsMany(p => p.MyPolys)
        .HasElementName("myPolys")
        .Property(p => p.Name)
        .HasElementName("name"); 
      });
          
  }
}

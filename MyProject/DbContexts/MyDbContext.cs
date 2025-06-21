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

    modelBuilder.Entity<MyMainEntity>()
      .ToCollection("myMains")
      .HasMany(e => e.MySubs)
      .WithOne()
      ;

    modelBuilder.Entity<MyMainEntity>()
      .HasDiscriminator()
      .HasValue<MyMainEntity>("myMain");

    modelBuilder.Entity<MyMainEntity>()
      .HasKey(p => p.ObjectId);

    modelBuilder.Entity<MyMainEntity>()
      .Property(p => p.ObjectId)
      .HasElementName("_id");

    modelBuilder.Entity<MyMainEntity>()
      .Property(p => p.Id)
      .HasElementName("uuid");

    //modelBuilder.Entity<RequestFormMongoDbEntity>()
    //  .Property(p => p.Fields)
    //  .HasElementName("fields");

    //modelBuilder.Entity<FieldMongoDbEntityBase>()
    //  .HasDiscriminator()
    //  //.HasValue<FieldMongoDbEntityBase>("field")
    //  .HasValue<FieldMongoDbEntity>("customerDefined");

    //modelBuilder.Entity<FieldMongoDbEntity>()
    //  .Property(p => p.FieldTemplate)
    //  .HasElementName("fieldTemplate");

    modelBuilder.Entity<MySubEntity>()
      .Property(p => p.Result)
      .HasElementName("result");

    //modelBuilder.Entity<FieldTemplateMongoDbEntity>()
    //  .Property(p => p.Name)
    //  .HasElementName("name");

    //modelBuilder.Entity<FieldTemplateMongoDbEntity>()
    //  .Property(p => p.DisplayName)
    //  .HasElementName("displayName");
  }
}

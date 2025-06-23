// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MyProject.DbContexts;
using MyProject.Entities;
using MyProject.Repositories;
using MyProject.Tests.Data;
using Testcontainers.MongoDb;

namespace MyProject.Tests;

public class MyMainRepositoryTests : IAsyncLifetime
{
  private readonly MongoDbContainer _mongoDbContainer;
  private const string BaseDataPath = "Data";

  public MyMainRepositoryTests()
  {
    _mongoDbContainer = new MongoDbBuilder()
     .WithImage("mongo:latest")
     .WithCleanUp(true)
     .Build();
  }

  public async Task InitializeAsync()
  {
    await _mongoDbContainer.StartAsync();
  }

  public async Task DisposeAsync()
  {
    await _mongoDbContainer.StopAsync();
  }

  private MyDbContextBuilder CreateDbContextBuilder() => new MyDbContextBuilder(_mongoDbContainer.GetConnectionString());

  protected SeedData DoSeedData(SeedDataBuilder seedDataBuilder)
  {
    string connectionString = _mongoDbContainer.GetConnectionString();
    seedDataBuilder.WithConnectionString(connectionString);
    var seedData = seedDataBuilder.Build();
    string databaseName = seedData.DatabaseName;
    SeedDataHelper.ImportDataByCollection<BsonDocument>(
      connectionString,
      databaseName,
      seedData.CollectionName,
      Path.Combine(BaseDataPath, seedData.FileName));

    return seedData;
  }

  [Theory]
  [ClassData(typeof(SeedDataTheoryData))]
  public async Task GetCountAsync_count_items_from_database(SeedDataBuilder seedDataBuilder)
  {
    // Arrange
    var seedData = DoSeedData(seedDataBuilder);
    using var dbContext = CreateDbContextBuilder()
      .WithDatabaseName(seedData.DatabaseName)
      .Build();
    var repository = new MyMainRepository(dbContext);

    // Act
    int count = await repository.GetCountAsync();

    // Assert
    Assert.True(count >= 0);
  }

  [Theory]
  [ClassData(typeof(SeedDataTheoryData))]
  public async Task GetEntities_get_all_entities_from_database(SeedDataBuilder seedDataBuilder)
  {
    // Arrange
    var seedData = DoSeedData(seedDataBuilder);
    using var dbContext = CreateDbContextBuilder()
      .WithDatabaseName(seedData.DatabaseName)
      .Build();
    var repository = new MyMainRepository(dbContext);

    // Act
    var entities = await repository.GetEntities()
      .ToListAsync();

    // Assert
    Assert.NotNull(entities);
    Assert.NotEmpty(entities);
    Assert.NotEmpty(entities.SelectMany(e => e.MySubs));
    Assert.NotEmpty(entities.SelectMany(e => e.MyPolys));
  }

  [Theory]
  [ClassData(typeof(SeedDataTheoryData))]
  public async Task GetEntities_create_entities_into_database(SeedDataBuilder seedDataBuilder)
  {
    // Arrange
    var seedData = DoSeedData(seedDataBuilder);
    using var dbContext = CreateDbContextBuilder()
      .WithDatabaseName(seedData.DatabaseName)
      .Build();
    var repository = new MyMainRepository(dbContext);

    // Act
    var dbSetEntities = repository.GetEntities();
    dbSetEntities.Add(new MyMainEntity()
    {
      MySubs = [
        new MySubEntity { Result = "3"},
        new MySubEntity { Result = "4"},
      ],
      MyPolys = [
        //new MyPolyEntity { Name = "Poly1" },
        //new MyPolyEntity { Name = "Poly2" }
        new Dictionary<string, string> { { "name", "value1" } },
        new Dictionary<string, string> { { "name", "value2" } }
      ]
    });
    
    await dbContext.SaveChangesAsync();
    

    // Assert
    var entities = await dbSetEntities.ToListAsync();
    Assert.NotNull(entities);
    Assert.NotEmpty(entities);
    Assert.NotEmpty(entities.SelectMany(e => e.MySubs));
    Assert.NotEmpty(entities.SelectMany(e => e.MyPolys));
  }
}
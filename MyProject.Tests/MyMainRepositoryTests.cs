// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Bson;
using MongoDB.Driver;
using MyProject.DbContexts;
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
    var entities = repository.GetEntities()
      .ToList();

    // Assert
    Assert.NotNull(entities);
    Assert.NotEmpty(entities);
    Assert.NotEmpty(entities.SelectMany(e => e.MySubs));
  }
}
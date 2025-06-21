// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.EntityFrameworkCore;

namespace MyProject.DbContexts;

public class MyDbContextBuilder
{
  private readonly string _connectionString;
  private string? _databaseName;

  public MyDbContextBuilder(string connectionString)
  {
    if (string.IsNullOrWhiteSpace(connectionString))
      throw new ArgumentNullException(nameof(connectionString));

    _connectionString = connectionString;
  }

  public MyDbContextBuilder WithDatabaseName(string databaseName)
  {
    if (string.IsNullOrWhiteSpace(databaseName))
      throw new ArgumentException(nameof(databaseName));

    _databaseName = databaseName;
    return this;
  } 

  public MyDbContext Build()
  {
    if (string.IsNullOrWhiteSpace(_databaseName))
      throw new ArgumentException(nameof(_databaseName));

    var options = new DbContextOptionsBuilder<MyDbContext>()
      .UseMongoDB(
      _connectionString,
      _databaseName)
      .Options;
    return new MyDbContext(options);
  }
}
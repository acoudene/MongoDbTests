// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace MyProject.Tests.Data;

public class SeedDataBuilder(string DatabaseName, string CollectionName, string FileName)
{
  private string _connectionString = string.Empty;

  public SeedDataBuilder(string databaseName, string collectionName)
      : this(databaseName, collectionName, $"{databaseName}.{collectionName}.json")
  {
    if (string.IsNullOrWhiteSpace(databaseName))
      throw new ArgumentNullException(nameof(databaseName));
    if (string.IsNullOrWhiteSpace(collectionName))
      throw new ArgumentNullException(nameof(collectionName));
  }

  public SeedDataBuilder WithConnectionString(string connectionString)
  {
    if (string.IsNullOrWhiteSpace(connectionString))
      throw new ArgumentNullException(nameof(connectionString));

    _connectionString = connectionString;
    return this;
  }

  public SeedData Build()
  {
    if (string.IsNullOrWhiteSpace(_connectionString))
      throw new InvalidOperationException("Connection string must be set before building SeedData.");
    if (string.IsNullOrWhiteSpace(FileName))
      throw new InvalidOperationException("File name is missing.");

    return new SeedData(
    _connectionString,
    DatabaseName,
    CollectionName,
    FileName);
  }
}

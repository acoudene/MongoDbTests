// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace MyProject.Tests.Data;

public record SeedData(
  string ConnectionString, 
  string DatabaseName, 
  string CollectionName, 
  string FileName);

// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace MyProject.Tests.Data;

public class SeedDataTheoryData : TheoryData<SeedDataBuilder>
{
  public SeedDataTheoryData()
  {
    Add(new SeedDataBuilder(databaseName: "myDatabase", collectionName: "myMains"));
  }
}

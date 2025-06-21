// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.EntityFrameworkCore;
using MyProject.DbContexts;
using MyProject.Entities;

namespace MyProject.Repositories;

public class MyMainRepository
{
  private readonly MyDbContext _dbContext;

  public MyMainRepository(MyDbContext dbContext)
  {
    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
  }

  internal protected DbSet<MyMainEntity> GetEntities() => _dbContext.MyMains;

  /// <summary>
  /// Asynchronously retrieves the total count of entities.
  /// </summary>
  /// <remarks>This method queries the underlying data source to determine the count of entities.  The operation
  /// may be canceled by passing a cancellation token.</remarks>
  /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the total number of entities.</returns>
  public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
  {
    return await GetEntities()
      .CountAsync(cancellationToken);
  }

  public async Task<List<MyMainEntity>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    return await GetEntities()
      .ToListAsync(cancellationToken);
  }
}

using System.Linq.Expressions;

namespace AgroScan.Core.Interfaces;

/// <summary>
/// Generic repository interface for common CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Gets an entity by its ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <returns>The entity if found, null otherwise</returns>
    Task<T?> GetByIdAsync(int id);
    
    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <returns>Collection of entities</returns>
    Task<IEnumerable<T>> GetAllAsync();
    
    /// <summary>
    /// Finds entities based on a predicate
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <returns>Collection of matching entities</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    
    /// <summary>
    /// Gets the first entity matching the predicate
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <returns>The first matching entity or null</returns>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    
    /// <summary>
    /// Adds a new entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <returns>The added entity</returns>
    Task<T> AddAsync(T entity);
    
    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">Entity to update</param>
    void Update(T entity);
    
    /// <summary>
    /// Removes an entity
    /// </summary>
    /// <param name="entity">Entity to remove</param>
    void Remove(T entity);
    
    /// <summary>
    /// Saves changes to the database
    /// </summary>
    /// <returns>Number of affected records</returns>
    Task<int> SaveChangesAsync();
}

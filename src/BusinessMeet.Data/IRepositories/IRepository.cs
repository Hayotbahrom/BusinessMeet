namespace BusinessMeet.Data.IRepositories;

public interface IRepository<TEntity>
{
    public Task<TEntity> CreateAsync(TEntity entity);
    public Task<TEntity> UpdateAsync(TEntity entity);
    public Task<bool> DeleteAsync(long id);
    public Task<TEntity> SelectByIdAsync(long id);
    public IQueryable<TEntity> SelectAll();
}

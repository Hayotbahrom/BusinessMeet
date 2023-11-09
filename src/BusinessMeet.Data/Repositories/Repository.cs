using BusinessMeet.Data.DbContexts;
using BusinessMeet.Data.IRepositories;
using BusinessMeet.Domain.Commons;
using Microsoft.EntityFrameworkCore;

namespace BusinessMeet.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    AppDbContext dbContext;
    DbSet<TEntity> dbset;

    public Repository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
        dbset = dbContext.Set<TEntity>();
    }
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await dbset.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var result = await dbset.FirstOrDefaultAsync(r => r.Id == id);
        dbset.Remove(result);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public IQueryable<TEntity> SelectAll()
        => dbset;
    

    public async Task<TEntity> SelectByIdAsync(long id)
    {
        var result = await dbset.Where(r => r.Id == id).FirstOrDefaultAsync();
        return result;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var result = dbContext.Update(entity).Entity;
        await dbContext.SaveChangesAsync();
        return result;
    }
}

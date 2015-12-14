using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.DBAccess.Interfaces.SingleId;

namespace EventCorps.Helper.DBAccess
{
  public abstract class GenericRepository<T, TIdProperty> : IRepositoryAddAndDelete<T, TIdProperty>, IRepositoryFindAll<T>, IRepositoryFindSingle<T, TIdProperty>, IRepositoryUpdate<T, TIdProperty>, ICountAsync<T> where T : class, IEntity<TIdProperty>
  {
    #region Field

    #endregion

    #region Ctor

    protected GenericRepository(DbContext ctx)
    {
      DBContext = ctx;
    }
    #endregion

    #region Method
    public async Task<bool> AddAsync(T model)
    {
      if (model == null)
      {
        throw new ArgumentNullException("model");
      }

      var existingModel = await this.FindAsync(model.Id);
      if (existingModel != null)
      {
        await RemoveAsync(existingModel);
      }
      this.DBContext.Set(typeof(T)).Add(model);
      return await DBContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveAsync(TIdProperty id)
    {
      var model = await this.FindAsync(id);
      return await RemoveAsync(model);
    }

    public async Task<bool> RemoveAsync(T model)
    {
      if (model == null)
      {
        throw new ArgumentNullException("model");
      }
      this.DBContext.Set(typeof(T)).Remove(model);
      return await this.DBContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(TIdProperty id)
    {
      var all = await this.GetAllAsync();
      return all.AsEnumerable().Any(e => e.Id.Equals(id));
    }

    public async Task<IQueryable<T>> GetAllAsync()
    {
      var result = new List<T>();
      var dbSet = this.GetTypedDBSet<T>();

      if (dbSet != null)
      {
        result = await dbSet.ToListAsync();
      }
      return result.AsQueryable();
    }

    public async Task<T> FindAsync(TIdProperty id)
    {
      T result = default(T);
      var dbSet = this.GetTypedDBSet<T>();
      if (dbSet != null)
      {
        result = await dbSet.FindAsync(id);
      }
      return result;

    }

    public async Task<bool> UpdateAsync(T model)
    {
      if (model == null)
      {
        throw new ArgumentNullException("model");
      }
      this.DBContext.Entry(model).State = EntityState.Modified;
      return await this.DBContext.SaveChangesAsync() > 0;
    }

    public async Task<int> CountAsync<T>() where T : class
    {
      DbSet<T> dbset = GetTypedDBSet<T>();
      var count = await dbset.CountAsync();
      return count;
    }


    protected DbSet<T> GetTypedDBSet<T>() where T : class
    {
      return (DbSet<T>)this.DBContext
                           .GetType()
                           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           .First(x => x.PropertyType.GenericTypeArguments.Contains(typeof(T)))
                           .GetValue(this.DBContext);
    }

    #endregion

    #region Property

    protected DbContext DBContext { get; private set; }
    #endregion
  }
}
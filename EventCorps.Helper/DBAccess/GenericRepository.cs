using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.DBAccess.Interfaces.SingleId;

namespace EventCorps.Helper.DBAccess
{
    public abstract class GenericRepository<T, TIdProperty> : IRepositoryAddAndDelete<T, TIdProperty>, IRepositoryFindAll<T>, IRepositoryFindSingle<T, TIdProperty>, IRepositoryUpdate<T, TIdProperty> where T : class, IEntity<TIdProperty>
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

        public abstract Task<IQueryable<T>> GetAllAsync();

        public abstract Task<T> FindAsync(TIdProperty id);

        public async Task<bool> UpdateAsync(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            this.DBContext.Entry(model).State = EntityState.Modified;
            return await this.DBContext.SaveChangesAsync() > 0;
        }


        #endregion

        #region Property

        protected DbContext DBContext { get; private set; }
        #endregion
    }
}
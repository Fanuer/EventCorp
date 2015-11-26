using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.AuthServer.Entities;
using EventCorp.AuthServer.Models;
using EventCorp.AuthServer.Repository.Interfaces;
using EventCorp.AuthServer.Repository.Interfaces.CRUD;
using EventCorp.AuthServer.Repository.Interfaces.CRUD.SingleID;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthServer.Repository
{
    internal class ApplicationRepository : IRepository
    {
        #region Field
        private ApplicationDbContext _ctx;
        private UserManager<ApplicationUser> _userManager;
        #endregion

        #region Ctor
        public ApplicationRepository()
        {
            _ctx = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            RefreshTokens = new RefreshTokenRepository(_ctx);
            Clients = new ClientRepository(_ctx);
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
        #region DELETE
        public async Task<IdentityResult> RegisterUser(CreateUserModel userModel)
        {
            var user = new ApplicationUser()
            {
                UserName = userModel.Username
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        #endregion


        #endregion

        #region Properties

        public IRefreshTokenRepository RefreshTokens { get; private set; }
        public IClientRepository Clients { get; private set; }

        #endregion

        #region Sealed Classes

        private abstract class GenericRepository<T, TIdProperty> : IRepositoryAddAndDelete<T, TIdProperty>, IRepositoryFindAll<T>, IRepositoryFindSingle<T, TIdProperty>, IRepositoryUpdate<T, TIdProperty> where T : class, IEntity<TIdProperty>
        {
            #region Field

            protected ApplicationDbContext _ctx;

            #endregion

            #region Ctor

            protected GenericRepository(ApplicationDbContext ctx)
            {
                _ctx = ctx;
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
                this._ctx.Set(typeof(T)).Add(model);
                return await _ctx.SaveChangesAsync() > 0;
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
                this._ctx.Set(typeof(T)).Remove(model);
                return await this._ctx.SaveChangesAsync() > 0;
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
                _ctx.Entry(model).State = EntityState.Modified;
                return await this._ctx.SaveChangesAsync() > 0;
            }


            #endregion

            #region Property
            #endregion
        }

        private class ClientRepository : GenericRepository<Client, string>, IClientRepository
        {
            #region Ctor
            public ClientRepository(ApplicationDbContext ctx) : base(ctx)
            {
            }
            #endregion

            #region Methods
            public async override Task<IQueryable<Client>> GetAllAsync()
            {
                var result = await this._ctx.Clients.ToListAsync();
                return result.AsQueryable();
            }

            public override async Task<Client> FindAsync(string id)
            {
                return await this._ctx.Clients.FindAsync(id);
            }

            
            #endregion
        }

        private class RefreshTokenRepository : GenericRepository<RefreshToken, string>, IRefreshTokenRepository
        {
            #region Ctor

            public RefreshTokenRepository(ApplicationDbContext ctx) : base(ctx) { }
            #endregion
            #region Methods
            public override async Task<IQueryable<RefreshToken>> GetAllAsync()
            {
                var result = await this._ctx.RefreshTokens.ToListAsync();
                return result.AsQueryable();
            }

            public override async Task<RefreshToken> FindAsync(string id)
            {
                return await this._ctx.RefreshTokens.FindAsync(id);
            }
            #endregion


        }

        #endregion
    }
}

namespace TestWebAPI.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TestWebAPI.Entities;

    /// <summary>
    /// The generic repository.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The database context type.
    /// </typeparam>
    public class GenericRepository<TEntity, TContext> : IRepository<TEntity>, IDisposable
        where TEntity : BaseEntity
        where TContext : DbContext

    {
        /// <summary>
        /// The database set.
        /// </summary>
        protected readonly DbSet<TEntity> DbSet;

        /// <summary>
        /// The context.
        /// </summary>
        protected readonly TContext Context;

        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity,TContext}"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public GenericRepository(TContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<TEntity>();
        }

        /// <inheritdoc />
        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.DbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <inheritdoc />
        public async Task<List<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.GetAllIncluding(includeProperties);
            var results = await query.Where(predicate).ToListAsync();
            return results;
        }

        /// <inheritdoc />
        public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.Where(predicate).AsNoTracking().ToList();
        }

        /// <inheritdoc />
        public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.GetAllIncluding(includeProperties);
            var results = query.Where(predicate).AsNoTracking().ToList();
            return results;
        }

        /// <inheritdoc />
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await this.DbSet.AsNoTracking().ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> GetAsync<TKey>(TKey id)
        {
            return await this.DbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        /// <inheritdoc />
        public virtual async Task Add(TEntity entity)
        {
            await this.DbSet.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual void Update(TEntity entity)
        {
            this.DbSet.Update(entity);
        }

        /// <inheritdoc />
        public virtual void Delete(TEntity entity)
        {
            this.DbSet.Remove(entity);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing">
        /// The disposing parameter.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                }
            }

            this.disposed = true;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="includeProperties">
        /// The include properties.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        private IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = this.DbSet.AsNoTracking();
            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}

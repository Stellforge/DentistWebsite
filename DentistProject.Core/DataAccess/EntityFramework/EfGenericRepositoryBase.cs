using DentistProject.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Core.DataAccess.EntityFramework
{
    public class EfGenericRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : EntityBase, new()
        where TContext : DbContext, new()

    {
        protected virtual IQueryable<TEntity> BaseGetAll(TContext context)
        {
            return context.Set<TEntity>();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var entry = context.Entry(entity);
                entry.State = EntityState.Added;
                await context.SaveChangesAsync();
                return entry.Entity;
            }
        }

        public int Count(Expression<Func<TEntity, bool>>? filter)
        {
            using (var context = new TContext())
            {
                return (filter != null)
                    ? context.Set<TEntity>().Where(filter).Count()
                    : context.Set<TEntity>().Count();
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter)
        {
            using (var context = new TContext())
            {
                return (filter != null)
                    ? await context.Set<TEntity>().Where(filter).CountAsync()
                    : await context.Set<TEntity>().CountAsync();
            }
        }

        public async Task<TEntity> Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var entry = context.Entry(entity);
                entry.State = EntityState.Deleted;
                await context.SaveChangesAsync();
                return entry.Entity;
            }
        }

        public async Task<TEntity> Get(long id)
        {
            using (var context = new TContext())
            {
                return await BaseGetAll(context).FirstOrDefaultAsync(x => x.Id == id);
            }
        }


        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return await BaseGetAll(context).FirstOrDefaultAsync(filter);
            }
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter)
        {
            using (var context = new TContext())
            {
                return (filter!=null)
                    ?await BaseGetAll(context).Where(filter).ToListAsync()
                    :await BaseGetAll(context).ToListAsync();
            }
        }

        public async Task<TEntity> SoftDelete(TEntity entity)
        {
            using (var context = new TContext())
            {
                
                entity.IsDeleted = true;


                var entry = context.Entry(entity);
                entry.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return entry.Entity;
            }
        }

        public async Task<TEntity> SoftDelete(long id)
        {
            using (var context = new TContext())
            {

                var entity = await context.Set<TEntity>().FindAsync(id);

                entity.IsDeleted = true;

                var entry = context.Entry(entity);
                entry.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return entry.Entity;
            }
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var entry = context.Entry(entity);
                entry.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return entry.Entity;
            }
        }
    }
}

using DentistProject.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Core.DataAccess
{
    public interface  IEntityRepository<T>
        where T : EntityBase, new()
    {
        public int Count(Expression<Func<T, bool>>? filter);
        public Task<int> CountAsync(Expression<Func<T, bool>>? filter);
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(T entity);
        public Task<T> SoftDelete(T entity);
        public Task<T> SoftDelete(long id);
        public Task<T> Get(long id);
        public Task<T> Get(Expression<Func<T,bool>> filter);
        public Task<List<T>> GetAll(Expression<Func<T,bool>>? filter);
        
    }
}

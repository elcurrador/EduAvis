using System.Linq.Expressions;

namespace EduAvis.Service
{
    public interface IGenericService<T> where T : class
    {
        Task<bool> AddAsync(T entity);
       
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}

using System.Linq.Expressions;

namespace Portal.Usuario.Core.Interfaces
{
    public interface IApplicationDbRepository<T>
    {
        Task<List<T>> List(Expression<Func<T, bool>> expression);
        Task Create(T entity); 
        Task Save();
    }
}
using Microsoft.EntityFrameworkCore;
using Portal.Usuario.Core.Interfaces;
using System.Linq.Expressions;

namespace Portal.Usuario.Infrastructure.DatabaseServices.Repository
{
    public class ApplicationDbRepository<T> : IApplicationDbRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        public ApplicationDbRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> List(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task Create(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

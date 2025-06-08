using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GoodBurger.Repositories.IRepositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}

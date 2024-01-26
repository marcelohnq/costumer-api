using ProvaPub.Models;
using System.Linq.Expressions;

namespace ProvaPub.Repository
{
    public interface IGenericDbContext<TEntity>
    {
        Task<EntityList<TEntity>> GetAll();
        Task<EntityList<TEntity>> GetPagination(int page, int size = 10);
        Task<TEntity?> Get(int id);
        Task<int> Count(Expression<Func<TEntity, bool>> expression);
    }
}

using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using System.Linq.Expressions;

namespace ProvaPub.Repository
{
    public interface IGenericDbContext<TEntity>
    {
        Task<GenericList<TEntity>> GetAll();
        Task<TEntity?> Get(int id);
        Task<int> Count(Expression<Func<TEntity, bool>> expression);
    }
}

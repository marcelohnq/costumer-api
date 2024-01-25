using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using System.Linq.Expressions;

namespace ProvaPub.Repository
{
    public class GenericDbContext<TEntity> : IGenericDbContext<TEntity> where TEntity : class
    {
        private readonly TestDbContext _ctx;
        private readonly DbSet<TEntity> _dbSet;

        public GenericDbContext(TestDbContext dbContext)
        {
            _ctx = dbContext;
            _dbSet = _ctx.Set<TEntity>();
        }

        public async Task<GenericList<TEntity>> GetAll()
        {
            List<TEntity> list = await _dbSet.ToListAsync();
            return new(list) { HasNext = false, TotalCount = list.Count };
        }

        public async Task<TEntity?> Get(int id) => await _dbSet.FindAsync(id);

        public async Task<int> Count(Expression<Func<TEntity, bool>> expression) => await _dbSet.CountAsync(expression);
    }
}

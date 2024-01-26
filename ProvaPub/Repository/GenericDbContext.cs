using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using System.Linq.Expressions;

namespace ProvaPub.Repository
{
    public class GenericDbContext<TEntity> : IGenericDbContext<TEntity> where TEntity : Entity
    {
        private readonly TestDbContext _ctx;
        private readonly DbSet<TEntity> _dbSet;

        public GenericDbContext(TestDbContext dbContext)
        {
            _ctx = dbContext;
            _dbSet = _ctx.Set<TEntity>();
        }

        public async Task<EntityList<TEntity>> GetAll()
        {
            List<TEntity> list = await _dbSet.ToListAsync();
            return new(list) { HasNext = false, TotalCount = list.Count };
        }

        public async Task<EntityList<TEntity>> GetPagination(int page, int size = 10)
        {
            int total = _dbSet.AsQueryable().Count();
            int current = (page - 1) * size;
            int last = page * size;

            List<TEntity> list = await _dbSet
                .OrderBy(e => e.Id)
                .Where(e => e.Id > current)
                .Take(size)
                .ToListAsync();

            return new(list) { HasNext = last < total, TotalCount = total };
        }

        public async Task<TEntity?> Get(int id) => await _dbSet.FindAsync(id);

        public async Task<int> Count(Expression<Func<TEntity, bool>> expression) => await _dbSet.CountAsync(expression);
    }
}

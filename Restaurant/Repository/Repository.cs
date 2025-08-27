using Microsoft.EntityFrameworkCore;
using Restaurant.Data;

namespace Restaurant.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<TKey>(TKey id, string propertyName, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;
            if (options.HasWhere) query = query.Where(options.Where);
            if (options.HasOrderBy) query = query.OrderBy(options.OrderBy);
            foreach (var item in options.GetIncludes()) query = query.Include(item);

            query = query.Where(e => EF.Property<TKey>(e, propertyName).Equals(id));

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;
            if (options.HasWhere)                       query = query.Where(options.Where);
            if (options.HasOrderBy)                     query = query.OrderBy(options.OrderBy);
            foreach(var item in options.GetIncludes())  query = query.Include(item);

            var primaryKey = _context.Model?
                .FindEntityType(typeof(T))?
                .FindPrimaryKey()?
                .Properties
                .Select(p => p.Name)
                .FirstOrDefault();
            
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKey) == id);
            
        }

        public async Task RemoveAsync(int id)
        {
            T? entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }     
        }

        public async Task UpdateAsync(T entity)
        {
           _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

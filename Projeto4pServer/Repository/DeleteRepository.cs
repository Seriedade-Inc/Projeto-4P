using Projeto4pServer.Data;

namespace Projeto4pServer.Repository
{
    public class DeleteRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public DeleteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"{typeof(T).Name} not found.");

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
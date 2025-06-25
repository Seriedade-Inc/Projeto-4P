using Projeto4pSharedLibrary.Classes;
using Projeto4pServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projeto4pServer.Repository
{
    public interface IAgendaRepository
    {
        Task<List<Agenda>> GetAllAsync();
        Task<Agenda?> GetByIdAsync(long id);
        Task<Agenda> CreateAsync(Agenda agenda);
        Task UpdateAsync(Agenda agenda);
    }

    public class AgendaRepository : IAgendaRepository
    {
        private readonly AppDbContext _context;

        public AgendaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Agenda>> GetAllAsync()
        {
            return await _context.Set<Agenda>().ToListAsync();
        }

        public async Task<Agenda?> GetByIdAsync(long id)
        {
            return await _context.Set<Agenda>().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Agenda> CreateAsync(Agenda agenda)
        {
            _context.Set<Agenda>().Add(agenda);
            await _context.SaveChangesAsync();
            return agenda;
        }

        public async Task UpdateAsync(Agenda agenda)
        {
            _context.Entry(agenda).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
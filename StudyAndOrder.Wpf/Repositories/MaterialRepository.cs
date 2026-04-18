// Implementerer IMaterialRepository og håndterer alle database-operationer for materialer via EF Core.

using Microsoft.EntityFrameworkCore;
using StudyAndOrder.Core.Data;
using StudyAndOrder.Core.Models;

namespace StudyAndOrder.Wpf.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _db;

        public MaterialRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Material>> GetAllAsync()
            => await _db.Materials.ToListAsync();

        public async Task AddAsync(Material material)
        {
            _db.Materials.Add(material);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Material material)
        {
            _db.Materials.Update(material);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Material material)
        {
            _db.Materials.Remove(material);
            await _db.SaveChangesAsync();
        }
    }
}
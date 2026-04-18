// Implementerer IEquipmentRepository og håndterer alle database-operationer for udstyr via EF Core.

using Microsoft.EntityFrameworkCore;
using StudyAndOrder.Core.Data;
using StudyAndOrder.Core.Models;

namespace StudyAndOrder.Wpf.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AppDbContext _db;

        public EquipmentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Equipment>> GetAllAsync()
            => await _db.Equipments.ToListAsync();

        public async Task AddAsync(Equipment equipment)
        {
            _db.Equipments.Add(equipment);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Equipment equipment)
        {
            _db.Equipments.Update(equipment);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Equipment equipment)
        {
            _db.Equipments.Remove(equipment);
            await _db.SaveChangesAsync();
        }
    }
}
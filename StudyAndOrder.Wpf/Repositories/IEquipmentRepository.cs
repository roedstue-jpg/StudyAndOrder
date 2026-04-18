// Interface der definerer hvilke database-operationer vi kan udføre på udstyr.

using StudyAndOrder.Core.Models;

namespace StudyAndOrder.Wpf.Repositories
{
    public interface IEquipmentRepository
    {
        Task<List<Equipment>> GetAllAsync();
        Task AddAsync(Equipment equipment);
        Task UpdateAsync(Equipment equipment);
        Task DeleteAsync(Equipment equipment);
    }
}
// Interface der definerer hvilke database-operationer vi kan udføre på materialer.

using StudyAndOrder.Core.Models;

namespace StudyAndOrder.Wpf.Repositories
{
    public interface IMaterialRepository
    {
        Task<List<Material>> GetAllAsync();
        Task AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task DeleteAsync(Material material);
    }
}
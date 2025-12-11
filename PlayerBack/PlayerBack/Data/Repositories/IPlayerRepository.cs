using PlayerBack.Models;

namespace PlayerBack.Data.Repositories
{
    public interface IPlayerRepository
    {
        Task<List<PlayerModel>> GetAllAsync(CancellationToken cancellationToken);
        Task<PlayerModel?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task CreateAsync(PlayerModel player, CancellationToken cancellationToken);
        Task<PlayerModel?> DeleteByIdAsync(string id, CancellationToken cancellationToken);
        Task DeleteAllAsync(CancellationToken cancellationToken);
    }
}

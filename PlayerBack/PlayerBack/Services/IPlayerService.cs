using PlayerBack.Models;

namespace PlayerBack.Services
{
    public interface IPlayerService
    {
        Task<List<PlayerModel>> GetPlayerListAsync(CancellationToken cancellationToken);
        Task<PlayerModel?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task CreateAsync(PlayerModel player, CancellationToken cancellationToken);
        Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken);
        Task DeleteAllAsync(CancellationToken cancellationToken);
        Task<StatisticsModel?> GetStatisticsAsync(CancellationToken cancellationToken);
    }
}

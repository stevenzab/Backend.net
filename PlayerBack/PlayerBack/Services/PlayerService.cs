using MongoDB.Bson;
using PlayerBack.Data.Repositories;
using PlayerBack.Models;

namespace PlayerBack.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repo;

        public PlayerService(IPlayerRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<PlayerModel>> GetPlayerListAsync(CancellationToken ct)
        {
            return await _repo.GetAllAsync(ct);
        }

        public async Task<PlayerModel?> GetByIdAsync(string id, CancellationToken ct)
        {
            return await _repo.GetByIdAsync(id, ct);
        }

        public async Task CreateAsync(PlayerModel player, CancellationToken ct)
        {
            if (player == null)
                throw new ArgumentException("Player cannot be null");

            if (string.IsNullOrWhiteSpace(player.Id))
                player.Id = ObjectId.GenerateNewId().ToString();

            await _repo.CreateAsync(player, ct);
        }

        public async Task<bool> DeleteByIdAsync(string id, CancellationToken ct)
        {
            var deleted = await _repo.DeleteByIdAsync(id, ct);
            return deleted != null;
        }

        public async Task DeleteAllAsync(CancellationToken ct)
        {
            await _repo.DeleteAllAsync(ct);
        }
    }

}

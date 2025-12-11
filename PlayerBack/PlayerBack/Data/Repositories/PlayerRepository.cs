using MongoDB.Driver;
using PlayerBack.Models;

namespace PlayerBack.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IMongoCollection<PlayerModel> _collection;

        public PlayerRepository(MongoDbService mongo)
        {
            _collection = mongo.Database.GetCollection<PlayerModel>("players");
        }

        public async Task<List<PlayerModel>> GetAllAsync(CancellationToken cancellationToken)
            => await _collection.Find(_ => true).SortBy(p => p.Data.Rank).ToListAsync(cancellationToken);

        public async Task<PlayerModel?> GetByIdAsync(string id, CancellationToken cancellationToken)
            => await _collection.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);

        public async Task CreateAsync(PlayerModel player, CancellationToken cancellationToken)
            => await _collection.InsertOneAsync(player, cancellationToken: cancellationToken);

        public async Task<PlayerModel?> DeleteByIdAsync(string id, CancellationToken cancellationToken)
            => await _collection.FindOneAndDeleteAsync(p => p.Id == id, cancellationToken: cancellationToken);

        public async Task DeleteAllAsync(CancellationToken cancellationToken)
            => await _collection.DeleteManyAsync(_ => true, cancellationToken);
    }

}

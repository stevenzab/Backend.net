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

        public async Task<List<PlayerModel>> GetPlayerListAsync(CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync(cancellationToken);
        }

        public async Task<PlayerModel?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(id, cancellationToken);
        }

        public async Task CreateAsync(PlayerModel player, CancellationToken cancellationToken)
        {
            if (player == null)
                throw new ArgumentException("Player cannot be null");

                player.Id = ObjectId.GenerateNewId().ToString();

            await _repo.CreateAsync(player, cancellationToken);
        }

        public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken)
        {
            var deleted = await _repo.DeleteByIdAsync(id, cancellationToken);
            return deleted != null;
        }

        public async Task DeleteAllAsync(CancellationToken cancellationToken)
        {
            await _repo.DeleteAllAsync(cancellationToken);
        }

        public async Task<StatisticsModel?> GetStatisticsAsync(CancellationToken cancellationToken)
        {
            var players = await _repo.GetAllAsync(cancellationToken);
            if (players == null || players.Count == 0)
                return null;

            var ratioModel = ComputeCountryWithHighestWinRatio(players);
            var avgBmi = ComputeAverageBmi(players);
            var medianHeight = ComputeMedianHeight(players);

            return new StatisticsModel
            {
                CountryCodeWithHighestWinRatio = ratioModel.CountryCode,
                HighestWinRatio = Math.Round(ratioModel.Ratio, 4),
                AverageBmi = Math.Round(avgBmi, 2),
                MedianHeight = Math.Round(medianHeight, 2)
            };
        }

        private static CountryWinRatioModel ComputeCountryWithHighestWinRatio(IEnumerable<PlayerModel> players)
        {
            var countryStats = players
                .Where(p => !string.IsNullOrWhiteSpace(p.Country?.Code))
                .Select(p => new
                {
                    Code = p.Country?.Code,
                    Wins = p.Data?.Last?.Count(r => r == 1) ?? 0,
                    Matches = p.Data?.Last?.Count ?? 0
                })
                .GroupBy(x => x.Code)
                .Select(g => new
                {
                    Code = g.Key,
                    Wins = g.Sum(x => x.Wins),
                    Matches = g.Sum(x => x.Matches)
                })
                .Select(x => new CountryWinRatioModel
                {
                    CountryCode = x.Code,
                    Ratio = x.Matches > 0 ? (double)x.Wins / x.Matches : 0.0
                })
                .OrderByDescending(x => x.Ratio)
                .FirstOrDefault();

            return countryStats ?? new CountryWinRatioModel { CountryCode = null, Ratio = 0.0 };
        }

        private static double ComputeAverageBmi(IEnumerable<PlayerModel> players)
        {
            var bmis = players
                .Select(p =>
                {
                    var w = p.Data?.Weight ?? 0;
                    var h = p.Data?.Height ?? 0;
                    return (w > 0 && h > 0) ? (double?)(w / Math.Pow(h / 100.0, 2)) : null;
                })
                .Where(b => b.HasValue)
                .Select(b => b.Value);

            return bmis.Any() ? bmis.Average() : 0.0;
        }

        private static double ComputeMedianHeight(IEnumerable<PlayerModel> players)
        {
            var heights = players
                .Select(p => p.Data?.Height ?? 0)
                .Where(h => h > 0)
                .OrderBy(h => h)
                .ToArray();

            if (heights.Length == 0)
                return 0.0;

            int n = heights.Length;
            if (n % 2 == 1)
                return heights[n / 2];

            return (heights[(n / 2) - 1] + heights[n / 2]) / 2.0;
        }
    }

}

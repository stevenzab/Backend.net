using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PlayerBack.Data;
using PlayerBack.Models;

namespace PlayerBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IMongoCollection<PlayerModel> _player;

        public PlayerController(MongoDbService mongoDbService)
        {
            if (mongoDbService?.Database == null)
            {
                throw new InvalidOperationException("MongoDB is not configured. Ensure ConnectionStrings:MongoDb is set in appsettings.json.");
            }

            _player = mongoDbService.Database.GetCollection<PlayerModel>("players");
        }

        [HttpGet("PlayerList")]
        public async Task<ActionResult<List<PlayerModel>>> GetPlayerListAsync()
        {
            var players = await _player.Find(_ => true)
                                       .SortBy(p => p.Data.Rank)
                                       .ToListAsync();

            if (players == null || players.Count == 0)
                return NotFound();

            return players;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerModel>> GetPlayerDetailsByIdAsync(string id)
        {
            var player = await _player.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (player == null)
                return NotFound();

            return player;
        }

        [HttpPost("CreatePlayer")]
        public async Task<ActionResult> CreatePlayerAsync(PlayerModel player)
        {
            if (player == null)
            {
                return BadRequest("Player payload is required.");
            }

            if (string.IsNullOrWhiteSpace(player.Id) || !ObjectId.TryParse(player.Id, out _))
            {
                player.Id = ObjectId.GenerateNewId().ToString();
            }

            try
            {
                await _player.InsertOneAsync(player);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : ""), statusCode: 500);
            }

            return CreatedAtRoute(new { id = player.Id.ToString() }, player);
        }
    }
}

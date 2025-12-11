using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PlayerBack.Data;
using PlayerBack.Models;
using PlayerBack.Services;

namespace PlayerBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;

        public PlayerController(IPlayerService service)
        {
            _service = service;
        }

        [HttpGet("PlayerList")]
        public async Task<ActionResult<List<PlayerModel>>> GetPlayerListAsync(CancellationToken cancellationToken)
        {
            var result = await _service.GetPlayerListAsync(cancellationToken);

            if (result == null || result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerModel>> GetPlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("GetStatistics")]
        public async Task<ActionResult<StatisticsModel>> GetPlayerStatisticsAsync(CancellationToken cancellationToken)
        {
            var stats = await _service.GetStatisticsAsync(cancellationToken);
            if (stats == null)
                return NotFound();

            return Ok(stats);
        }

        [HttpPost("CreatePlayer")]
        public async Task<ActionResult> CreatePlayerAsync(PlayerModel player, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(player, cancellationToken);
            return CreatedAtRoute(new { id = player.Id }, player);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            bool deleted = await _service.DeleteByIdAsync(id, cancellationToken);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("DeleteAllPlayer")]
        public async Task<ActionResult> DeleteAllPlayerAsync(CancellationToken cancellationToken)
        {
            await _service.DeleteAllAsync(cancellationToken);
            return NoContent();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController(IUoW unitOfWork, IMapper mapper, ILogger<GamesController> logger) : ControllerBase
{
    private readonly IUoW _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GamesController> _logger = logger;

    // GET: api/Tournaments
    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(string? title)
    {
        IEnumerable<Game>? games;
        try
        {
            if (string.IsNullOrEmpty(title))
            {
                games = await _unitOfWork.GameRepository.GetAllAsync();
            }
            else
            {
                games = await _unitOfWork.GameRepository.FindAsync(g => g.Title.Contains(title));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving games.");
            return StatusCode(500, "Internal server error.");
        }

        try
        {
            // Null check for games (though usually this should return an empty list)
            if (games is null || !games.Any())
            {
                _logger.LogWarning("No games found.");
                return NotFound("No games found.");
            }

            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving games.");
            return StatusCode(500, "Internal server error.");
        }
    }


    // GET: api/Games/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> GetGame(int id)
    {
        try
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                _logger.LogWarning($"Game with ID {id} not found.");
                return NotFound($"Game with ID {id} not found.");
            }

            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the game with ID {id}.");
            return StatusCode(500, "Internal server error.");
        }
    }

    // POST: api/Games
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the GameDto.");
            return BadRequest(ModelState);
        }

        var game = _mapper.Map<Game>(gameDto);

        if (game == null)
        {
            _logger.LogWarning("Mapping error: GameDto could not be mapped to Game.");
            return StatusCode(500, "Internal server error.");
        }

        if (string.IsNullOrEmpty(game.Title) || game.Time == default)
        {
            string[] missingFields = {
                string.IsNullOrEmpty(game.Title) ? "title" : "",
                game.Time == default ? "Time" : ""
            };

            _logger.LogWarning($"Bad request for creating a game. Missing required fields: {string.Join(", ", missingFields.Where(f => f != null))}");
            return BadRequest("Missing required fields.");
        }

        if (await _unitOfWork.GameRepository.AnyAsync(game.Id))
        {
            _logger.LogWarning($"Conflict: Game with ID {game.Id} already exists.");
            return Conflict("Game with the same ID already exists.");
        }

        await _unitOfWork.GameRepository.AddAsync(game);
        _unitOfWork.GameRepository.ChangeState(game, EntityState.Added);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrent update error occurred while creating game with ID {game.Id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating game with ID {game.Id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        var createdGame = _mapper.Map<GameDto>(game);
        return CreatedAtAction("GetGame", new { id = createdGame.Id }, createdGame);
    }

    // PUT: api/Games/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, GameDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the GameDto.");
            return BadRequest(ModelState);
        }

        var existingGame = await _unitOfWork.GameRepository.GetAsync(id);

        if (existingGame is null)
        {
            _logger.LogWarning($"Game with ID {id} not found.");
            return NotFound();
        }

        var game = _mapper.Map(dto, existingGame);

        if (id != game.Id)
        {
            _logger.LogWarning($"Bad request: Game ID mismatch. Path ID: {id}, Body ID: {game.Id}.");
            return BadRequest("Game ID mismatch.");
        }

        if (string.IsNullOrEmpty(game.Title) || game.Time == default)
        {
            string[] missingFields = {
                string.IsNullOrEmpty(game.Title) ? "title" : "",
                game.Time == default ? "time" : ""
            };

            _logger.LogWarning($"Bad request: Incomplete body for game ID {id}, Missing required fields: {string.Join(" and ", missingFields.Where(f => f != null))}");
            return BadRequest("Incomplete game data.");
        }

        _unitOfWork.GameRepository.ChangeState(game, EntityState.Modified);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, $"Concurrent update error occurred while updating game with ID {id}.");
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating game with ID {id}.");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // DELETE: api/Games/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(id);

        if (game is null)
        {
            _logger.LogWarning($"Game with ID {id} not found.");
            return NotFound("Game not found.");
        }

        _unitOfWork.GameRepository.Remove(game);
        _unitOfWork.GameRepository.ChangeState(game, EntityState.Deleted);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, $"Concurrent update error occurred while deleting game with ID {id}.");
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting game with ID {id}.");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }
}

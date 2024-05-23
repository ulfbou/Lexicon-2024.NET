using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Repositories;
using Serilog;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TournamentsController(IUoW unitOfWork, IMapper mapper, ILogger<TournamentsController> logger) : ControllerBase
{
    private readonly IUoW _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<TournamentsController> _logger = logger;

    // GET: api/Tournaments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
    {
        var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();

        if (tournaments is null)
        {
            _logger.LogWarning($"Tournament no longer exists.");
            return NotFound("Tournaments no longer exists.");
        }

        return Ok(_mapper.Map<IEnumerable<Tournament>>(tournaments));
    }

    // GET: api/Tournaments/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TournamentDto>> GetTournament(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Invalid model state for the Tournament {id}.");
            return BadRequest(ModelState);
        }

        var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {id} no longer exists.");
            return NotFound("Tournament no longer exists.");
        }

        return Ok(_mapper.Map<Tournament>(tournament));
    }

    // POST: api/Tournaments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournament(TournamentDto tournamentDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the TournamentDto.");
            return BadRequest(ModelState);
        }

        var tournament = _mapper.Map<Tournament>(tournamentDto);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {tournamentDto.Id} no longer exists.");
            return NotFound("Tournament no longer exists.");
        }

        if (tournament.Title is null || tournament.StartDate is null || tournament.Games is null)
        {
            string[] missing = [
                tournament.Title != null ? "" : "title missing",
                tournament.StartDate != null ? "" : "StartDate missing",
                tournament.Games != null ? "" : "Games missing"
            ];
            _logger.LogWarning($"Bad request for creating a tournament. Missing required fields `{string.Join(',', missing)}`");
            return BadRequest();
        }

        var id = tournament.Id;

        if (await _unitOfWork.TournamentRepository.AnyAsync(id))
        {
            _logger.LogWarning($"Conflict with post tournament `{id}`");
            return Conflict();
        }

        _unitOfWork.TournamentRepository.Add(tournament);
        _unitOfWork.TournamentRepository.ChangeState(tournament, EntityState.Added);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
            {
                _logger.LogError("Post Tournament: Failed to create concurrent post.");
                return StatusCode(500);
            }

            _logger.LogError(ex, "Concurrent update error occurred while updating tournament with ID {id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating tournament with ID {id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        var createdTournament = _mapper.Map<TournamentDto>(tournament);

        return CreatedAtAction("GetTournament", new { id = createdTournament.Id }, createdTournament);
    }

    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournament(
        int id,
        TournamentDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the TournamentDto.");
            return BadRequest(ModelState);
        }

        if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
        {
            _logger.LogWarning($"Tournament with ID {id} not found.");
            return NotFound();
        }

        var tournament = _mapper.Map<Tournament>(dto);

        if (id != tournament.Id)
        {
            _logger.LogWarning($"Bad request: Tournament ID mismatch. Path ID: {id}, Body ID: {tournament.Id}.");
            return BadRequest("Tournament ID mismatch.");
        }

        if (string.IsNullOrEmpty(tournament.Title) || tournament.StartDate == default || tournament.Games == null)
        {
            _logger.LogWarning($"Bad request: Incomplete body for tournament ID {id}.");
            return BadRequest("Incomplete tournament data.");
        }

        _unitOfWork.TournamentRepository.ChangeState(tournament, EntityState.Modified);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
            {
                _logger.LogError("Post Tournament: Failed to create concurrent post.");
                return StatusCode(500);
            }

            _logger.LogError(ex, "Concurrent update error occurred while updating tournament with ID {id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating tournament with ID {id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTournament(
        int id,
        TournamentDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the TournamentDto.");
            return BadRequest(ModelState);
        }

        var tournament = _mapper.Map<Tournament>(dto);

        if (id != tournament.Id) return BadRequest();

        _unitOfWork.TournamentRepository.ChangeState(tournament, EntityState.Modified);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
            {
                _logger.LogError("Post Tournament: Failed to create concurrent post.");
                return StatusCode(500);
            }

            _logger.LogError(ex, "Concurrent update error occurred while updating tournament with ID {id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating tournament with ID {id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // DELETE: api/Tournaments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournament(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the TournamentDto.");
            return BadRequest(ModelState);
        }

        var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {id} no longer exists.");
            return NotFound("Tournament no longer exists.");
        }

        _unitOfWork.TournamentRepository.Remove(tournament);
        _unitOfWork.TournamentRepository.ChangeState(tournament, EntityState.Deleted);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
            {
                _logger.LogError("Post Tournament: Failed to create concurrent post.");
                return StatusCode(500);
            }

            _logger.LogError(ex, "Concurrent update error occurred while updating tournament with ID {id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating tournament with ID {id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // GET: api/Tournaments/{tournamentId}/Games
    [HttpGet("{tournamentId:int}/Games")]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(int tournamentId)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {tournamentId} no longer exists.");
            return NotFound("Tournament no longer exists.");
        }

        var games = tournament.Games;


        if (games is null)
        {
            _logger.LogWarning($"Games no longer exists.");
            return NotFound("Tournament's Games no longer exists.");
        }

        return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
    }

    // GET: api/Tournaments/{tournamentId}/Games/{gameId}
    [HttpGet("{tournamentId:int}/Games/{gameId:int}")]
    public async Task<ActionResult<GameDto>> GetGame(
        int tournamentId,
        int gameId)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {tournamentId} no longer exists.");
            return NotFound("Tournament no longer exists.");
        }

        var game = tournament.Games.FirstOrDefault(g => g.Id == gameId);

        if (game is null)
        {
            _logger.LogWarning($"Game with ID {gameId} no longer exists.");
            return NotFound("Game no longer exists.");
        }

        return Ok(_mapper.Map<GameDto>(game));
    }

    // POST: api/Tournaments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("{tournamentId}/Games")]
    public async Task<ActionResult<TournamentDto>> PostGame(
        int tournamentId,
        GameDto gameDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the GameDto.");
            return BadRequest(ModelState);
        }

        var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {tournamentId} no longer exists.");
            return NotFound("Tournament no longer exists.");
        }

        var game = _mapper.Map<Game>(gameDto);

        if (game is null)
        {
            _logger.LogWarning($"Game with ID {gameDto.Id} no longer exists.");
            return NotFound("Game no longer exists.");
        }

        if (game.Title is null || game.Time is null)
        {
            string[] missing = [
                game.Title != null ? "" : "title missing",
                game.Time != null ? "" : "StartDate missing"
            ];

            _logger.LogWarning($"Post tournament: Bad request! Missing required fields `{string.Join(',', missing)}`");
            return BadRequest("Missing required fields");
        }

        tournament.Games.Add(game);
        _unitOfWork.GameRepository.Add(game);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await _unitOfWork.GameRepository.AnyAsync(game.Id))
            {
                _logger.LogError("Post Game: Failed to create concurrent post.");
                return StatusCode(500);
            }

            _logger.LogError(ex, "Concurrent update error occurred while creating game with ID {id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating game with ID {id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        var createdGame = _mapper.Map<GameDto>(game);

        return CreatedAtAction("GetGame", new { id = createdGame.Id }, createdGame);
    }

    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{tournamentId}/Games/{gameId}")]
    public async Task<IActionResult> PutGame(TournamentGameDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the GameDto.");
            return BadRequest(ModelState);
        }

        var tournamentId = dto.TournamentId;
        var gameId = dto.GameId;
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);

        if (tournament == null) return NotFound();
        var game = _mapper.Map<Game>(dto);

        if (tournament.Games.FirstOrDefault(game => game.Id == gameId) is null) return NotFound(game);
        if (gameId != game.Id) return BadRequest();

        _unitOfWork.GameRepository.ChangeState(game, EntityState.Modified);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await _unitOfWork.GameRepository.AnyAsync(game.Id))
            {
                _logger.LogError("Post Game: Failed to create concurrent post.");
                return StatusCode(500);
            }

            _logger.LogError(ex, "Concurrent update error occurred while creating game with ID {id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating game with ID {id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // DELETE: api/Tournaments/5
    [HttpDelete("{tournamentId}/Games/{gameId}")]
    public async Task<IActionResult> DeleteTournament(
        TournamentDto tournamentDto,
        int gameId)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the GameDto.");
            return BadRequest(ModelState);
        }

        var tournament = _mapper.Map<Tournament>(tournamentDto);
        if (tournament is null) return NotFound();

        var game = await _unitOfWork.GameRepository.GetAsync(gameId);

        if (game is null) return NotFound(gameId);
        if (tournament.Games.FirstOrDefault(game => game.Id == gameId) is null) return NotFound();

        tournament.Games.Remove(game);
        _unitOfWork.GameRepository.Remove(game);
        _unitOfWork.GameRepository.ChangeState(game, EntityState.Deleted);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await _unitOfWork.GameRepository.AnyAsync(game.Id))
            {
                _logger.LogError("Post Game: Failed to create concurrent post.");
                return StatusCode(500);
            }

            _logger.LogError(ex, "Concurrent update error occurred while deleting game with ID {id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting game with ID {id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // Generic method to check if an entity exists
    private async Task<bool> Exists<T>(IRepository<T> repository, int id) where T : class
        => await repository.AnyAsync(id);
}

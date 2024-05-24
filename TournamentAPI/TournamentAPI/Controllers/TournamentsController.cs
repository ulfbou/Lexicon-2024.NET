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
public class TournamentsController(IUoW unitOfWork, IMapper mapper, ILogger<GamesController> logger) : ControllerBase
{
    private readonly IUoW _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GamesController> _logger = logger;

    // GET: api/Tournaments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments(string? title, bool? include = true)
    {
        var inclusion = true;

        if (include.HasValue)
        {
            inclusion = include.Value;
        }

        if (!string.IsNullOrEmpty(title))
        {
            try
            {
                var tournaments = await _unitOfWork.TournamentRepository.FindAsync(t => t.Title.Contains(title), inclusion);

                if (tournaments is null || !tournaments.Any())
                {
                    _logger.LogWarning("No tournaments found matching {title}.", []);
                    return NotFound("No tournaments found.");
                }

                return Ok(_mapper.Map<IEnumerable<TournamentDto>>(tournaments));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving tournaments.");
                return StatusCode(500, "Internal server error.");
            }
        }

        try
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync(inclusion);

            if (tournaments is null || !tournaments.Any())
            {
                _logger.LogWarning("No tournaments found.");
                return NotFound("No tournaments found.");
            }

            return Ok(_mapper.Map<IEnumerable<TournamentDto>>(tournaments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving tournaments.");
            return StatusCode(500, "Internal server error.");
        }
    }

    // GET: api/Tournaments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournament(int id, bool? inclusion)
    {
        bool include;

        if (inclusion.HasValue)
        {
            include = inclusion.Value;
        }
        else
        {
            include = true;
        }

        try
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id, include);

            if (tournament == null)
            {
                _logger.LogWarning($"Tournament with ID {id} not found.");
                return NotFound($"Tournament with ID {id} not found.");
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            return Ok(tournamentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the tournament with ID {id}.");
            return StatusCode(500, "Internal server error.");
        }
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

        if (tournament == null)
        {
            _logger.LogWarning("Mapping error: TournamentDto could not be mapped to Tournament.");
            return StatusCode(500, "Internal server error.");
        }

        if (string.IsNullOrEmpty(tournament.Title) || tournament.StartTime == default || tournament.Games == null)
        {
            string[] missingFields = {
                string.IsNullOrEmpty(tournament.Title) ? "title" : "",
                tournament.StartTime == default ? "startDate" : "",
                tournament.Games is null ? "games" : ""
            };

            _logger.LogWarning($"Bad request for creating a tournament. Missing required fields: {string.Join(", ", missingFields.Where(f => f != null))}");
            return BadRequest("Missing required fields.");
        }

        if (await _unitOfWork.TournamentRepository.AnyAsync(tournament.Id))
        {
            _logger.LogWarning($"Conflict: Tournament with ID {tournament.Id} already exists.");
            return Conflict("Tournament with the same ID already exists.");
        }

        await _unitOfWork.TournamentRepository.AddAsync(tournament);
        _unitOfWork.TournamentRepository.ChangeState(tournament, EntityState.Added);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrent update error occurred while creating tournament with ID {tournament.Id}.", []);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating tournament with ID {tournament.Id}.", []);
            return StatusCode(500, "An unexpected error occurred.");
        }

        var createdTournament = _mapper.Map<TournamentDto>(tournament);
        return CreatedAtAction("GetTournament", new { id = createdTournament.Id }, createdTournament);
    }

    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournament(int id, TournamentDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the TournamentDto.");
            return BadRequest(ModelState);
        }

        var existingTournament = await _unitOfWork.TournamentRepository.GetAsync(id);

        if (existingTournament is null)
        {
            _logger.LogWarning($"Tournament with ID {id} not found.");
            return NotFound();
        }

        var tournament = _mapper.Map(dto, existingTournament);

        if (id != tournament.Id)
        {
            _logger.LogWarning($"Bad request: Tournament ID mismatch. Path ID: {id}, Body ID: {tournament.Id}.");
            return BadRequest("Tournament ID mismatch.");
        }

        if (string.IsNullOrEmpty(tournament.Title) || tournament.StartTime == default || tournament.Games == null)
        {
            string[] missingFields = {
                string.IsNullOrEmpty(tournament.Title) ? "title" : "",
                tournament.StartTime == default ? "startDate" : "",
                tournament.Games == null ? "games" : ""
            };

            _logger.LogWarning($"Bad request: Incomplete body for tournament ID {id}, Missing required fields: {string.Join(", ", missingFields.Where(f => f != null))}");
            return BadRequest("Incomplete tournament data.");
        }

        _unitOfWork.TournamentRepository.ChangeState(tournament, EntityState.Modified);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, $"Concurrent update error occurred while updating tournament with ID {id}.");
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating tournament with ID {id}.");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // DELETE: api/Tournaments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournament(int id)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {id} not found.");
            return NotFound("Tournament not found.");
        }

        _unitOfWork.TournamentRepository.Remove(tournament);
        _unitOfWork.TournamentRepository.ChangeState(tournament, EntityState.Deleted);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, $"Concurrent update error occurred while deleting tournament with ID {id}.");
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting tournament with ID {id}.");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // GET: api/Tournaments/{id}/Games
    [HttpGet("{tournamentId}/Games")]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetTournamentGames(int tournamentId, string title)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {tournamentId} not found.");
            return NotFound($"Tournament with ID {tournamentId} not found.");
        }

        var games = tournament.Games;

        if (games is null || !games.Any())
        {
            _logger.LogWarning($"No games found for Tournament with ID {tournamentId}.");
            return NotFound($"No games found for Tournament with ID {tournamentId}.");
        }

        if (!string.IsNullOrEmpty(title))
        {
            games = games.Where(g => g.Title.Contains(title)).ToList();

            if (!games.Any())
            {
                _logger.LogWarning($"No games found for Tournament with ID {tournamentId} matching search string `{title}`.");
                return NotFound($"No games found for Tournament with ID {tournamentId} matching search string `{title}`.");
            }
        }

        var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);

        return Ok(gamesDto);
    }

    // GET: api/Tournaments/{tournamentId}/Games
    [HttpGet("{tournamentId}/Games/{gameId}")]
    public async Task<ActionResult<GameDto>> GetTournamentGame(int tournamentId, int gameId)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {tournamentId} not found.");
            return NotFound($"Tournament with ID {tournamentId} not found.");
        }

        var game = tournament.Games.FirstOrDefault(g => g.Id == gameId);

        if (game is null)
        {
            _logger.LogWarning($"Game with ID {gameId} not found in tournament with ID {tournamentId}.");
            return NotFound($"Game with ID {gameId} not found in tournament with ID {tournamentId}.");
        }

        return Ok(_mapper.Map<GameDto>(game));
    }

    // POST: api/Tournaments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("{tournamentId}/Games")]
    public async Task<ActionResult<GameDto>> PostTournamentGame(int tournamentId, GameDto gameDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the GameDto.");
            return BadRequest(ModelState);
        }

        var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {tournamentId} not found.");
            return NotFound($"Tournament with ID {tournamentId} not found.");
        }

        var game = _mapper.Map<Game>(gameDto);

        if (string.IsNullOrEmpty(game.Title) || game.Time == default)
        {
            string[] missing = {
                string.IsNullOrEmpty(game.Title) ? "title missing" : "",
                game.Time == default ? "time missing" : ""
            };

            _logger.LogWarning($"Post game: Bad request! Missing required fields `{string.Join(", ", missing)}`");
            return BadRequest("Missing required fields");
        }

        tournament.Games.Add(game);
        await _unitOfWork.GameRepository.AddAsync(game);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrent update error occurred while creating game with ID {GameId}.", game.Id);
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating game with ID {GameId}.", game.Id);
            return StatusCode(500, "An unexpected error occurred.");
        }

        var createdGame = _mapper.Map<GameDto>(game);

        return CreatedAtAction("GetGame", new { tournamentId = tournamentId, gameId = createdGame.Id }, createdGame);
    }

    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{tournamentId}/Games/{gameId}")]
    public async Task<IActionResult> PutTournamentGame(TournamentGameDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for the GameDto.");
            return BadRequest(ModelState);
        }

        var tournamentId = dto.TournamentId;
        var gameId = dto.GameId;
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);

        if (tournament is null)
        {
            _logger.LogWarning($"Tournament with ID {tournamentId} not found.");
            return NotFound($"Tournament with ID {tournamentId} not found.");
        }

        var game = _mapper.Map<Game>(dto);

        if (game is null)
        {
            _logger.LogWarning($"Failed to map GameDto to Game object.");
            return BadRequest("Failed to map GameDto to Game object.");
        }

        if (gameId != game.Id)
        {
            _logger.LogWarning($"Bad request: Game ID mismatch. Path ID: {gameId}, Body ID: {game.Id}.");
            return BadRequest("Game ID mismatch.");
        }

        var existingGame = tournament.Games.FirstOrDefault(g => g.Id == gameId);

        if (existingGame is null)
        {
            _logger.LogWarning($"Game with ID {gameId} not found in tournament.");
            return NotFound($"Game with ID {gameId} not found in tournament.");
        }

        _unitOfWork.GameRepository.ChangeState(game, EntityState.Modified);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, $"Concurrent update error occurred while updating game with ID {gameId}.");
            return StatusCode(500, "A concurrency error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating game with ID {gameId}.");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return NoContent();
    }

    // DELETE: api/Tournaments/5
    [HttpDelete("{tournamentId}/Games/{gameId}")]
    public async Task<IActionResult> DeleteTournamentGame(
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
        if (tournament.Games.FirstOrDefault(game => game.Id == gameId) is null)
        {
            _logger.LogWarning($"Game with ID {gameId} not found in tournament.");
            return NotFound($"Game with ID {gameId} not found in tournament.");
        }

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
}

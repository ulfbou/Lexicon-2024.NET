using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;

namespace TournamentAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController(IUoW unitOfWork, IMapper mapper, ILogger logger) : ControllerBase
{
    private readonly IUoW _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    // GET: api/Games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetGames()
    {
        var games = await _unitOfWork.GameRepository.GetAllAsync();

        if (games is null) return NotFound();

        return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
    }

    // GET: api/Games/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(int id)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(id);

        if (game is null) return NotFound(id);

        return Ok(_mapper.Map<GameDto>(game));
    }

    // POST: api/Games
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    public async Task<ActionResult<Game>> PostGame(GameDto dto)
    {
        var game = _mapper.Map<Game>(dto);

        if (await _unitOfWork.GameRepository.AnyAsync(game.Id) ||
            game.Title is null ||
            game.Time is null)
        {
            return Conflict(game.Id);
        }

        _unitOfWork.GameRepository.Add(game);
        _unitOfWork.GameRepository.ChangeState(game, EntityState.Added);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if (await GameExists(game.Id)) return NotFound(game.Id);
            throw;
        }

        var created = _mapper.Map<GameDto>(game);
        return CreatedAtAction("GetGame", new { id = created.Id }, created);
    }

    // PUT: api/Games/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, GameDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest();
        }

        var game = _mapper.Map<Game>(dto);

        if (string.IsNullOrEmpty(game.Title) || game.Time is null)
        {
            _logger.LogWarning($"Bad request: Incomplete body for tournament ID {id}.");
            return BadRequest("Incomplete tournament data.");
        }

        _unitOfWork.GameRepository.ChangeState(game, EntityState.Modified);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GameExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // PUT: api/Games/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PatchGame(int id, GameDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest();
        }

        var game = _mapper.Map<Game>(dto);

        _unitOfWork.GameRepository.ChangeState(game, EntityState.Modified);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GameExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Games/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await _unitOfWork.GameRepository.GetAsync(id);

        if (game is null) return NotFound();

        _unitOfWork.GameRepository.Remove(game);
        _unitOfWork.GameRepository.ChangeState(game, EntityState.Deleted);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GameExists(id)) return StatusCode(500);
            throw;
        }

        return NoContent();
    }


    // Generic method to check if an entity exists
    private async Task<bool> Exists<T>(IRepository<T> repository, int id) where T : class
        => await repository.AnyAsync(id);

    private async Task<bool> GameExists(int id)
        => await _unitOfWork.GameRepository.AnyAsync(id);
}

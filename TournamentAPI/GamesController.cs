using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Repositories;

namespace TournamentAPI;

//[Route("api/[controller]")]
//[ApiController]
public class GamesController(TournamentAPIContext context, IUoW unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly TournamentAPIContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IUoW _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    // GET: api/Games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
    {
        var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
        var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        return Ok(tournamentDtos);
    }

    // GET: api/Games/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournament(int id)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

        if (tournament == null)
        {
            return NotFound();
        }

        var tournamentDto = _mapper.Map<TournamentDto>(tournament);
        return Ok(tournamentDto);
    }

    // PUT: api/Games/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, Game game)
    {
        if (id != game.Id)
        {
            return BadRequest();
        }

        _context.Entry(game).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GameExists(id))
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

    // POST: api/Games
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Game>> PostGame(Game game)
    {
        _context.Game.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetGame", new { id = game.Id }, game);
    }

    // DELETE: api/Games/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await _context.Game.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        _context.Game.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GameExists(int id)
    {
        return _context.Game.Any(e => e.Id == id);
    }
}

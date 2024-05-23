using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Repositories;

namespace TournamentAPI;

//[Route("api/[controller]")]
//[ApiController]
public class TournamentsController(TournamentAPIContext context, IUoW unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly TournamentAPIContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IUoW _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    // GET: api/Tournaments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
    {
        var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();

        if (tournaments is null) return NotFound();

        var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        return Ok(tournamentDtos);
    }

    // GET: api/Tournaments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Tournament>> GetTournament(int id)
    {
        var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

        if (tournament == null)
        {
            return NotFound();
        }

        var tournamentDto = _mapper.Map<TournamentDto>(tournament);
        return Ok(tournamentDto);
    }

    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournament(int id, Tournament tournament)
    {
        if (id != tournament.Id)
        {
            return BadRequest();
        }

        _unitOfWork.TournamentRepository.Update(tournament);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TournamentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        _context.Entry(tournament).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TournamentExists(id))
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

    // POST: api/Tournaments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Tournament>> PostTournament(Tournament tournament)
    {
        _context.Tournament.Add(tournament);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTournament", new { id = tournament.Id }, tournament);
    }

    // DELETE: api/Tournaments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournament(int id)
    {
        var tournament = await _context.Tournament.FindAsync(id);
        if (tournament == null)
        {
            return NotFound();
        }

        _context.Tournament.Remove(tournament);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TournamentExists(int id)
    {
        return _context.Tournament.Any(e => e.Id == id);
    }
}

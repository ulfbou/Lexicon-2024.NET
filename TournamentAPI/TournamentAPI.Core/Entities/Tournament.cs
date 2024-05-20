namespace TournamentAPI.Core.Entities;

public class Tournament
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string StartDate { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = new List<Game>();  
}

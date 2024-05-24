using System.Text.Json.Serialization;

namespace TournamentAPI.Core.Entities;

public class Tournament : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = new List<Game>();
}

namespace TournamentAPI.Core.Repositories;

public interface IUoW
{
    ITournamentRepository Repository { get; }
    IGameRepository GameRepository { get; }  
}

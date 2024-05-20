using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TournamentAPI.Core.Entities;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data;
public class SeedData
{ 
    private static readonly string TimeFormat = "yyyy-MM-dd HH:mm";
    private static Faker? faker = null;

    public static async Task InitAsync(TournamentAPIContext context)
    {
        if (await context.Tournament.AnyAsync()) return;

        await Console.Out.WriteLineAsync($"Init SeedData beginning at {DateTime.Now}...");
        faker = new Faker("sv");

        var tournaments = GenerateTournament(20);
        await context.AddRangeAsync(tournaments);
        foreach (var tournament in tournaments)
        {
            await context.AddRangeAsync(tournament.Games);
        }

        await context.SaveChangesAsync();
        await Console.Out.WriteLineAsync($"Init SeedData completed at {DateTime.Now}...");
    }

    private static IEnumerable<string> GeneratePlayerNames(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            yield return $"{faker!.Name.FirstName()} {faker.Name.LastName()}";
        }
    }

    private static IEnumerable<Game> GenerateGames(int numberOfGames, IEnumerable<string> players, DateTime time)
    {
        var games = new List<Game>();
        var chosen = new HashSet<string>();

        DateTime startOfDay = time;
        DateTime startTime = time.AddHours(1);

        for (int i = 0; i < numberOfGames; i++)
        {
            var player1 = faker!.PickRandom(players);
            chosen.Add(player1);

            var player2 = faker.PickRandom(players);  

            while (chosen.Contains(player2))
            {
                player2 = faker.PickRandom(players);
            }

            chosen.Add(player2);

            var title = player1 + " vs " + player2;
            var game = new Game
            {
                Title = title,
                Time = startTime.ToString(TimeFormat)
            };

            games.Add(game);

            startTime = startTime.AddHours(1);

            if (startTime.Hour >= 20)
            {
                startOfDay = startOfDay.AddDays(1);
                startTime = startOfDay.AddHours(1);
            }
        }

        return games;
    }

    private static IEnumerable<Tournament> GenerateTournament(int numberOfTournaments)
    {
        var tournaments = new List<Tournament>(numberOfTournaments);
        var players = GeneratePlayerNames(numberOfTournaments * 4);

        for (int i = 0; i < numberOfTournaments; i++)
        {
            var title = faker!.Company.CompanyName() + " Cup";
            var time = DateTime.Today.AddHours(10).AddDays(30);
            var games = GenerateGames(8, players, time!);

            var tournament = new Tournament
            {
                Title = title,
                StartDate = time.ToString(TimeFormat),
                Games = (ICollection<Game>)games
            };

            tournaments.Add(tournament);
        }

        return tournaments;
    }

#if false
    private static async Task<IEnumerable<Game>> GenerateGames(int numberOfGames, IEnumerable<string> players, DateTime time)
    {
        var games = new List<Game>();
        var chosen = new HashSet<string>();

        DateTime startOfDay = time;
        DateTime startTime = time.AddHours(1);

        for (int i = 0; i < numberOfGames; i++)
        {
            var player1 = faker!.PickRandom(players);
            chosen.Add(player1);

            var player2 = faker.PickRandom(players);

            while (chosen.Contains(player2))
            {
                player2 = faker.PickRandom(players);
            }

            chosen.Add(player2);

            var title = player1 + " vs " + player2;
            var game = new Game
            {
                Title = title,
                Time = startTime.ToString(TimeFormat)
            };

            games.Add(game);
            startTime = startTime.AddHours(1);

            if (startTime.Hour >= 20)
            {
                startOfDay = startOfDay.AddDays(1);
                startTime = startOfDay.AddHours(1);
            }
        }

        return await Task.FromResult<IEnumerable<Game>>(games);
    }

    public static async Task InitAsync(TournamentAPIContext context)
    {
        if (await context.Tournament.AnyAsync()) return;

        await Console.Out.WriteLineAsync($"Init SeedData beginning at {DateTime.Now}...");
        faker = new Faker("sv");

        var tournaments = await GenerateTournament(20);
        await context.AddRangeAsync(tournaments);
        await foreach (var tournament in tournaments)
        {
            await context.AddRangeAsync(tournament.Games);
        }

        await context.SaveChangesAsync();
        await Console.Out.WriteLineAsync($"Init SeedData completed at {DateTime.Now}...");
    }

    private static IEnumerable<string> GeneratePlayerNames(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            yield return $"{faker!.Name.FirstName()} {faker.Name.LastName()}";
        }
    }

    private static async IAsyncEnumerable<Game> GenerateGamesAsync(int numberOfGames, IEnumerable<string> players, DateTime time)
    {
        await Task.Yield();
        
        var games = new List<Game>();
        var chosen = new HashSet<string>();

        DateTime startOfDay = time;
        DateTime startTime = time.AddHours(1);

        for (int i = 0; i < numberOfGames; i++)
        {
            var player1 = faker!.PickRandom(players);
            chosen.Add(player1);

            var player2 = faker.PickRandom(players);

            while (chosen.Contains(player2))
            {
                player2 = faker.PickRandom(players);
            }

            chosen.Add(player2);

            var title = player1 + " vs " + player2;
            var game = new Game
            {
                Title = title,
                Time = startTime.ToString(TimeFormat)
            };

            games.Add(game);

            yield return game;
            
            await Task.Yield();
            startTime = startTime.AddHours(1);

            if (startTime.Hour >= 20)
            {
                startOfDay = startOfDay.AddDays(1);
                startTime = startOfDay.AddHours(1);
            }
        }
        //return games.AsQueryable().AsAsyncEnumerable();
    }

    private static async Task<IAsyncEnumerable<Tournament>> GenerateTournament(int numberOfTournaments)
    {
        var tournaments = new List<Tournament>(numberOfTournaments);
        var players = GeneratePlayerNames(numberOfTournaments * 4);

        for (int i = 0; i < numberOfTournaments; i++)
        {
            var title = faker!.Company.CompanyName() + " Cup";
            var time = DateTime.Today.AddHours(10).AddDays(30);
            var games = GenerateGamesAsync(8, players, time!);
            
            var tournament = new Tournament
            {
                Title = title,
                StartDate = time.ToString(TimeFormat),
                Games = (ICollection<Game>)await Task.FromResult<IAsyncEnumerable<Game>>(games)
            };

            tournaments.Add(tournament);
        }

        return tournaments.AsQueryable().AsAsyncEnumerable();
    }

private static async Task<IEnumerable<Game>> GenerateGames(int numberOfGames, IEnumerable<string> players, DateTime time)
{
    var games = new List<Game>();
    var chosen = new HashSet<string>();

    DateTime startOfDay = time;
    DateTime startTime = time.AddHours(1);

    for (int i = 0; i < numberOfGames; i++)
    {
        var player1 = faker!.PickRandom(players);
        chosen.Add(player1);

        var player2 = faker.PickRandom(players);

        while (chosen.Contains(player2))
        {
            player2 = faker.PickRandom(players);
        }

        chosen.Add(player2);

        var title = player1 + " vs " + player2;
        var game = new Game
        {
            Title = title,
            Time = startTime.ToString(TimeFormat)
        };

        games.Add(game);
        startTime = startTime.AddHours(1);

        if (startTime.Hour >= 20)
        {
            startOfDay = startOfDay.AddDays(1);
            startTime = startOfDay.AddHours(1);
        }
    }

    return await Task.FromResult<IEnumerable<Game>>(games);
}
#endif
}

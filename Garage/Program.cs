namespace Garage;

/// <summary>
/// Represents the entry point of the program.
/// </summary>
public static class Program
{
    // The user interface instance used throughout the program.
    private static IUI UI = SimpleUI.Instance;

    /// <summary>
    /// The main entry point of the program.
    /// </summary>
    public static void Main()
    {
        Logger logger = Logger.Instance;
        logger.Log(LogLevel.Info, $"Initializing Garage.");

        // Create a new instance of the GarageManager using the specified user interface.
        GarageManager manager = new GarageManager(UI);

        // Run the garage manager.
        manager.Run();
    }
}

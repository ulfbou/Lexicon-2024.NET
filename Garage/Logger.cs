using System.ComponentModel;

namespace Garage;

public enum LogLevel
{
    Info,
    Warning,
    Error, 
    Fatal,
    Disabled
}

public class Logger
{
    private static Logger instance = null!;
    private LogLevel level = LogLevel.Info;

    public static Logger Instance
    {
        get
        {
            if (instance is null)
            {
                instance = new Logger();
            }

            return instance;
        }
    }

    public void SetVisibleToConsoleLevel(LogLevel level) => this.level = level;

    public void Log(object error, string message)
    {
        
    }
}
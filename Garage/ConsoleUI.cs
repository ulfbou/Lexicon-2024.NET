using System;
using System.Drawing;

namespace Garage;

public class ConsoleUI : IUI
{
    #region Properties
    /// <summary>
    /// Event Handlers that fires on pressing the keys Up, Down, and Escape.
    /// </summary>
    public event EventHandler? OnUpPressed;
    public event EventHandler? OnDownPressed;
    public event EventHandler? OnEnterPressed;
    public event EventHandler? OnEscapePressed;
    private readonly Dictionary<ConsoleKey, Action> KeyEvents;
    private List<(string, ConsoleColor?)> output = null!;
    private static ConsoleUI instance = null!;
    #endregion

    #region Static
    public static ConsoleUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConsoleUI();
            }
            return instance;
        }
        private set { }
    }
    #endregion

    #region Constructors
    static ConsoleUI()
    {
        Console.SetCursorPosition(0, 0);
        Console.CursorVisible = false;
        instance = new ConsoleUI();
    }

    public ConsoleUI()
    {
        KeyEvents = new Dictionary<ConsoleKey, Action>
        {
            { ConsoleKey.UpArrow, () => OnUpPressed?.Invoke(this, EventArgs.Empty) },
            { ConsoleKey.DownArrow, () => OnDownPressed?.Invoke(this, EventArgs.Empty) },
            { ConsoleKey.Enter, () => OnEnterPressed?.Invoke(this, EventArgs.Empty) },
            { ConsoleKey.Escape, () => OnEscapePressed?.Invoke(this, EventArgs.Empty) }
        };
        output = new List<(string, ConsoleColor?)>();
    }

    #endregion

    #region public methods
    public ConsoleKey KeyListener()
    {
        Console.CursorVisible = false;

        // Add event handlers to key pushes. 
        ConsoleKeyInfo keyPressed;
        bool eventActivation = false;

        do
        {
            keyPressed = Console.ReadKey();

            if (KeyEvents.ContainsKey(keyPressed.Key))
            {
                KeyEvents[keyPressed.Key]?.Invoke();
                eventActivation = true;
            }
        }
        while (!eventActivation);

        return keyPressed.Key;
    }

    public async void RunAsync()
    {
        Console.CursorVisible = false;

        await Task.Run(() =>
        {
            // Add event handlers to key pushes. 
            ConsoleKeyInfo keyPressed;

            do
            {
                keyPressed = Console.ReadKey();

                if (KeyEvents.ContainsKey(keyPressed.Key))
                {
                    KeyEvents[keyPressed.Key]?.Invoke();
                }
                else
                {
                    Console.WriteLine($"Pressed {keyPressed.KeyChar}");
                }
            }
            while (keyPressed.Key != ConsoleKey.Escape);
        });
    }

    public void Run()
    {
        //throw new NotImplementedException();
    }

    public void Reset()
    {
        output = new List<(string, ConsoleColor?)>();
    }

    public void Write(string output = "", ConsoleColor? color = null)
    {
        this.output.Add((output, color));
    }

    public void Display(bool clearScreen = true)
    {

        if (output == null)
        {
            throw new NullReferenceException(nameof(output));
        }

        if (clearScreen)
        {
            Console.Clear();
        }

        foreach ((string, ConsoleColor?) output in this.output)
        {
            if (output.Item2 is not null)
            {
                Console.ForegroundColor = (ConsoleColor)output.Item2!;
            }

            Console.WriteLine(output.Item1);
        }

        Reset();
    }

    public string Read(bool clearScreen = true)
    {
        if (output.Count > 0)
        {
            Display(clearScreen);
        }

        Console.CursorVisible = true;
        string input = Console.ReadLine() ?? string.Empty;
        Console.CursorVisible = false;

        return input;
    }

    public int ReadInt(bool clearScreen = true)
    {
        string input = Read(clearScreen);
        int result;

        return int.TryParse(input, out result) ? result : -1;
    }
    #endregion
}

using System;
using System.Collections.Generic;

namespace Garage
{
    /// <summary>
    /// Represents a simple console-based user interface.
    /// </summary>
    public class SimpleUI : IUI
    {
        #region Properties
        /// <summary>
        /// Event fired when the Up Arrow key is pressed.
        /// </summary>
        public event EventHandler? OnUpPressed;

        /// <summary>
        /// Event fired when the Down Arrow key is pressed.
        /// </summary>
        public event EventHandler? OnDownPressed;

        /// <summary>
        /// Event fired when the Enter key is pressed.
        /// </summary>
        public event EventHandler? OnEnterPressed;

        /// <summary>
        /// Event fired when the Escape key is pressed.
        /// </summary>
        public event EventHandler? OnEscapePressed;

        private readonly Dictionary<ConsoleKey, Action> KeyEvents;
        private List<(string, ConsoleColor?)> output = new List<(string, ConsoleColor?)>();
        private static SimpleUI instance = null!;
        #endregion

        #region Static
        /// <summary>
        /// Gets the singleton instance of the <see cref="SimpleUI"/> class.
        /// </summary>
        public static SimpleUI Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SimpleUI();
                }
                return instance;
            }
            private set { }
        }
        #endregion

        #region Constructors
        private SimpleUI()
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
        /// <summary>
        /// Listens for keyboard input and triggers corresponding events.
        /// </summary>
        /// <returns>The <see cref="ConsoleKey"/> representing the key pressed.</returns>
        public ConsoleKey KeyListener()
        {
            Console.CursorVisible = false;

            ConsoleKeyInfo keyPressed;
            bool eventActivation = false;

            Display(clearScreen: true);

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

        /// <summary>
        /// Resets the output buffer.
        /// </summary>
        public void Reset()
        {
            output = new List<(string, ConsoleColor?)>();
        }

        /// <summary>
        /// Writes text to the output buffer.
        /// </summary>
        /// <param name="output">The text to write.</param>
        /// <param name="color">The color of the text (optional).</param>
        public void Write(string output = "", ConsoleColor? color = null)
        {
            this.output.Add((output, color));
        }

        /// <summary>
        /// Displays the output buffer on the console.
        /// </summary>
        /// <param name="clearScreen">Flag indicating whether to clear the console before displaying (default is true).</param>
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

            if (output.Count == 0)
            {
                return;
            }

            foreach ((string, ConsoleColor?) output in this.output)
            {
                if (output.Item2 is not null)
                {
                    Console.ForegroundColor = (ConsoleColor)output.Item2!;
                }

                Console.WriteLine(output.Item1);
            }

            Reset();            // Reset the buffer. 
        }

        /// <summary>
        /// Reads a line of input from the console.
        /// </summary>
        /// <param name="clearScreen">Flag indicating whether to clear the console before reading (default is true).</param>
        /// <returns>The input string.</returns>
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

        /// <summary>
        /// Reads an integer input from the console.
        /// </summary>
        /// <param name="clearScreen">Flag indicating whether to clear the console before reading (default is true).</param>
        /// <returns>The integer input.</returns>
        public int ReadInt(bool clearScreen = true)
        {
            string input = Read(clearScreen);
            int result;

            return int.TryParse(input, out result) ? result : -1;
        }
        #endregion
    }
}

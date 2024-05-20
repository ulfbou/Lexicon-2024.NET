namespace Garage
{
    /// <summary>
    /// Represents a user interface.
    /// </summary>
    public interface IUI
    {
        /// <summary>
        /// Event raised when the Down arrow key is pressed.
        /// </summary>
        event EventHandler? OnDownPressed;

        /// <summary>
        /// Event raised when the Enter key is pressed.
        /// </summary>
        event EventHandler? OnEnterPressed;

        /// <summary>
        /// Event raised when the Escape key is pressed.
        /// </summary>
        event EventHandler? OnEscapePressed;

        /// <summary>
        /// Event raised when the Up arrow key is pressed.
        /// </summary>
        event EventHandler? OnUpPressed;

        /// <summary>
        /// Displays output on the user interface.
        /// </summary>
        /// <param name="clearScreen">Flag indicating whether to clear the screen before displaying output (default is true).</param>
        void Display(bool clearScreen = true);

        /// <summary>
        /// Listens for a key press event and returns the pressed key.
        /// </summary>
        /// <returns>The ConsoleKey representing the pressed key.</returns>
        ConsoleKey KeyListener();

        /// <summary>
        /// Reads input from the user interface.
        /// </summary>
        /// <param name="clearScreen">Flag indicating whether to clear the screen before reading input (default is true).</param>
        /// <returns>The input string.</returns>
        string Read(bool clearScreen = true);

        /// <summary>
        /// Reads an integer input from the user interface.
        /// </summary>
        /// <param name="clearScreen">Flag indicating whether to clear the screen before reading input (default is true).</param>
        /// <returns>The integer input.</returns>
        int ReadInt(bool clearScreen = true);

        /// <summary>
        /// Resets the user interface.
        /// </summary>
        void Reset();

        /// <summary>
        /// Writes output to the user interface.
        /// </summary>
        /// <param name="output">The string to write.</param>
        /// <param name="color">The color of the text (optional).</param>
        void Write(string output = "", ConsoleColor? color = null);
    }
}

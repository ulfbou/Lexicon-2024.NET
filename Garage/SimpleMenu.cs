namespace Garage
{
    /// <summary>
    /// Represents a simple console-based menu.
    /// </summary>
    public class SimpleMenu
    {
        private readonly List<IMenuItem> menuItems = new List<IMenuItem>();
        private int currentItemIndex;
        private bool terminateOnExit;
        private readonly IUI ui = SimpleUI.Instance;
        private ConsoleColor titleColor = ConsoleColor.Blue;
        private ConsoleColor foregroundColor = ConsoleColor.Green;
        private ConsoleColor highlightColor = ConsoleColor.Yellow;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMenu"/> class with the specified title and termination behavior.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="terminateOnExit">Flag indicating whether to terminate the application when the exit key is pressed (default is false).</param>
        public SimpleMenu(string title, bool terminateOnExit = false)
        {
            Title = title;
            this.terminateOnExit = terminateOnExit;
        }

        /// <summary>
        /// Gets the title of the menu.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Adds a menu item to the menu.
        /// </summary>
        /// <param name="menuItemTitle">The title of the menu item.</param>
        /// <param name="action">The action to execute when the menu item is selected.</param>
        /// <param name="confirm">Flag indicating whether to confirm the action when the menu item is selected (default is true).</param>
        /// <returns>The current instance of the <see cref="SimpleMenu"/> class.</returns>
        public SimpleMenu AddMenuItem(string menuItemTitle, Action action, bool confirm = true)
        {
            Action itemAction;

            if (confirm)
            {
                itemAction = () =>
                {
                    action();
                    ui.Write("Press return to continue, please.", ConsoleColor.White);
                    ui.Read();
                };
            }
            else
            {
                itemAction = action;
            }

            menuItems.Add(new MenuItem(menuItemTitle, itemAction));

            return this;
        }

        /// <summary>
        /// Adds a submenu to the menu.
        /// </summary>
        /// <param name="menuItemTitle">The title of the submenu item.</param>
        /// <param name="subMenu">The submenu to add.</param>
        /// <returns>The current instance of the <see cref="SimpleMenu"/> class.</returns>
        public SimpleMenu AddSubMenu(string menuItemTitle, SimpleMenu subMenu)
        {
            menuItems.Add(new SubMenuEntry(menuItemTitle, subMenu));

            return this;
        }

        /// <summary>
        /// Runs the menu and handles user input.
        /// </summary>
        /// <param name="exitKey">The key to exit the menu (default is Escape).</param>
        public void Run(ConsoleKey exitKey = ConsoleKey.Escape)
        {
            bool continueRunning = true;

            do
            {
                Show();

                ConsoleKey keyPressed = Console.ReadKey().Key;

                if (keyPressed == exitKey || (terminateOnExit && keyPressed == ConsoleKey.Escape))
                {
                    continueRunning = false;
                }

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    currentItemIndex = (currentItemIndex - 1 + menuItems.Count) % menuItems.Count;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    currentItemIndex = (currentItemIndex + 1) % menuItems.Count;
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    ExecuteSelectedItem();
                }
            }
            while (continueRunning);

            if (terminateOnExit)
            {
                ui.Write("Thank you for using the Garage simulator.");
                Environment.Exit(0);
            }
        }

        private void Show()
        {
            ui.Display(clearScreen: true);
            ui.Write(Title, highlightColor);
            ui.Write(new string('-', Title.Length));

            int index = 0;

            foreach (var menuItem in menuItems)
            {
                ConsoleColor color = (index == currentItemIndex) ? highlightColor : foregroundColor;
                ui.Write($"{menuItem.Title}", color);
                index++;
            }

            if (terminateOnExit)
            {
                ui.Write("Select an option or press ESC to exit.", foregroundColor);
            }
            else
            {
                ui.Write("Select an option.", foregroundColor);
            }

            ui.Display(clearScreen: false);
        }

        private void ExecuteSelectedItem()
        {
            if (menuItems.Count == 0)
                return;

            var menuItem = menuItems[currentItemIndex];
            menuItem.Execute();
        }
    }
}

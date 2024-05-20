using Microsoft.VisualBasic;
using System;
using System.Text;

namespace Garage;

public class ConsoleMenu
{
    #region Private Properties
    private List<ConsoleMenuItem> items = new List<ConsoleMenuItem>();
    private ConsoleMenu? parentMenu = null!;
    private ConsoleMenu? currentMenu = null;
    private ConsoleMenu? subMenu = null!;
    private int currentItemIndex;
    private bool terminateOnExit;
    #endregion

    #region Public Properties
    public string Title { get; private set; }
    public ConsoleColor HighlightColor { get; set; }
    public ConsoleColor HeaderColor { get; set; }
    public ConsoleColor ForegroundColor { get; set; }
    public ConsoleColor MenuItemColor { get; set; }
    #endregion

    #region Static 
    private static readonly ConsoleUI UI = ConsoleUI.Instance;

    static ConsoleMenu()
    {
        UI.Run();
    }
    #endregion

    #region Constructor
    public ConsoleMenu(string title, ConsoleMenu? parent = null, bool terminateOnExit = false)
    {
        Title = title;
        parentMenu = parent;

        this.terminateOnExit = terminateOnExit;

        if (parent == null)
        {
            HighlightColor = ConsoleColor.Yellow;
            HeaderColor = ConsoleColor.Blue;
            ForegroundColor = ConsoleColor.White;
            MenuItemColor = ConsoleColor.Green;
        }
        else
        {
            HighlightColor = parent.HighlightColor;
            HeaderColor = parent.HeaderColor;
            ForegroundColor = parent.ForegroundColor;
            MenuItemColor = parent.MenuItemColor;
        }
    }
    #endregion

    #region methods
    public ConsoleMenu AddItem(string itemTitle, Action itemAction, bool confirm = true)
    {
        items.Add(new ConsoleMenuItem(itemTitle, () =>
        {
            itemAction();

            if (confirm)
            {
                UI.Write("Push return to continue, please.");
                UI.Read();
            }
        }));

        return this;
    }

    public ConsoleMenu AddSubMenu(string? itemTitle, ConsoleMenu subMenu)
    {
        subMenu.parentMenu = this;

        items.Add(new ConsoleMenuItem(
            itemTitle ?? "*** Missing menu item title ***",
            () =>
            {
                subMenu.Run();
            }
            ));

        return this;
    }

    private void Subscribe()
    {
        UI.OnUpPressed += HandleUpPressed;
        UI.OnDownPressed += HandleDownPressed;
        UI.OnEnterPressed += HandleEnterPressed;
        UI.OnEscapePressed += HandleEscapePressed;
    }

    private void Unsubscribe()
    {
        UI.OnUpPressed -= HandleUpPressed;
        UI.OnDownPressed -= HandleDownPressed;
        UI.OnEnterPressed -= HandleEnterPressed;
        UI.OnEscapePressed -= HandleEscapePressed;
    }

    public void Run(ConsoleKey exitKey = ConsoleKey.Escape)
    {
        ConsoleKey keyPressed = ConsoleKey.None;

        if (terminateOnExit)
        {
            terminateOnExit = exitKey == ConsoleKey.Escape;
        }

        currentMenu = this;
        currentMenu.subMenu = null;

        Subscribe();

        do
        {
            Show();
            UI.Display();
            UI.Reset();
            keyPressed = UI.KeyListener();
        }
        while (keyPressed != exitKey);

        Unsubscribe();
    }

    private string DecorateTitle(string Title)
    {
        int maxLength = Title.Length;

        foreach (var item in items)
        {
            string itemString = item.ItemTitle;
            maxLength = Math.Max(maxLength, itemString.Length);
        }

        StringBuilder sb = new StringBuilder();
        string titleString = $" {Title} ";
        string leftPad = new string('=', Math.Max((maxLength - titleString.Length) / 2, 0));
        string rightPad = new string('=', Math.Max((maxLength - titleString.Length + 1) / 2, 0));


        string result = $"{leftPad}{titleString}{rightPad}";

        if (maxLength == result.Length)
        {
            return result;
        }
        else
        {
            return result;
        }
    }

    private void Show()
    {
        /* TODO: Add a hook to be run prior to showing the menu. */
        string title = DecorateTitle(Title);

        Console.ForegroundColor = HeaderColor;
        UI.Write(DecorateTitle(Title), HeaderColor);

        for (int i = 0; i < items.Count; i++)
        {
            ConsoleColor color = (currentItemIndex == i) ? HighlightColor : MenuItemColor;
            UI.Write(items[i].ItemTitle, color);
        }

        UI.Write(new string('=', title.Length), HeaderColor);

        UI.Write("Move up or down the menu.", ForegroundColor);
        UI.Write("Press return to select.");

        if (parentMenu != null || !terminateOnExit)
        {
            UI.Write("Press ESC to exit the menu.");
        }
        else
        {
            // The application only exits when there is no parent menu and it is set fo terminate on exit. 
            UI.Write("Press ESC to exit the program.");
        }
    }

    private void HandleUpPressed(object? sender, EventArgs args)
    {
        if (currentItemIndex > 0)
        {
            currentItemIndex--;
        }
        else
        {
            currentItemIndex = items.Count - 1;
        }
    }

    private void HandleDownPressed(object? sender, EventArgs args)
    {
        if (currentItemIndex < items.Count - 1)
        {
            currentItemIndex++;
        }
        else
        {
            currentItemIndex = 0;
        }
    }

    private void HandleEnterPressed(object? sender, EventArgs args)
    {
        if (currentItemIndex < 0 || currentItemIndex >= items.Count)
        {
            throw new InvalidOperationException(nameof(currentItemIndex));
        }

        Unsubscribe();
        items[currentItemIndex].ItemAction?.Invoke();
        Subscribe();
    }

    private void HandleEscapePressed(object? sender, EventArgs args)
    {
        if (parentMenu == null && terminateOnExit)
        {
            Environment.Exit(0);
        }
    }

    public override string ToString()
    {
        return Title;
    }

    #endregion
}
//onödigt krånglig syntax??

using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Deprecated;

public class NotSoSimpleMenu
{
    private readonly List<IMenuItem> menuItems = new List<IMenuItem>();
    private readonly NotSoSimpleMenu? parentMenu;
    private int currentItemIndex;
    private bool terminateOnExit;
    private static IUI ui = SimpleUI.Instance;
    private ConsoleColor foregroundColor = ConsoleColor.Green;
    private ConsoleColor highlightColor = ConsoleColor.Yellow;

    public NotSoSimpleMenu(string title, NotSoSimpleMenu? parent = null, bool terminateOnExit = false)
    {
        Title = title;
        parentMenu = parent;
        this.terminateOnExit = terminateOnExit;
    }

    public string Title { get; }
    public bool TerminateOnExit { get; }
    public ConsoleColor ForegroundColor
    {
        get => foregroundColor;
        set
        {
            foregroundColor = value;
        }
    }
    public ConsoleColor HighlightColor
    {
        get => highlightColor;
        set
        {
            highlightColor = value;
        }
    }

    public NotSoSimpleMenu AddMenuItem(string menuItemTitle, Action action, bool confirm = true)
    {
        Action itemAction;

        if (confirm)
        {
            itemAction = () =>
            {
                action();
                ui.Write("Push return to continue, please.", ConsoleColor.White);
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

    public NotSoSimpleMenu AddSubMenu(string menuItemTitle, SimpleMenu subMenu)
    {
        menuItems.Add(new SubMenuEntry(menuItemTitle, subMenu));

        return this;
    }

    public void Run(ConsoleKey exitKey = ConsoleKey.Escape)
    {
        bool continueRunning = true;

        Subscribe();

        do
        {
            Show();

            ConsoleKey keyPressed = ui.KeyListener();

            if (keyPressed == ConsoleKey.Escape || keyPressed == exitKey)
            {
                continueRunning = false;
            }
        }
        while (continueRunning);

        Unsubscribe();
    }

    private int subs = 0;

    private void Subscribe()
    {
        ui.OnUpPressed += HandleUpPressed;
        ui.OnDownPressed += HandleDownPressed;
        ui.OnEnterPressed += HandleEnterPressed;
        ui.OnEscapePressed += HandleEscapePressed;
        subs++;
    }

    private void Unsubscribe()
    {
        ui.OnUpPressed -= HandleUpPressed;
        ui.OnDownPressed -= HandleDownPressed;
        ui.OnEnterPressed -= HandleEnterPressed;
        ui.OnEscapePressed -= HandleEscapePressed;
        subs--;
    }

    private void Show()
    {
        ui.Display(clearScreen: true);
        ui.Write(Title);
        ui.Write(new string('-', Title.Length));

        int index = 0;

        foreach (var menuItem in menuItems)
        {
            ConsoleColor color = index == currentItemIndex ? HighlightColor : ForegroundColor;
            ui.Write($"{menuItem.Title}", color);
            index++;
        }

        ui.Write("Select an option or press ESC to exit.", ForegroundColor);
        ui.Display();
    }

    private void ExecuteSelectedItem()
    {
        if (menuItems.Count == 0)
            return;

        var menuItem = menuItems.ElementAt(currentItemIndex);
        menuItem.Execute();
    }

    private void HandleUpPressed(object? sender, EventArgs args)
    {
        currentItemIndex = (currentItemIndex - 1 + menuItems.Count) % menuItems.Count;
    }

    private void HandleDownPressed(object? sender, EventArgs args)
    {
        currentItemIndex = (currentItemIndex + 1) % menuItems.Count;
    }

    private void HandleEnterPressed(object? sender, EventArgs args)
    {
        ExecuteSelectedItem();
    }

    private void HandleEscapePressed(object? sender, EventArgs args)
    {
        if (TerminateOnExit)
        {
            ui.Write("Thank you for visiting!", ConsoleColor.Green);
            ui.Display();
            Environment.Exit(0);
        }
    }
}

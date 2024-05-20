namespace Garage;

/// <summary>
/// Represents a menu item.
/// </summary>
public interface IMenuItem
{
    /// <summary>
    /// Gets the title of the menu item.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Executes the action associated with the menu item.
    /// </summary>
    void Execute();
}

/// <summary>
/// Represents a basic menu item implementation.
/// </summary>
public class MenuItem : IMenuItem
{
    /// <summary>
    /// Gets the title of the menu item.
    /// </summary>
    public string Title { get; }

    private readonly Action action;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItem"/> class with the specified title and action.
    /// </summary>
    /// <param name="title">The title of the menu item.</param>
    /// <param name="action">The action to execute when the menu item is selected.</param>
    public MenuItem(string title, Action action)
    {
        Title = title;
        this.action = action ?? throw new ArgumentNullException(nameof(action));
    }

    /// <summary>
    /// Executes the action associated with the menu item.
    /// </summary>
    public void Execute()
    {
        action.Invoke();
    }
}

/// <summary>
/// Represents a submenu entry in a menu.
/// </summary>
public class SubMenuEntry : IMenuItem
{
    /// <summary>
    /// Gets the title of the submenu entry.
    /// </summary>
    public string Title { get; }

    private readonly SimpleMenu subMenu;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubMenuEntry"/> class with the specified title and submenu.
    /// </summary>
    /// <param name="title">The title of the submenu entry.</param>
    /// <param name="subMenu">The submenu associated with the entry.</param>
    public SubMenuEntry(string title, SimpleMenu subMenu)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        this.subMenu = subMenu ?? throw new ArgumentNullException(nameof(subMenu));
    }

    /// <summary>
    /// Executes the submenu associated with the entry.
    /// </summary>
    public void Execute()
    {
        subMenu.Run();
    }
}

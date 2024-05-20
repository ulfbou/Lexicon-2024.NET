namespace Garage;

public class ConsoleMenuItem
{
    #region Properties

    public int ID { get; set; }
    public string ItemTitle { get; set; }
    public Action ItemAction { get; set; }

    #endregion

    #region Constructors

    public ConsoleMenuItem(string text, Action action)
    {
        //this.ID = id;
        this.ItemTitle = text;
        this.ItemAction = action;
    }

    #endregion

    public override string ToString()
    {
        return ItemTitle;
    }
}

namespace LMS.api.Model
{
    public interface IEntity
    {
        string Id { get; set; }
        DateTime Created { get; set; }
        string SearchableString { get; }
    }

    public interface IEntity<TKey> where TKey : notnull
    {
        TKey Id { get; set; }
        DateTime Created { get; set; }
        string SearchableString { get; }
    }
}

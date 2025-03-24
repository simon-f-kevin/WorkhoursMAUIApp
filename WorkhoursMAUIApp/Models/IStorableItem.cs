using SQLite;

namespace WorkhoursMAUIApp.Models;

public interface IStorableItem
{
    public int? Id { get; set; }
}

public abstract class StorableItem : IStorableItem
{
    [PrimaryKey, AutoIncrement]
    public int? Id { get; set; }
}
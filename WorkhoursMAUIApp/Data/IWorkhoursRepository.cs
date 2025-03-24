using WorkhoursMAUIApp.Models;

namespace WorkhoursMAUIApp.Data;

public interface IWorkhoursRepository<T> where T : StorableItem
{
    public (bool, T) Upsert(T item);

    public T GetById(int id);
}
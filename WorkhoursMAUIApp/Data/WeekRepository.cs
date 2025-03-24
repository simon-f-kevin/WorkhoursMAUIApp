using WorkhoursMAUIApp.Models;

namespace WorkhoursMAUIApp.Data;

public class WeekRepository : IWorkhoursRepository<WeekItem>
{
    private readonly WorkhoursDatabase _workhoursDatabase;

    public WeekRepository(WorkhoursDatabase workhoursDatabase)
    {
        _workhoursDatabase = workhoursDatabase;
    }

    public IEnumerable<WeekItem> GetAll()
    {
        return Task.Run(_workhoursDatabase.GetWeekItems).Result;
    }

    public WeekItem GetById(int id)
    {
        return Task.Run(async () => await _workhoursDatabase.GetWeekItem(id)).Result;
    }

    public (bool, WeekItem) Upsert(WeekItem item)
    {
        WeekItem storedItem;
        if (item.Id.HasValue)
        {
            storedItem = Task.Run(async () => await _workhoursDatabase.GetWeekItem(item.Id.Value)).Result;;
        }
        else
        {
            storedItem = item;
            storedItem.Id = Task.Run(async () => await _workhoursDatabase.InsertWeekItem(item)).Result;
            return (true, storedItem);
        }
        storedItem.Name = item.Name;
        storedItem.TotalHours = item.TotalHours;
        storedItem.WeekNumber = item.WeekNumber;
        storedItem.Id = Task.Run(async () => await _workhoursDatabase.UpdateWeekItem(storedItem)).Result;
        return (true, storedItem);
    }
}
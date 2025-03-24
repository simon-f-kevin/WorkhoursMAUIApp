using System.Threading.Tasks;
using WorkhoursMAUIApp.Models;

namespace WorkhoursMAUIApp.Data;

public class WorkdayRepository : IWorkhoursRepository<DayItem>
{
    private readonly WorkhoursDatabase _workhoursDatabase;

    public WorkdayRepository(WorkhoursDatabase workhoursDatabase)
    {
        _workhoursDatabase = workhoursDatabase;
    }

    public (bool, DayItem) Upsert(DayItem item)
    {
        DayItem storedItem;
        if (item.Id.GetValueOrDefault() != 0)
        {
            storedItem = Task.Run(async () => await _workhoursDatabase.GetDayItem(item.Id.GetValueOrDefault())).Result;
        }
        else
        {
            storedItem = item;
            storedItem.Id = Task.Run(async () => await _workhoursDatabase.InsertDayItem(item)).Result;
            return (true, storedItem);
        }
        storedItem.BreakMinutes = item.BreakMinutes;
        storedItem.HoursWorked = item.HoursWorked;
        storedItem.MinutesWorked = item.MinutesWorked;
        storedItem.Id = Task.Run(async () => await _workhoursDatabase.UpdateDayItem(item)).Result;
        return (true, storedItem);
    }

    public DayItem GetById(int id)
    {
        return Task.Run(async () => await _workhoursDatabase.GetDayItem(id)).Result;
    }

    public IEnumerable<DayItem> GetByWeekId(int weekId)
    {
        var all = Task.Run(_workhoursDatabase.GetDayItems).Result;
        return all.Where(i => i.WeekId == weekId);
    }
}
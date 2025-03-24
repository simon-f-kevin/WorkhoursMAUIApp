using System.Diagnostics;
using SQLite;
using WorkhoursMAUIApp.Models;

namespace WorkhoursMAUIApp.Data;

public class WorkhoursDatabase
{
    private SQLiteAsyncConnection database;

    async Task Init()
    {
        if (database is not null)
        {
            return;
        }
        database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await database.CreateTableAsync<DayItem>().ContinueWith(res =>
        {
            Debug.WriteLine(res.Result.ToString());
        });
        await database.CreateTableAsync<WeekItem>().ContinueWith(res =>
        {
            Debug.WriteLine(res.Result.ToString());
        });
    }

    public async Task<DayItem> GetDayItem(int id)
    {
        await Init();
        return await database.Table<DayItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> InsertDayItem(DayItem item)
    {
        await Init();
        return await database.InsertAsync(item);
    }

    public async Task<IEnumerable<DayItem>> GetDayItems()
    {
        await Init();
        return await database.Table<DayItem>().ToListAsync();
    }

    public async Task<int> UpdateDayItem(DayItem item)
    {
        await Init();
        return await database.UpdateAsync(item);
    }

    public async Task<WeekItem> GetWeekItem(int id)
    {
        await Init();
        return await database.Table<WeekItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> InsertWeekItem(WeekItem item)
    {
        await Init();
        return await database.InsertAsync(item);
    }

    public async Task<int> UpdateWeekItem(WeekItem item)
    {
        await Init();
        return await database.UpdateAsync(item);
    }

    public async Task<IEnumerable<WeekItem>> GetWeekItems()
    {
        await Init();
        return await database.Table<WeekItem>().ToListAsync();
    }
}
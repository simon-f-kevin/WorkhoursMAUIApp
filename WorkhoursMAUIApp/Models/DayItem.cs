
namespace WorkhoursMAUIApp.Models;
public class DayItem : StorableItem
{
    public DayItem()
    {

    }

    public DayItem(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
    public double HoursWorked { get; set; }
    public double MinutesWorked { get; set; }
    public double BreakMinutes { get; set; }
    public int WeekId { get; set; }
    public string WorkdayStart { get; set; }
    public string LunchStart { get; set; }
    public string LunchEnd { get; set; }
    public string WorkdayEnd { get; set; }

    public string TimeWorkedDisplayText { get
        {
            return $"{HoursWorked} timmar {MinutesWorked} minuter";
        }
    }
}
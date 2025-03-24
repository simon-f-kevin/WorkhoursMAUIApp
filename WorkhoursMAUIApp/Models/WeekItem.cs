namespace WorkhoursMAUIApp.Models;
public class WeekItem : StorableItem
{
    public WeekItem()
    {
        
    }
    
    public WeekItem(int weekNumber)
    {
        Name = $"Week {weekNumber}";
        WeekNumber = weekNumber;
    }
    public string Name { get; set; }
    public int WeekNumber { get; set; }
    public string TotalHoursWorkedText { get {
            var hours = (int)TotalHours;
            var minutes = (int)((TotalHours - hours) * 60);
            return $"{hours} timmar {minutes} minuter";
    } }
    public double TotalHours { get; set; }
}
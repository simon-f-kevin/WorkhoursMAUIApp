public static class WorktimeCalculator
{
    public static WorktimeCalculatorResult Calculate(TimeOnly dayStart, TimeOnly dayEnd, TimeOnly breakStart, TimeOnly breakEnd)
    {
        
        var breakMinutes = breakEnd - breakStart;
        var totalHours = (dayEnd - dayStart) - breakMinutes;
        return new WorktimeCalculatorResult()
        {
            Hours = totalHours.Hours,
            Minutes = totalHours.Minutes,
            BreakMinutes = breakMinutes.TotalMinutes
        };
    }
}
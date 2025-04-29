using System;
using WorkhoursMAUIApp.Models;

namespace WorkhoursTests.Models;

public class WeekItemTests
{
    public string GetTotalHoursWorked(double totalHours)
    {
        var hours = (int)totalHours;
        var remaining = totalHours - hours;
        var minutes = Math.Round(remaining * 60);
        return $"{hours} timmar {minutes} minuter";
    }

    [Fact]
    public void TotalHoursWorkedText()
    {
        var expectedTotalHoursWorkedText = "40 timmar 43 minuter";

        var totalHoursWorkedText = GetTotalHoursWorked(40.71666666666666);

        Assert.Equal(totalHoursWorkedText, expectedTotalHoursWorkedText);
    }
}

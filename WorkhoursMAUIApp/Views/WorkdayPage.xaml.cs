using WorkhoursMAUIApp.Data;
using WorkhoursMAUIApp.Models;

namespace WorkhoursMAUIApp.Views;

public partial class WorkdayPage : ContentPage
{

	TimeOnly _workDayStart = TimeOnly.MinValue;
	TimeOnly _workDayEnd = TimeOnly.MinValue;
	TimeOnly _lunchStart = TimeOnly.MinValue;
	TimeOnly _lunchEnd = TimeOnly.MinValue;

	public string _weekDayNameText;
    private readonly int? _workdayId;
    private readonly int _weekNumber;
    private readonly IWorkhoursRepository<DayItem> _workdayRepository;
    private WorktimeCalculatorResult? _worktimeResult;

    public WorkdayPage(string weekDayName, int? workdayId, int weekId, IWorkhoursRepository<DayItem> workdayRepository)
	{
		InitializeComponent();
		BindingContext = this;
		_weekDayNameText = weekDayName;
        _workdayId = workdayId;
        _weekNumber = weekId;
        _workdayRepository = workdayRepository;
    }

	protected override void OnNavigatedTo(Microsoft.Maui.Controls.NavigatedToEventArgs args)
	{
		WeekdayName.Text = _weekDayNameText;
		if (_workdayId.HasValue)
		{
			var storedWorkday = _workdayRepository.GetById(_workdayId.Value);
			WorkdayStart.Text = storedWorkday.WorkdayStart;
			ValidateTimeField(WorkdayStart, EventArgs.Empty);
			LunchStart.Text = storedWorkday.LunchStart;
			ValidateTimeField(LunchStart, EventArgs.Empty);
			LunchEnd.Text = storedWorkday.LunchEnd;
			ValidateTimeField(LunchEnd, EventArgs.Empty);
			WorkdayEnd.Text = storedWorkday.WorkdayEnd;
			ValidateTimeField(WorkdayEnd, EventArgs.Empty);
		}
		
		base.OnNavigatedTo(args);
	}

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
    }

    public void ValidateTimeField(object sender, EventArgs e)
    {
        if (sender is Entry senderEntry)
        {
            var timeText = senderEntry.Text;
            if (string.IsNullOrEmpty(timeText))
            {
                return;
            }
            ValidateTime(timeText, senderEntry);
        }
    }

    private async void ValidateTime(string timeText, Entry senderEntry) {
		TimeOnly currentValue = TimeOnly.MinValue;
		var dotFormatValidResult = TimeOnly.TryParseExact(timeText, "HH.mm", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out var _currentValueDot);
		var colonFormatValidResult = TimeOnly.TryParseExact(timeText, "HH:mm", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out var _currentValueColon);
		var generalValidResult = TimeOnly.TryParse(timeText, out var _currentValueGeneral);
		if (!dotFormatValidResult && !colonFormatValidResult && !generalValidResult)
		{
			if (senderEntry != null)
			{
				var parent = senderEntry.Parent as VerticalStackLayout;
				Label? label;
				if (parent != null)
				{
					label = parent.Children.OfType<Label>().FirstOrDefault(c => c.StyleId.StartsWith(senderEntry.StyleId));
					await DisplayAlert("Ajaj!", $"Ogitlig tid för {label?.Text} '{timeText}'", "OK");
				}
			}
			
		}
		if (_currentValueDot > currentValue)
		{
			currentValue = _currentValueDot;
		}
		else if (_currentValueColon > currentValue)
		{
			currentValue = _currentValueColon;
		}
		else
		{
			currentValue = _currentValueGeneral;
		}
		switch (senderEntry.StyleId)
		{
			case "WorkdayStart":
				_workDayStart = currentValue;
				break;
			case "LunchStart":
				_lunchStart = currentValue;
				break;
			case "LunchEnd":
				_lunchEnd = currentValue;
				break;
			case "WorkdayEnd":
				_workDayEnd = currentValue;
				break;
			default:
				break;
		}

	}

	public void OnSubmitTimesBtnClicked(object sender, EventArgs e)
	{
		var worktimeResult = WorktimeCalculator.Calculate(_workDayStart, _workDayEnd, _lunchStart, _lunchEnd);

		var hoursWorkedText = HoursWorked.Text;
		var res = hoursWorkedText.Split(':');
		var timePart = $": {worktimeResult.Hours} h {worktimeResult.Minutes} min";
		HoursWorked.Text = string.Concat(res[0], timePart);

		var minutesLunchText = MinutesLunch.Text;
		res = minutesLunchText.Split(':');
		timePart = $": {worktimeResult.BreakMinutes} minuter";
		MinutesLunch.Text = string.Concat(res[0], timePart);
		_worktimeResult = worktimeResult;
		MainThread.InvokeOnMainThreadAsync(() =>
		{
			var res = _workdayRepository.Upsert(new DayItem(_weekDayNameText)
			{
				Id = _workdayId.GetValueOrDefault(),
				HoursWorked = _worktimeResult.Hours,
				MinutesWorked = _worktimeResult.Minutes,
				BreakMinutes = _worktimeResult.BreakMinutes,
				WorkdayStart = _workDayStart.ToString(),
				LunchStart = _lunchStart.ToString(),
				LunchEnd = _lunchEnd.ToString(),
				WorkdayEnd = _workDayEnd.ToString(),
				WeekId = _weekNumber
			});
		});
	}
}
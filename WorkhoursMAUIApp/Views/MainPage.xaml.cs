namespace WorkhoursMAUIApp;

public partial class MainPage : ContentPage
{
	TimeOnly _workDayStart = TimeOnly.MinValue;
	TimeOnly _workDayEnd = TimeOnly.MinValue;
	TimeOnly _lunchStart = TimeOnly.MinValue;
	TimeOnly _lunchEnd = TimeOnly.MinValue;

	public MainPage()
	{
		InitializeComponent();
	}

	public async void ValidateTimeField(object sender, EventArgs e)
	{
		if (sender is Entry senderEntry)
		{
			var timeText = senderEntry.Text;
			TimeOnly currentValue = TimeOnly.MinValue;
			var dotFormatValidResult = TimeOnly.TryParseExact(timeText, "HH.mm", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out var _currentValueDot);
			var colonFormatValidResult = TimeOnly.TryParseExact(timeText, "HH:mm", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out var _currentValueColon);
			var generalValidResult = TimeOnly.TryParse(timeText, out var _currentValueGeneral);
			if (!dotFormatValidResult && !colonFormatValidResult && !generalValidResult)
			{
				var parent = senderEntry.Parent as VerticalStackLayout;
				Label label;
				if (parent != null)
				{
					label = parent.Children.OfType<Label>().FirstOrDefault(c => c.StyleId.StartsWith(senderEntry.StyleId));
					await DisplayAlert("Ajaj!", $"Ogitlig tid för {label.Text} '{timeText}'", "OK");
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
	}
}


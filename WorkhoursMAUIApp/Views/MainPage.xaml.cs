using System.Collections.ObjectModel;
using Windows.System;
using WorkhoursMAUIApp.Data;
using WorkhoursMAUIApp.Models;
using WorkhoursMAUIApp.ViewModels;

namespace WorkhoursMAUIApp.Views;

public partial class MainPage : ContentPage
{
    private readonly IWorkhoursRepository<DayItem> _workdayRepository;
	private readonly WeekRepository _weekRepository;

    public ObservableCollection<WeekItem> WeekItems { get; set; } = new ObservableCollection<WeekItem>();

	public MainPage(IWorkhoursRepository<DayItem> workdayRepository, IWorkhoursRepository<WeekItem> weekRepository)
	{
		InitializeComponent();
		BindingContext = this;
		// WeekItems.Add(new WeekItem(1) { TotalHours = 40 });
		_workdayRepository = workdayRepository;
		_weekRepository = (WeekRepository)weekRepository;
    }

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		var weekItems = _weekRepository.GetAll();
		if (!weekItems.Any())
		{
			var firstWeekItem = new WeekItem(System.Globalization.ISOWeek.GetWeekOfYear(DateTime.Now));
			WeekItems.Add(firstWeekItem);
			_weekRepository.Upsert(firstWeekItem);
		}
		foreach (var weekItem in weekItems)
		{
			WeekItems.Add(weekItem);
		}
		base.OnNavigatedTo(args);
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        WeekItems.Clear();
        base.OnNavigatedFrom(args);
    }


	public void OnCreateWeekBtnClicked(object sender, EventArgs e)
	{
		var weekItem = new WeekItem(WeekItems.Last().WeekNumber + 1);
		WeekItems.Add(weekItem);
		_weekRepository.Upsert(weekItem);
	}

	public async void OnWeekSelected(object sender, SelectionChangedEventArgs args)
	{
		var cv = (CollectionView)sender;
		if (cv.SelectedItem == null)
			return;

		if (args.CurrentSelection != null)
		{
			var selectedItems = args.CurrentSelection;
			var selectedWeek = selectedItems.FirstOrDefault() as WeekItem;
			WeekPage weekPage = new WeekPage(selectedWeek, _workdayRepository, _weekRepository);
			await Navigation.PushAsync(weekPage);
		}

		cv.SelectedItem = null;
	}
}


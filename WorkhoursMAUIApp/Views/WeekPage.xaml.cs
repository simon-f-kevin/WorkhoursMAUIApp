using System.Collections.ObjectModel;
using WorkhoursMAUIApp.Data;
using WorkhoursMAUIApp.Models;

namespace WorkhoursMAUIApp.Views;

public partial class WeekPage : ContentPage
{
    private readonly int _weekNumber;
    private readonly int? _weekId;
    private readonly WorkdayRepository _workdayRepository;
    private readonly WeekRepository _weekRepository;

    public ObservableCollection<DayItem> Workdays { get; set; } = new ObservableCollection<DayItem>();

    public WeekPage(WeekItem currentWeek, IWorkhoursRepository<DayItem> repository, IWorkhoursRepository<WeekItem> weekRepository)
    {
        InitializeComponent();
        BindingContext = this;

        _weekNumber = currentWeek.WeekNumber;
        _weekId = currentWeek.Id;
        _workdayRepository = (WorkdayRepository)repository;
        _weekRepository = (WeekRepository)weekRepository;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        var workdaysForWeek = _workdayRepository.GetByWeekId(_weekNumber);

        Workdays.Add(workdaysForWeek.FirstOrDefault(d => d.Name == "Måndag", new DayItem("Måndag")));
        Workdays.Add(workdaysForWeek.FirstOrDefault(d => d.Name == "Tisdag", new DayItem("Tisdag")));
        Workdays.Add(workdaysForWeek.FirstOrDefault(d => d.Name == "Onsdag", new DayItem("Onsdag")));
        Workdays.Add(workdaysForWeek.FirstOrDefault(d => d.Name == "Torsdag", new DayItem("Torsdag")));
        Workdays.Add(workdaysForWeek.FirstOrDefault(d => d.Name == "Fredag", new DayItem("Fredag")));

        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        //todo: sum all workdays and update the current weeks total hours
        var currentWeek = _weekRepository.GetById(_weekId.GetValueOrDefault());
        currentWeek.TotalHours = WorktimeCalculator.GetTotalHoursWorkedForWeek(Workdays);
        _weekRepository.Upsert(currentWeek);
        Workdays.Clear();
        base.OnNavigatedFrom(args);
    }

    public async void OnDaySelected(object sender, SelectionChangedEventArgs args)
    {
        var cv = (CollectionView)sender;
        if (cv.SelectedItem == null)
            return;

        if (args.CurrentSelection != null)
        {
            var selectedItems = args.CurrentSelection;
            var selectedDay = selectedItems.FirstOrDefault() as DayItem;
            WorkdayPage pageData = new WorkdayPage(selectedDay?.Name, selectedDay.Id, _weekNumber, _workdayRepository);
            await Navigation.PushAsync(pageData);
        }

        cv.SelectedItem = null;
    }
}
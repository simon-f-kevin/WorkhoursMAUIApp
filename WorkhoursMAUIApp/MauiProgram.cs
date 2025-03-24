using Microsoft.Extensions.Logging;
using WorkhoursMAUIApp.Data;
using WorkhoursMAUIApp.Models;
using WorkhoursMAUIApp.Views;

namespace WorkhoursMAUIApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		//repositories
		builder.Services.AddTransient<IWorkhoursRepository<DayItem>, WorkdayRepository>();
		builder.Services.AddTransient<IWorkhoursRepository<WeekItem>, WeekRepository>();

		//database
		builder.Services.AddSingleton<WorkhoursDatabase>();

		//views
		builder.Services.AddSingleton<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

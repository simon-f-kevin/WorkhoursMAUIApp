using WorkhoursMAUIApp.CustomControls;

namespace WorkhoursMAUIApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping("LabelCustomization", (handler, view) =>
		{
			if (view is SelectableLabel)
			{
#if ANDROID
				handler.PlatformView.SetTextIsSelectable(true);
#elif WINDOWS
				handler.PlatformView.IsTextSelectionEnabled = true;
		#endif
			}
		});
	}

	

	protected override Window CreateWindow(IActivationState? activationState)
	{
		const int newheight = 1000;
		const int newwidth = 1290;

		var wins = new Window(new AppShell());
		wins.Height = wins.MinimumHeight = wins.MaximumHeight = newheight;
		wins.Width = wins.MinimumWidth = wins.MaximumWidth = newwidth;
		return wins;
	}
}

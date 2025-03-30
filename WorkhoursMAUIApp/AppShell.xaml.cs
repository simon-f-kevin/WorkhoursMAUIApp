namespace WorkhoursMAUIApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		BindingContext = this;
	}

	public string HomepageMessage
	{
		get
		{
			var currentTime = DateTime.Now.Hour;
			if (currentTime >= 0 && currentTime <= 7)
			{
				return "Good night!";
			}
			else if (currentTime >= 7 && currentTime <= 10)
			{
				return "Good morning!";
			}
			else if (currentTime >= 10 && currentTime <= 18)
			{
				return "Good day!";
			}
			else
			{
				return "Good evening!";
			}
		}
	}
}

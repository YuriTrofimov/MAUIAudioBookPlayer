using MauiAudioBookPlayer.Views;

namespace MauiAudioBookPlayer;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("scan", typeof(ManageScanFolders));
		Routing.RegisterRoute("scan/explorer", typeof(SelectFolderPage));
		Routing.RegisterRoute("library", typeof(LibraryPage));
		Routing.RegisterRoute("library/player", typeof(BookPlayerPage));
	}
}

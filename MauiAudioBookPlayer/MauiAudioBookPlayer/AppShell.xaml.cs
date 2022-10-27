using MauiAudioBookPlayer.Pages;

namespace MauiAudioBookPlayer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("ManageScanFolders", typeof(ManageScanFolders));
    }
}

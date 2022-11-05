// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MauiAudioBookPlayer.Views;

namespace MauiAudioBookPlayer;

/// <summary>
/// AppShell class.
/// </summary>
public partial class AppShell : Shell
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AppShell"/> class.
	/// </summary>
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("scan", typeof(ManageScanFolders));
		Routing.RegisterRoute("scan/explorer", typeof(SelectFolderPage));
		Routing.RegisterRoute("library", typeof(LibraryPage));
		Routing.RegisterRoute("library/player", typeof(BookPlayerPage));
	}
}
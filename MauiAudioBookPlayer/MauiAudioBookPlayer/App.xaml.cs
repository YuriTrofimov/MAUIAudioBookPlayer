// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MauiAudioBookPlayer;

/// <summary>
/// Application class.
/// </summary>
public partial class App : Application
{
	/// <summary>
	/// Initializes a new instance of the <see cref="App"/> class.
	/// </summary>
	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
	}

	/// <summary>
	/// On application start.
	/// </summary>
	protected override void OnStart()
	{
		base.OnStart();
	}
}
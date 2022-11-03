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
		InitApp();
	}

	private async void InitApp()
	{
		var result = await CheckStoragePermissions();
		if (result != PermissionStatus.Granted)
		{
		}
	}

	private async Task<PermissionStatus> CheckStoragePermissions()
	{
		var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

		if (status == PermissionStatus.Granted)
		{
			return status;
		}

		if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
		{
			// Prompt the user to turn on in settings
			// On iOS once a permission has been denied it may not be requested again from the application
			return status;
		}

		if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
		{
			// Prompt the user with additional information as to why the permission is needed
		}

		status = await Permissions.RequestAsync<Permissions.StorageWrite>();
		return status;
	}
}
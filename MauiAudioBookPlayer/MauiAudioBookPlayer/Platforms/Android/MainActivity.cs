// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace MauiAudioBookPlayer;

/// <summary>
/// Main android version app activity.
/// </summary>
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	private static MainActivity instance;

	/// <summary>
	/// Gets main activity instance.
	/// </summary>
	public static MainActivity Instance => instance;

	/// <summary>
	/// Called when the activity is first created.
	/// This is where you should do all of your normal static set up: create views, bind data to lists, etc.
	/// This method also provides you with a Bundle containing the activity's previously frozen state,
	/// if there was one.
	/// Always followed by onStart().
	/// </summary>
	/// <param name="savedInstanceState">The previous instance had generated from onSaveInstanceState(Bundle).</param>
	protected override void OnCreate(Bundle savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		instance = this;

		if (IsPermissionRequired())
		{
			RequestPermissions();
		}
	}

	/// <summary>
	/// Request all required permissions.
	/// </summary>
	private void RequestPermissions()
	{
		if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
		{
			RequestPermissionsAbove11();
		}
		else
		{
			RequestPermissionsBelow11();
		}
	}

	private bool IsPermissionRequired()
	{
		string permission = Manifest.Permission.WriteExternalStorage;
		if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
		{
			permission = Manifest.Permission.ManageExternalStorage;
		}

		var result = ContextCompat.CheckSelfPermission(this, permission);
		return result != Permission.Granted;
	}

	/// <summary>
	/// Request permissions for android versions above or equal 11(R).
	/// </summary>
	private void RequestPermissionsAbove11()
	{
		if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ManageExternalStorage))
		{
			var msg = "Manage external storage permissions is required. Please add it.";
			Toast.MakeText(this, msg, ToastLength.Short).Show();
			return;
		}

		ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.ManageExternalStorage }, 100);
	}

	/// <summary>
	/// Request permissions for android versions below 11(R).
	/// </summary>
	private void RequestPermissionsBelow11()
	{
		if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage))
		{
			var msg = "Storage write permissions is required. Please add it.";
			Toast.MakeText(this, msg, ToastLength.Short).Show();
			return;
		}

		ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.WriteExternalStorage }, 100);
	}
}
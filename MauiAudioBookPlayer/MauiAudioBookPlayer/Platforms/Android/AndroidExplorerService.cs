// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Android.Content;
using Android.OS;
using Android.OS.Storage;
using AudioBookPlayer.Core.Services.ExplorerService;
using Environment = Android.OS.Environment;

namespace MauiAudioBookPlayer.Platforms.Android
{
	/// <summary>
	/// Android implmentation of ExplorerService.
	/// </summary>
	public class AndroidExplorerService : ExplorerService
	{
		/// <summary>
		/// Returns folder children items.
		/// </summary>
		/// <param name="folderPath">Folder path.</param>
		/// <returns>Folder children items.</returns>
		public override List<ExplorerItem> LoadDirectoryItems(string folderPath)
		{
			if (string.IsNullOrEmpty(folderPath))
			{
				return GetChildren(GetRootFolder());
			}

			return GetChildren(folderPath);
		}

		private string GetRootFolder()
		{
			var context = MainActivity.Instance;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
			{
				StorageManager storageManager = (StorageManager)context.GetSystemService(Context.StorageService);
#pragma warning disable CA1416 // Validate platform compatibility
				return storageManager.PrimaryStorageVolume.Directory.AbsolutePath;
#pragma warning restore CA1416 // Validate platform compatibility
			}
			else
			{
#pragma warning disable CS0618 // Type or member is obsolete
				return Environment.ExternalStorageDirectory.AbsolutePath;
#pragma warning restore CS0618 // Type or member is obsolete
			}
		}
	}
}
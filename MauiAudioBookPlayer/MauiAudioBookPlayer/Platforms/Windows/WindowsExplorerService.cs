// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AudioBookPlayer.Core.Services.ExplorerService;

namespace MauiAudioBookPlayer.Platforms.Windows
{
	/// <summary>
	/// Windows implementation of ExplorerService.
	/// </summary>
	public class WindowsExplorerService : ExplorerService
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
				var result = new List<ExplorerItem>();
				foreach (var drive in Environment.GetLogicalDrives())
				{
					result.Add(new FolderItem(drive, null));
				}

				return result;
			}
			else
			{
				return GetChildren(folderPath);
			}
		}
	}
}
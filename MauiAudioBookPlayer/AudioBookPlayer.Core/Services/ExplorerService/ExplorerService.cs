// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace AudioBookPlayer.Core.Services.ExplorerService
{
	/// <summary>
	/// File system explorer service.
	/// </summary>
	public abstract class ExplorerService : IExplorerService
	{
		/// <summary>
		/// Returns folder children items.
		/// </summary>
		/// <param name="folderPath">Folder path.</param>
		/// <returns>Folder children items.</returns>
		public abstract List<ExplorerItem> LoadDirectoryItems(string folderPath);

		/// <summary>
		/// Get directory children items.
		/// </summary>
		/// <param name="path">Directory path.</param>
		/// <returns>List of directory children items.</returns>
		protected List<ExplorerItem> GetChildren(string path)
		{
			var result = new List<ExplorerItem>();
			foreach (var dir in Directory.GetDirectories(path))
			{
				result.Add(new FolderItem(dir, path));
			}

			foreach (var file in Directory.GetFiles(path))
			{
				result.Add(new FileItem(file));
			}

			return result;
		}
	}
}
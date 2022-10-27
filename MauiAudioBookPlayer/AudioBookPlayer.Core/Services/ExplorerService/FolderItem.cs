// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace AudioBookPlayer.Core.Services.ExplorerService
{
	/// <summary>
	/// Folder explorer item.
	/// </summary>
	public class FolderItem : ExplorerItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FolderItem"/> class.
		/// </summary>
		/// <param name="path">Folder path.</param>
		public FolderItem(string path)
			: base(path)
		{
			Name = new DirectoryInfo(path).Name;
		}

		/// <summary>
		/// Get current item children.
		/// </summary>
		/// <returns>List of current item children.</returns>
		public List<ExplorerItem> GetChildren()
		{
			var result = new List<ExplorerItem>();
			foreach (var dir in Directory.GetDirectories(Path))
			{
				result.Add(new FolderItem(dir));
			}

			foreach (var file in Directory.GetFiles(Path))
			{
				result.Add(new FileItem(file));
			}

			return result;
		}
	}
}
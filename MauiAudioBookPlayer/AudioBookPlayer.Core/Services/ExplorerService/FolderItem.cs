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
		public FolderItem()
			: base(string.Empty)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FolderItem"/> class.
		/// </summary>
		/// <param name="path">Folder path.</param>
		/// <param name="parentPath">Folder parent path.</param>
		public FolderItem(string path, string parentPath)
			: base(path)
		{
			Name = new DirectoryInfo(path).Name;
			ParentPath = parentPath;
		}

		/// <summary>
		/// Gets or sets path to parent directory.
		/// </summary>
		public string? ParentPath { get; set; }
	}
}
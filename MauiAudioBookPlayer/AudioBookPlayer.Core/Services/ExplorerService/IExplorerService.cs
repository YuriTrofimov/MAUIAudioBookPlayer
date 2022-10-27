﻿// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AudioBookPlayer.Core.Services.ExplorerService
{
	/// <summary>
	/// File system explorer service interface.
	/// </summary>
	public interface IExplorerService
	{
		/// <summary>
		/// Returns folder explorer item.
		/// </summary>
		/// <param name="folderPath">Folder path.</param>
		/// <returns>Folder item.</returns>
		FolderItem LoadFolder(string folderPath);
	}
}
﻿// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using AudioBookPlayer.Core.Model.Entities;

namespace AudioBookPlayer.Core.Model
{
	/// <summary>
	/// Core application data repository interface.
	/// </summary>
	public interface IAppDataRepository
	{
		/// <summary>
		/// Gets current error.
		/// </summary>
		string? Error { get; }

		/// <summary>
		/// Add new scan folder record.
		/// </summary>
		/// <param name="path">New scan folder path.</param>
		/// <returns>async Task.</returns>
		Task AddScanFolderAsync(string path);

		/// <summary>
		/// Remove scan folder record by path.
		/// </summary>
		/// <param name="path">Scan folder path.</param>
		/// <returns>async Task.</returns>
		Task RemoveScanFolderAsync(string path);

		/// <summary>
		/// Get all scan folders.
		/// </summary>
		/// <returns>Scan folder records.</returns>
		Task<List<ScanFolder>> GetAllScanFoldersAsync();
	}
}
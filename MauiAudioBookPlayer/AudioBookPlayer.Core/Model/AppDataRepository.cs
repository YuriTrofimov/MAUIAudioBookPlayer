// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioBookPlayer.Core.Model.Entities;
using SQLite;

namespace AudioBookPlayer.Core.Model
{
	/// <summary>
	/// Core data repository.
	/// </summary>
	public class AppDataRepository
	{
		private readonly string dbFilePath;
		private SQLiteAsyncConnection? connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="AppDataRepository"/> class.
		/// </summary>
		/// <param name="dbPath">SqLite database file path.</param>
		public AppDataRepository(string dbPath)
		{
			dbFilePath = dbPath;
		}

		/// <summary>
		/// Gets current error text.
		/// </summary>
		public string? Error { get; private set; }

		/// <summary>
		/// Add new scan folder record.
		/// </summary>
		/// <param name="path">New scan folder path.</param>
		/// <returns>async Task.</returns>
		public async Task AddScanFolderAsync(string path)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(path))
				{
					Error = "Incorrect scan folder";
					return;
				}

				await InitAsync();
				if (connection != null)
				{
					await connection.InsertOrReplaceAsync(new ScanFolder { Path = path });
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}
		}

		/// <summary>
		/// Remove scan folder record by path.
		/// </summary>
		/// <param name="path">Scan folder path.</param>
		/// <returns>async Task.</returns>
		public async Task RemoveScanFolderAsync(string path)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(path))
				{
					Error = "Incorrect scan folder";
					return;
				}

				await InitAsync();
				if (connection != null)
				{
					await connection.Table<ScanFolder>().DeleteAsync(x => x.Path == path);
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}
		}

		/// <summary>
		/// Get all scan folders.
		/// </summary>
		/// <returns>Scan folder records.</returns>
		public async Task<List<ScanFolder>> GetAllScanFoldersAsync()
		{
			try
			{
				await InitAsync();
				if (connection != null)
				{
					return await connection.Table<ScanFolder>().ToListAsync();
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}

			return new List<ScanFolder>();
		}

		private async Task InitAsync()
		{
			if (connection != null)
			{
				return;
			}

			connection = new SQLiteAsyncConnection(dbFilePath);
			await connection.CreateTableAsync<ScanFolder>();
		}
	}
}
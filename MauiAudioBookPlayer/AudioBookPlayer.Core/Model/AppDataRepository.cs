// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AudioBookPlayer.Core.Model.Entities;
using SQLite;

namespace AudioBookPlayer.Core.Model
{
	/// <summary>
	/// Core data repository.
	/// </summary>
	public class AppDataRepository : IAppDataRepository
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

		/// <summary>
		/// Add new book to database.
		/// If book with current folder path exists - it will be replaced.
		/// </summary>
		/// <param name="book">New book.</param>
		/// <returns>async Task.</returns>
		public async Task AddBookAsync(Book book)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(book.Caption))
				{
					Error = "Incorrect book caption";
					return;
				}

				if (string.IsNullOrWhiteSpace(book.FolderPath))
				{
					Error = "Incorrect book FolderPath";
					return;
				}

				await InitAsync();
				if (connection != null)
				{
					await connection.Table<Book>()
						.DeleteAsync(a => a.FolderPath == book.FolderPath);

					await connection.InsertOrReplaceAsync(book);

					if (!book.ID.HasValue)
					{
						return;
					}

					await connection.Table<BookFile>().DeleteAsync(a => a.BookID == book.ID.Value);

					foreach (var file in book.Files)
					{
						file.BookID = book.ID.Value;
						await connection.InsertOrReplaceAsync(file);
					}
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}
		}

		/// <summary>
		/// Remove book record.
		/// </summary>
		/// <param name="book">Book to delete.</param>
		/// <returns>async Task.</returns>
		public async Task RemoveBookAsync(Book book)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(book?.FolderPath))
				{
					Error = "Incorrect book FolderPath";
					return;
				}

				await InitAsync();
				if (connection != null)
				{
					await connection.Table<Book>().DeleteAsync(x => x.ID == book.ID);
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}
		}

		/// <summary>
		/// Get all books.
		/// </summary>
		/// <returns>Books records.</returns>
		public async Task<List<Book>> GetAllBooksAsync()
		{
			var sw = new Stopwatch();
			try
			{
				await InitAsync();
				if (connection != null)
				{
					sw.Start();
					return await connection.Table<Book>().ToListAsync();
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}
			finally
			{
				sw.Stop();
				Trace.TraceInformation($"Book list loaded in:{sw.ElapsedMilliseconds} ms");
			}

			return new List<Book>();
		}

		/// <summary>
		/// Delete all books.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task DeleteAllBooksAsync()
		{
			try
			{
				await InitAsync();
				if (connection != null)
				{
					await connection.DeleteAllAsync<Book>();
					await connection.DeleteAllAsync<BookFile>();
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}
		}

		/// <summary>
		/// Get all book files.
		/// </summary>
		/// <param name="book">Book.</param>
		/// <returns>Book files records.</returns>
		public async Task<List<BookFile>> GetAllBookFilesAsync(Book book)
		{
			var sw = new Stopwatch();
			try
			{
				if (book == null)
				{
					return new List<BookFile>();
				}

				await InitAsync();
				if (connection != null)
				{
					sw.Start();
					return await connection.Table<BookFile>()
						.Where(a => a.BookID == book.ID).
						ToListAsync();
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}
			finally
			{
				sw.Stop();
				Trace.TraceInformation($"Book with ID:{book.ID} files list loaded in:{sw.ElapsedMilliseconds} ms");
			}

			return new List<BookFile>();
		}

		/// <summary>
		/// Clear database.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task ClearDB()
		{
			try
			{
				await InitAsync();
				if (connection != null)
				{
					await connection.DeleteAllAsync<ScanFolder>();
					await connection.DeleteAllAsync<Book>();
					await connection.DeleteAllAsync<BookFile>();
				}
			}
			catch (Exception ex)
			{
				Error = ex.Message;
			}
		}

		private async Task InitAsync()
		{
			if (connection != null)
			{
				return;
			}

			connection = new SQLiteAsyncConnection(dbFilePath);
			await connection.ExecuteAsync("PRAGMA foreignkeys = ON");
			await connection.CreateTableAsync<ScanFolder>();
			await connection.CreateTableAsync<Book>();
			await connection.CreateTableAsync<BookFile>();
		}
	}
}
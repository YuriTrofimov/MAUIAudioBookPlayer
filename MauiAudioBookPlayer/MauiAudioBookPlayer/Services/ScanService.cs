// Copyright (c) 2022 Yuri Trofimov. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBookPlayer.Core.Model;
using AudioBookPlayer.Core.Model.Entities;
using AudioBookPlayer.Core.Services.BookScanService;

namespace MauiAudioBookPlayer.Services
{
	/// <summary>
	/// Audiobook scan service.
	/// </summary>
	public class ScanService : IScanService
	{
		private readonly IAppDataRepository repository;
		private readonly IBookScanService bookScanService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScanService"/> class.
		/// </summary>
		/// <param name="repository">App data repository.</param>
		/// <param name="bookScanService">Book scanning service.</param>
		public ScanService(IAppDataRepository repository, IBookScanService bookScanService)
		{
			this.repository = repository;
			this.bookScanService = bookScanService;
		}

		/// <summary>
		/// Search for audio books on device and update database.
		/// Delete all existing book records.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task<List<Book>> ScanAsync()
		{
			var scanFolders = await repository.GetAllScanFoldersAsync();
			await repository.DeleteAllBooksAsync();
			List<Book> books = new List<Book>();
			await Task.Run(() => books = bookScanService.SearchBooks(scanFolders));
			foreach (var book in books)
			{
				await repository.AddBookAsync(book);
			}

			return books;
		}
	}
}

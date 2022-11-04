// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using AudioBookPlayer.Core.Model;
using AudioBookPlayer.Core.Model.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace MauiAudioBookPlayer.ViewModel
{
	/// <summary>
	/// Book library view model.
	/// </summary>
	public partial class BookListViewModel : ObservableObject
	{
		private readonly IAppDataRepository repository;
		private bool initialized;

		[ObservableProperty]
		private ObservableCollection<Book> books;

		[ObservableProperty]
		private bool loading;

		[ObservableProperty]
		private Book selectedBook;

		/// <summary>
		/// Initializes a new instance of the <see cref="BookListViewModel"/> class.
		/// </summary>
		public BookListViewModel()
		{
			repository = Ioc.Default.GetService<IAppDataRepository>();
			books = new ObservableCollection<Book>();
		}

		/// <summary>
		/// Initialize view model.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task InitializeAsync()
		{
			if (initialized)
			{
				return;
			}

			await ReloadLibraryAsync();
			initialized = true;
		}

		/// <summary>
		/// Reload library.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task ReloadLibraryAsync()
		{
			books.Clear();
			var booksList = await repository.GetAllBooksAsync();
			booksList.ForEach(b => books.Add(b));
		}
	}
}
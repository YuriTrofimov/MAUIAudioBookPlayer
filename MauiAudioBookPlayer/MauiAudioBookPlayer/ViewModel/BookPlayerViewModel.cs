// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using AudioBookPlayer.Core.Model;
using AudioBookPlayer.Core.Model.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MauiAudio;

namespace MauiAudioBookPlayer.ViewModel
{
	/// <summary>
	/// Book player view model.
	/// </summary>
	public partial class BookPlayerViewModel : ObservableObject
	{
		private readonly IAppDataRepository repository;
		private readonly INativeAudioService audioService;
		private bool initialized;

		[ObservableProperty]
		private Book book;

		[ObservableProperty]
		private ObservableCollection<BookFile> files;

		/// <summary>
		/// Initializes a new instance of the <see cref="BookPlayerViewModel"/> class.
		/// </summary>
		public BookPlayerViewModel()
		{
			repository = Ioc.Default.GetService<IAppDataRepository>();
			audioService = Ioc.Default.GetService<INativeAudioService>();
			files = new ObservableCollection<BookFile>();
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

			await ReloadFilesAsync();
			initialized = true;
		}

		[RelayCommand]
		private async Task PlayBook(BookFile file)
		{
			if (file == null)
			{
				return;
			}

			await audioService.InitializeAsync(file.FilePath);
			await audioService.PlayAsync();
		}

		private async Task ReloadFilesAsync()
		{
			files.Clear();
			var filesList = await repository.GetAllBookFilesAsync(Book);
			filesList.ForEach(f => files.Add(f));
		}
	}
}
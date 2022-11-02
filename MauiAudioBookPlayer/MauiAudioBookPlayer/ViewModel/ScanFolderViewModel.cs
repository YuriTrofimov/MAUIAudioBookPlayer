// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using AudioBookPlayer.Core.Model;
using AudioBookPlayer.Core.Model.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace MauiAudioBookPlayer.ViewModel
{
	/// <summary>
	/// Manage scan folders ViewModel.
	/// </summary>
	public partial class ScanFolderViewModel : ObservableObject
	{
		private readonly IAppDataRepository repository;
		private bool initialized;

		[ObservableProperty]
		private ObservableCollection<ScanFolder> folders;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScanFolderViewModel"/> class.
		/// </summary>
		public ScanFolderViewModel()
		{
			repository = Ioc.Default.GetService<IAppDataRepository>();
			folders = new ObservableCollection<ScanFolder>();
			initialized = false;
		}

		/// <summary>
		/// ViewModel initialization. Can be called only once.
		/// </summary>
		/// <returns>async Task.</returns>
		public async Task Initialize()
		{
			if (initialized)
			{
				return;
			}

			await ReloadFoldersAsync();
			initialized = true;
		}

		private async Task ReloadFoldersAsync()
		{
			folders.Clear();
			var scanFolders = await repository.GetAllScanFoldersAsync();
			scanFolders.ForEach(f => folders.Add(f));
		}

		[RelayCommand]
		private async Task Add(string path)
		{
			await AddNewScanFolder(path);
		}

		private async Task AddNewScanFolder(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}

			var allFolders = await repository.GetAllScanFoldersAsync();
			if (allFolders.Any(a => string.Equals(path, a.Path, StringComparison.InvariantCultureIgnoreCase)) == true)
			{
				return;
			}

			await repository.AddScanFolderAsync(path);
			await ReloadFoldersAsync();
		}

		[RelayCommand]
		private async Task Delete(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			await repository.RemoveScanFolderAsync(path);
			await ReloadFoldersAsync();
		}
	}
}
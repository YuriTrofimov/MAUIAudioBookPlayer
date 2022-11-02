// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using AudioBookPlayer.Core.Services.ExplorerService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace MauiAudioBookPlayer.ViewModel
{
	/// <summary>
	/// File system explorer ViewModel.
	/// </summary>
	public partial class ExplorerViewModel : ObservableObject
	{
		private readonly IExplorerService explorer;

		[ObservableProperty]
		private string currentPath;

		[ObservableProperty]
		private ObservableCollection<FolderItem> folders;

		[ObservableProperty]
		private ObservableCollection<FileItem> files;

		[ObservableProperty]
		private bool canReturn;

		[ObservableProperty]
		private string previousPath;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExplorerViewModel"/> class.
		/// </summary>
		public ExplorerViewModel()
		{
			explorer = Ioc.Default.GetService<IExplorerService>();
			folders = new ObservableCollection<FolderItem>();
			files = new ObservableCollection<FileItem>();
		}

		/// <summary>
		/// Event invoked when CurrentPath is confirmed.
		/// </summary>
		public event EventHandler OnPathConfirmed;

		/// <summary>
		/// ViewModel initialization.
		/// </summary>
		public void Initialize()
		{
			// Select root folder
			SelectFolder(null);
		}

		[RelayCommand]
		private void SelectFolder(string path)
		{
			if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
			{
				CurrentPath = null;
				PreviousPath = null;
			}
			else
			{
				CurrentPath = path;
				DirectoryInfo previousPath = Directory.GetParent(CurrentPath);
				PreviousPath = previousPath?.FullName;
			}

			CanReturn = !string.IsNullOrEmpty(path);

			var rootFolders = explorer.LoadDirectoryItems(path);
			FillLists(rootFolders);
		}

		private void FillLists(IEnumerable<ExplorerItem> children)
		{
			Folders.Clear();
			Files.Clear();
			foreach (var item in children)
			{
				if (item is FolderItem)
				{
					Folders.Add((FolderItem)item);
				}
				else if (item is FileItem)
				{
					Files.Add((FileItem)item);
				}
			}
		}

		[RelayCommand]
		private void ConfirmPath()
		{
			OnPathConfirmed?.Invoke(this, new EventArgs());
		}
	}
}
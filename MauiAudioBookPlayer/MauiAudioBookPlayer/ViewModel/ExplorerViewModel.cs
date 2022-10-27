// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

		/// <summary>
		/// Initializes a new instance of the <see cref="ExplorerViewModel"/> class.
		/// </summary>
		public ExplorerViewModel()
		{
			explorer = Ioc.Default.GetService<IExplorerService>();
		}

		/// <summary>
		/// Event invoked when CurrentPath is confirmed.
		/// </summary>
		public event EventHandler OnPathConfirmed;

		[RelayCommand]
		private void SelectFolder(string path)
		{
			CurrentPath = path;
		}

		[RelayCommand]
		private void ConfirmPath()
		{
			OnPathConfirmed?.Invoke(this, new EventArgs());
		}
	}
}
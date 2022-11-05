// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommunityToolkit.Mvvm.DependencyInjection;
using MauiAudioBookPlayer.Extensions;
using MauiAudioBookPlayer.ViewModel;

namespace MauiAudioBookPlayer.Views;

/// <summary>
/// Manage scan folders page.
/// </summary>
public partial class ManageScanFolders : ContentPage, IQueryAttributable
{
	private readonly ScanFolderViewModel viewModel;

	/// <summary>
	/// Initializes a new instance of the <see cref="ManageScanFolders"/> class.
	/// </summary>
	public ManageScanFolders()
	{
		viewModel = Ioc.Default.GetService<ScanFolderViewModel>();
		BindingContext = viewModel;
		InitializeComponent();
		InitializeViewModel();
	}

	/// <summary>
	/// Async viewmodel initialization.
	/// </summary>
	public async void InitializeViewModel()
	{
		if (viewModel == null)
		{
			return;
		}

		await viewModel.Initialize();
	}

	/// <summary>
	/// IQueryAttributable interface method.
	/// </summary>
	/// <param name="query">Query parameters dictionary.</param>
	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		var navParams = query.AsNavParams();
		if (!string.IsNullOrEmpty(navParams.SelectedFolder))
		{
			viewModel.AddCommand.Execute(navParams.SelectedFolder);
		}
	}
}
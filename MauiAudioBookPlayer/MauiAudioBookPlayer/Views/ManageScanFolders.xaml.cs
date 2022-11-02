// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using MauiAudioBookPlayer.ViewModel;
using MauiAudioBookPlayer.Views;

namespace MauiAudioBookPlayer.Pages;

/// <summary>
/// Manage scan folders page.
/// </summary>
public partial class ManageScanFolders : ContentPage
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

	private async void SelectFolder_Clicked(object sender, EventArgs e)
	{
		var result = await this.ShowPopupAsync(new SelectFolderPage()) as string;
		await viewModel.AddCommand.ExecuteAsync(result);
	}
}
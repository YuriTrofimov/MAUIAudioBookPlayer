// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommunityToolkit.Mvvm.DependencyInjection;
using MauiAudioBookPlayer.Model;
using MauiAudioBookPlayer.ViewModel;

namespace MauiAudioBookPlayer.Views;

/// <summary>
/// Select folder popup content.
/// </summary>
public partial class SelectFolderPage : ContentPage
{
	private readonly ExplorerViewModel viewModel;

	/// <summary>
	/// Initializes a new instance of the <see cref="SelectFolderPage"/> class.
	/// </summary>
	public SelectFolderPage()
	{
		viewModel = Ioc.Default.GetService<ExplorerViewModel>();
		BindingContext = viewModel;
		InitializeComponent();
		viewModel.Initialize();
	}

	private async void CloseButton_Clicked(object sender, EventArgs e)
	{
		var navParams = new NavParams
		{
			SelectedFolder = viewModel.CurrentPath,
		};
		await Shell.Current.GoToAsync($"..", true, navParams);
	}
}
// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommunityToolkit.Mvvm.DependencyInjection;
using MauiAudioBookPlayer.ViewModel;

namespace MauiAudioBookPlayer.Views;

/// <summary>
/// Library page.
/// </summary>
public partial class LibraryPage : ContentPage
{
	private readonly BookListViewModel viewModel;

	/// <summary>
	/// Initializes a new instance of the <see cref="LibraryPage"/> class.
	/// </summary>
	public LibraryPage()
	{
		InitializeComponent();
		viewModel = Ioc.Default.GetService<BookListViewModel>();
		BindingContext = viewModel;
		InitViewModel();
	}

	/// <summary>
	/// OnPage appearing.
	/// </summary>
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.ReloadLibraryAsync();
	}

	private async void InitViewModel()
	{
		if (viewModel == null)
		{
			return;
		}

		await viewModel.InitializeAsync();
	}
}
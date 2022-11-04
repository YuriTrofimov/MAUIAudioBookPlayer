// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MauiAudioBookPlayer.Extensions;
using MauiAudioBookPlayer.ViewModel;

namespace MauiAudioBookPlayer.Views;

/// <summary>
/// Book player view.
/// </summary>
public partial class BookPlayerPage : ContentPage, IQueryAttributable
{
	private readonly BookPlayerViewModel viewModel;

	/// <summary>
	/// Initializes a new instance of the <see cref="BookPlayerPage"/> class.
	/// </summary>
	public BookPlayerPage()
	{
		InitializeComponent();
		viewModel = (BookPlayerViewModel)BindingContext;
	}

	/// <summary>
	/// IQueryAttributable interface method.
	/// </summary>
	/// <param name="query">Query parameters dictionary.</param>
	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		viewModel.Book = query.AsNavParams().SelectedBook;
		await InitViewModel();
	}

	/// <summary>
	/// On page dissapear.
	/// </summary>
	protected override async void OnDisappearing()
	{
		base.OnDisappearing();
		await viewModel.StopCommand.ExecuteAsync(null);
		await viewModel.SaveProgress();
	}

	private async Task InitViewModel()
	{
		await viewModel.InitializeAsync();
	}
}
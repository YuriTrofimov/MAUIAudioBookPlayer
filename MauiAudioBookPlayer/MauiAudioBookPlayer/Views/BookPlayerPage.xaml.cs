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
	/// <summary>
	/// Initializes a new instance of the <see cref="BookPlayerPage"/> class.
	/// </summary>
	public BookPlayerPage()
	{
		InitializeComponent();
	}

	/// <summary>
	/// IQueryAttributable interface method.
	/// </summary>
	/// <param name="query">Query parameters dictionary.</param>
	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		var vm = BindingContext as BookPlayerViewModel;
		if (vm == null)
		{
			return;
		}

		vm.Book = query.AsNavParams().SelectedBook;
		await InitViewModel();
	}

	private async Task InitViewModel()
	{
		var vm = BindingContext as BookPlayerViewModel;
		if (vm == null)
		{
			return;
		}

		await vm.InitializeAsync();
	}
}
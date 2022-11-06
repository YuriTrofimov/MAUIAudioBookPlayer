// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommunityToolkit.Mvvm.DependencyInjection;
using MauiAudioBookPlayer.ViewModel;

namespace MauiAudioBookPlayer.Views;

/// <summary>
/// Settings page.
/// </summary>
public partial class SettingsPage : ContentPage
{
	private readonly SettingsViewModel viewModel;

	/// <summary>
	/// Initializes a new instance of the <see cref="SettingsPage"/> class.
	/// </summary>
	public SettingsPage()
	{
		InitializeComponent();
		viewModel = Ioc.Default.GetService<SettingsViewModel>();
		BindingContext = viewModel;
	}
}
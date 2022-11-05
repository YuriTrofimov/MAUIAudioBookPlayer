// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AudioBookPlayer.Core.Model;
using AudioBookPlayer.Core.Services.BookScanService;
using AudioBookPlayer.Core.Services.ExplorerService;
using CommunityToolkit.Maui;
using MauiAudio;
using MauiAudioBookPlayer.Extensions;
using MauiAudioBookPlayer.Services;
using MauiAudioBookPlayer.ViewModel;

namespace MauiAudioBookPlayer;

/// <summary>
/// Maui Application.
/// </summary>
public static class MauiProgram
{
	/// <summary>
	/// Application initialization.
	/// </summary>
	/// <returns>MauiApp.</returns>
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiAudio()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterServices();

		return builder.Build();
	}

	/// <summary>
	/// DependencyInjection setup.
	/// </summary>
	/// <param name="builder">AppBuilder.</param>
	/// <returns>AppBuilder Fluent API.</returns>
	public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
	{
		var dbPath = "data.db3".ToLocalFilePath();
		builder.Services.AddSingleton<IAppDataRepository>(new AppDataRepository(dbPath));

#if WINDOWS
		builder.Services.AddSingleton<IExplorerService, MauiAudioBookPlayer.Platforms.Windows.WindowsExplorerService>();
#elif ANDROID
		builder.Services.AddSingleton<IExplorerService, MauiAudioBookPlayer.Platforms.Android.AndroidExplorerService>();
#endif

		builder.Services.AddSingleton<IBookScanService, BookScanService>();
		builder.Services.AddSingleton<IScanService, ScanService>();
		builder.Services.AddSingleton<BookPlayerViewModel>();
		builder.Services.AddSingleton<IMessageBoxService, MessageBoxService>();
		builder.Services.AddSingleton<INavigationService, NavigationService>();

		builder.Services.AddTransient<ScanFolderViewModel>();
		builder.Services.AddTransient<ExplorerViewModel>();
		builder.Services.AddTransient<BookListViewModel>();

		builder.Services.AddSingleton<IMauiInitializeService>(new IocConfigurationService());
		return builder;
	}
}
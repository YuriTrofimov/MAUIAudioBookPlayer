﻿// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AudioBookPlayer.Core.Model;
using AudioBookPlayer.Core.Services.BookScanService;
using AudioBookPlayer.Core.Services.ExplorerService;
using CommunityToolkit.Mvvm.DependencyInjection;
using MauiAudioBookPlayer.Extensions;
using MauiAudioBookPlayer.Services;
using MauiAudioBookPlayer.ViewModel;

namespace MauiAudioBookPlayer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
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
		builder.Services.AddSingleton<IExplorerService, ExplorerService>();
		builder.Services.AddSingleton<IBookScanService, BookScanService>();
		builder.Services.AddTransient<ScanFolderViewModel>();

		builder.Services.AddSingleton<IMauiInitializeService>(new IocConfigurationService());
		return builder;
	}
}
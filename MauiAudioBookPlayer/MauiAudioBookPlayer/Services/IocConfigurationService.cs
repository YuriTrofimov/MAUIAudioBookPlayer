// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommunityToolkit.Mvvm.DependencyInjection;

namespace MauiAudioBookPlayer.Services
{
	/// <summary>
	/// CommunityToolkit IoC container wrapper service.
	/// </summary>
	public class IocConfigurationService : IMauiInitializeService
	{
		/// <summary>
		/// Initialize services.
		/// </summary>
		/// <param name="services">Services.</param>
		public void Initialize(IServiceProvider services)
		{
			Ioc.Default.ConfigureServices(services);
		}
	}
}
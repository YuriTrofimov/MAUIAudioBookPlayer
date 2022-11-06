// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using MauiAudioBookPlayer.Services;

namespace MauiAudioBookPlayer.ViewModel
{
	/// <summary>
	/// App settings viewmodel.
	/// </summary>
	public partial class SettingsViewModel : ObservableObject
	{
		private readonly ISettingsService settingsService;

		/// <summary>
		/// Sleep timer period in minutes.
		/// </summary>
		[ObservableProperty]
		private int sleepTimerPeriod;

		/// <summary>
		/// True if sleep timer enabled.
		/// </summary>
		[ObservableProperty]
		private bool sleepTimerEnabled;

		/// <summary>
		/// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
		/// </summary>
		public SettingsViewModel()
		{
			settingsService = Ioc.Default.GetService<ISettingsService>();
			SleepTimerEnabled = settingsService.SleepModeEnabled;
			SleepTimerPeriod = settingsService.SleepModeTimerPeriod;
		}

		partial void OnSleepTimerEnabledChanged(bool value)
		{
			settingsService.SleepModeEnabled = value;
		}

		partial void OnSleepTimerPeriodChanged(int value)
		{
			settingsService.SleepModeTimerPeriod = value;
		}
	}
}
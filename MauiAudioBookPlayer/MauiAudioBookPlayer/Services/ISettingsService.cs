// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MauiAudioBookPlayer.Services
{
	/// <summary>
	/// App settings service interface.
	/// </summary>
	public interface ISettingsService
	{
		/// <summary>
		/// Gets or sets a value indicating whether sleep mode enabled.
		/// </summary>
		bool SleepModeEnabled { get; set; }

		/// <summary>
		/// Gets or sets sleep mode timer period, minutes.
		/// </summary>
		int SleepModeTimerPeriod { get; set; }
	}
}
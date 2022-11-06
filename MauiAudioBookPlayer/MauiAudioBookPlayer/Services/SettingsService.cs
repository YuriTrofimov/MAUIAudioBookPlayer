// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace MauiAudioBookPlayer.Services
{
	/// <summary>
	/// App setting service.
	/// </summary>
	public class SettingsService : ISettingsService
	{
		/// <inheritdoc/>
		public bool SleepModeEnabled
		{
			get => InnerGet<bool>();
			set => InnerSet(value);
		}

		/// <inheritdoc/>
		public int SleepModeTimerPeriod
		{
			get => InnerGet<int>();
			set => InnerSet(value);
		}

		private T InnerGet<T>([CallerMemberName] string name = null)
			where T : struct
		{
			return Preferences.Default.Get(name, default(T));
		}

		private void InnerSet<T>(T val, [CallerMemberName] string name = null)
			where T : struct
		{
			Preferences.Default.Set<T>(name, val);
		}
	}
}
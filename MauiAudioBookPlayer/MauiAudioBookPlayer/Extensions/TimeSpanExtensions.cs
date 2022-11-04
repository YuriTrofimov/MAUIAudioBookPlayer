// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Maui.Platform;

namespace MauiAudioBookPlayer.Extensions
{
	/// <summary>
	/// TimeSpan extensions.
	/// </summary>
	public static class TimeSpanExtensions
	{
		/// <summary>
		/// Create readable string from timespan.
		/// </summary>
		/// <param name="timeSpan">Timespan.</param>
		/// <returns>Display string.</returns>
		public static string ToFormattedTime(this TimeSpan timeSpan)
		{
			return timeSpan.ToFormattedString(timeSpan.Hours > 0 ? "hh:mm:ss" : "mm:ss");
		}
	}
}
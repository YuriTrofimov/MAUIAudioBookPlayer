// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MauiAudioBookPlayer.Model
{
	/// <summary>
	/// Audio player state.
	/// </summary>
	public enum EPlayerState
	{
		/// <summary>
		/// Player stopped.
		/// </summary>
		Stop,

		/// <summary>
		/// Player on pause.
		/// </summary>
		Pause,

		/// <summary>
		/// Playback.
		/// </summary>
		Play,
	}
}
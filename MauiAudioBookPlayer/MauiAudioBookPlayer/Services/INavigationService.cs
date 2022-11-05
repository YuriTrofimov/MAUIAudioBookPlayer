// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AudioBookPlayer.Core.Model.Entities;

namespace MauiAudioBookPlayer.Services
{
	/// <summary>
	/// InApp navigation service interface.
	/// </summary>
	public interface INavigationService
	{
		/// <summary>
		/// Navigate to book player page.
		/// </summary>
		/// <param name="bookToPlay">Book to play.</param>
		/// <returns>Async task.</returns>
		Task GoToBookPlayerAsync(Book bookToPlay);

		/// <summary>
		/// Navigate to select scan folder page.
		/// </summary>
		/// <returns>Async task.</returns>
		Task GoToFolderExplorer();
	}
}
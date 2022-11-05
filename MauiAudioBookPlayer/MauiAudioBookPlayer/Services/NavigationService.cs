// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AudioBookPlayer.Core.Model.Entities;
using MauiAudioBookPlayer.Model;

namespace MauiAudioBookPlayer.Services
{
	/// <summary>
	/// InApp navigation service.
	/// </summary>
	public class NavigationService : INavigationService
	{
		/// <summary>
		/// Navigate to book player page.
		/// </summary>
		/// <param name="bookToPlay">Book to play.</param>
		/// <returns>Async task.</returns>
		public async Task GoToBookPlayerAsync(Book bookToPlay)
		{
			var navParams = new NavParams
			{
				SelectedBook = bookToPlay,
			};

			await Shell.Current.GoToAsync("///library/player", true, navParams);
		}

		/// <summary>
		/// Navigate to select scan folder page.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task GoToFolderExplorer()
		{
			await Shell.Current.GoToAsync("///scan/explorer", true);
		}
	}
}
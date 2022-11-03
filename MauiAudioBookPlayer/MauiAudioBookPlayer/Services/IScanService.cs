// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AudioBookPlayer.Core.Model.Entities;

namespace MauiAudioBookPlayer.Services
{
	/// <summary>
	/// Book scan service interface.
	/// </summary>
	public interface IScanService
	{
		/// <summary>
		/// Search for audio books on device and update database.
		/// Delete all existing book records.
		/// </summary>
		/// <returns>Async task.</returns>
		Task<List<Book>> ScanAsync();
	}
}
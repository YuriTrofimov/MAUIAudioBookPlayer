// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using AudioBookPlayer.Core.Model.Entities;

namespace AudioBookPlayer.Core.Services.BookScanService
{
	/// <summary>
	/// BookScanService interface.
	/// </summary>
	public interface IBookScanService
	{
		/// <summary>
		/// Search books in provided scan folders.
		/// </summary>
		/// <param name="folders">Scan folders.</param>
		/// <returns>List of books finded.</returns>
		List<Book> SearchBooks(List<ScanFolder> folders);
	}
}
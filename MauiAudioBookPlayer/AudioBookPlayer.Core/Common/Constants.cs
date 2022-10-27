// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AudioBookPlayer.Core.Common
{
	/// <summary>
	/// Global application constants.
	/// </summary>
	public sealed class Constants
	{
		/// <summary>
		/// Supported audio file extensions.
		/// </summary>
		public static readonly string[] AudioFiles = new[] { ".mp3" };

		/// <summary>
		/// Supported image file extensions.
		/// </summary>
		public static readonly string[] ImageFiles = new[] { ".jpg", ".jpeg", ".png" };
	}
}
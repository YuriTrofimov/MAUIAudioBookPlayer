// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MauiAudioBookPlayer.Extensions
{
	/// <summary>
	/// String type extensions.
	/// </summary>
	internal static class StringExtensions
	{
		/// <summary>
		/// Convert relative to absolute local file path.
		/// </summary>
		/// <param name="path">Relative file path.</param>
		/// <returns>Absolute local file path.</returns>
		public static string ToLocalFilePath(this string path)
		{
			return Path.Combine(FileSystem.AppDataDirectory, path);
		}
	}
}
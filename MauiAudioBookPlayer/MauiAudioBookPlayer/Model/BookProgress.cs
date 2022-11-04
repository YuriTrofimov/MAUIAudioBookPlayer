// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using Newtonsoft.Json;

namespace MauiAudioBookPlayer.Model
{
	/// <summary>
	/// Audio book progress data.
	/// </summary>
	public record BookProgress
	{
		private const string ProgressFileName = "progress.dat";

		/// <summary>
		/// Gets or sets last played file path.
		/// </summary>
		public string FilePath { get; set; }

		/// <summary>
		/// Gets or sets last played file position.
		/// </summary>
		public double Position { get; set; }

		/// <summary>
		/// Load book progress from file in book folder.
		/// </summary>
		/// <param name="path">Book path.</param>
		/// <returns>Null if progress file missing or invalid.</returns>
		public static async Task<BookProgress> LoadAsync(string path)
		{
			try
			{
				var filePath = Path.Combine(path, ProgressFileName);
				if (!File.Exists(filePath))
				{
					return null;
				}

				var json = await File.ReadAllTextAsync(filePath);
				return JsonConvert.DeserializeObject<BookProgress>(json);
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// Save book progress to file in book folder.
		/// </summary>
		/// <param name="path">Book folder path.</param>
		/// <param name="currentFilePath">Current played file path.</param>
		/// <param name="progress">File play progress.</param>
		/// <returns>Async task.</returns>
		public static async Task SaveAsync(string path, string currentFilePath, double progress)
		{
			try
			{
				var filePath = Path.Combine(path, ProgressFileName);
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}

				var json = JsonConvert.SerializeObject(new BookProgress
				{
					FilePath = currentFilePath,
					Position = progress,
				});
				await File.WriteAllTextAsync(filePath, json);
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
		}
	}
}
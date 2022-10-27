// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioBookPlayer.Core.Common;
using AudioBookPlayer.Core.Model.Entities;

namespace AudioBookPlayer.Core.Services.BookScanService
{
	/// <summary>
	/// Service for search book files.
	/// </summary>
	/// <remarks>
	/// Folder structure templates:
	/// (1) /ScanFolder/../BookName(non numeric)/file.mp3
	/// (2) /ScanFolder/../BookName(non numeric)/01(numeric)/file.mp3
	/// (3) /ScanFolder/file.mp3 -- Single file book.
	/// </remarks>
	public class BookScanService
	{
		/// <summary>
		/// Search books in provided scan folders.
		/// </summary>
		/// <param name="folders">Scan folders.</param>
		/// <returns>List of books finded.</returns>
		public List<Book> SearchBooks(List<ScanFolder> folders)
		{
			var result = new List<Book>();
			var processedFolders = new List<string>();

			foreach (var path in folders.Select(f => f.Path))
			{
				var folder = new FolderInfo(path);
				result.AddRange(ParseTemplate1(folder.Children, ref processedFolders));
				result.AddRange(ParseTemplate2(folder.Children, ref processedFolders));
				result.AddRange(ParseTemplate3(new List<FolderInfo>() { folder }, ref processedFolders));
			}

			return result;
		}

		/// <summary>
		/// Parse template1 folder structure:
		/// "/ScanFolder/../BookName(non numeric)/file.mp3".
		/// </summary>
		/// <param name="folders">FolderData to process.</param>
		/// <param name="processedFolders">Already processed folders.</param>
		/// <returns>Books finded.</returns>
		private List<Book> ParseTemplate1(List<FolderInfo> folders, ref List<string> processedFolders)
		{
			var result = new List<Book>();
			foreach (var folder in folders)
			{
				if (processedFolders.Contains(folder.Path))
				{
					continue;
				}

				var leaves = FolderInfo.GetLeaves(folder);
				foreach (var leave in leaves)
				{
					if (processedFolders.Contains(leave.Path))
					{
						continue;
					}

					if (leave.IsNumeric)
					{
						continue; // Ignore numeric folder names
					}

					var book = new Book
					{
						Caption = leave.Name,
						FolderPath = leave.Path,
						CoverImagePath = leave.ImageFiles.FirstOrDefault(),
					};
					foreach (var file in leave.AudioFiles.OrderBy(a => a))
					{
						book.AddBookFile(file);
					}

					if (book.Files.Count == 1)
					{
						book.Caption = book.Files[0].Name;
					}

					result.Add(book);
					processedFolders.Add(leave.Path);
				}
			}

			return result;
		}

		/// <summary>
		/// Parse template2 folder structure:
		/// "/ScanFolder/../BookName(non numeric)/01(numeric)/file.mp3"
		/// "TODO /ScanFolder/../BookName(non numeric and not audio)/Chapter 01(non numeric)/file.mp3".
		/// </summary>
		/// <param name="folders">FolderData to process.</param>
		/// <param name="processedFolders">Already processed folders.</param>
		/// <returns>Books finded.</returns>
		private List<Book> ParseTemplate2(List<FolderInfo> folders, ref List<string> processedFolders)
		{
			var result = new List<Book>();
			foreach (var folder in folders)
			{
				if (processedFolders.Contains(folder.Path))
				{
					continue;
				}

				var leaves = FolderInfo.GetLeaves(folder);
				if (leaves.Count == 0)
				{
					continue;
				}

				var book = new Book
				{
					Caption = folder.Name,
					FolderPath = folder.Path,
					CoverImagePath = folder.ImageFiles.FirstOrDefault(),
				};
				processedFolders.Add(folder.Path);

				foreach (var leave in leaves.OrderBy(a => a.Name))
				{
					if (processedFolders.Contains(leave.Path))
					{
						continue;
					}

					if (!leave.IsNumeric)
					{
						continue; // Ignore non numeric folder names
					}

					foreach (var file in leave.AudioFiles.OrderBy(a => a))
					{
						book.AddBookFile(file);
					}

					processedFolders.Add(leave.Path);
				}

				if (book.Files.Count > 0)
				{
					if (book.Files.Count == 1)
					{
						book.Caption = book.Files[0].Name;
					}

					result.Add(book);
				}
			}

			return result;
		}

		/// <summary>
		/// Parse single file audio books.
		/// </summary>
		/// <param name="folders">FolderData to process.</param>
		/// <param name="processedFolders">Already processed folders.</param>
		/// <returns>Books finded.</returns>
		private List<Book> ParseTemplate3(List<FolderInfo> folders, ref List<string> processedFolders)
		{
			var result = new List<Book>();
			foreach (var folder in folders)
			{
				if (processedFolders.Contains(folder.Path))
				{
					continue;
				}

				if (!folder.IsNumeric)
				{
					foreach (var file in folder.AudioFiles)
					{
						var book = new Book
						{
							Caption = Path.GetFileNameWithoutExtension(file),
							FolderPath = folder.Path,
							CoverImagePath = TryGetCoverImageFromAudio(file),
						};
						book.AddBookFile(file);
						result.Add(book);
					}

					processedFolders.Add(folder.Path);
				}

				if (folder.HasChildren)
				{
					result.AddRange(ParseTemplate3(folder.Children, ref processedFolders));
				}
			}

			return result;
		}

		/// <summary>
		/// Search and return cover image for single file book audio file.
		/// </summary>
		/// <param name="path">Audio file path.</param>
		/// <returns>Path to cover image. Null if not exists.</returns>
		private string? TryGetCoverImageFromAudio(string path)
		{
			var folder = Path.GetDirectoryName(path);
			foreach (var extension in Constants.ImageFiles)
			{
				var filePath = Path.Combine(folder, $"fileName{extension}");
				if (File.Exists(filePath))
				{
					return filePath;
				}
			}

			return null;
		}
	}
}
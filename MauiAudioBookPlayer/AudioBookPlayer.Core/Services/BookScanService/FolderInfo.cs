// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AudioBookPlayer.Core.Services.BookScanService
{
	/// <summary>
	/// Folder data for further parsing.
	/// </summary>
	internal class FolderInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FolderInfo"/> class.
		/// </summary>
		/// <param name="path">Folder path.</param>
		public FolderInfo(string path)
		{
			Path = path;
			Name = new DirectoryInfo(path).Name;
			if (int.TryParse(Name, out int val))
			{
				Number = val;
			}

			SearchSubFolders(path, this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FolderInfo"/> class.
		/// </summary>
		/// <param name="path">Folder path.</param>
		/// <param name="parent">Parent folder info.</param>
		public FolderInfo(string path, FolderInfo parent)
			: this(path)
		{
			Parent = parent;
		}

		/// <summary>
		/// Gets current folder path.
		/// </summary>
		public string Path { get; private set; }

		/// <summary>
		/// Gets current folder name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets a value indicating whether true if folder name is numeric.
		/// </summary>
		public bool IsNumeric => Number.HasValue;

		/// <summary>
		/// Gets number of folder, if name is numeric.
		/// </summary>
		public int? Number { get; private set; }

		/// <summary>
		/// Gets a value indicating whether true if folder contains children.
		/// </summary>
		public bool HasChildren => Children?.Count > 0;

		/// <summary>
		/// Gets a value indicating whether true if folder contains audio files.
		/// </summary>
		public bool HasAudio => AudioFiles?.Count > 0;

		/// <summary>
		/// Gets parent folder info.
		/// </summary>
		public FolderInfo? Parent { get; private set; }

		/// <summary>
		/// Gets children folders infos.
		/// </summary>
		public List<FolderInfo> Children { get; private set; } = new List<FolderInfo>();

		/// <summary>
		/// Gets audio files list in current folder.
		/// </summary>
		public List<string> AudioFiles { get; private set; } = new List<string>();

		/// <summary>
		/// Gets image files list in current folder.
		/// </summary>
		public List<string> ImageFiles { get; private set; } = new List<string>();

		/// <summary>
		/// Returns leaves of folder tree
		/// meaning, folders without children.
		/// </summary>
		/// <param name="folder">Start folder.</param>
		/// <returns>List of leaves folders info.</returns>
		public static List<FolderInfo> GetLeaves(FolderInfo folder)
		{
			var result = new List<FolderInfo>();
			if (folder.Children.Count == 0 && folder.AudioFiles.Count > 0)
			{
				result.Add(folder);
			}

			foreach (var child in folder.Children.OrderBy(a => a.Name))
			{
				result.AddRange(GetLeaves(child));
			}

			return result;
		}

		/// <summary>
		/// Customize display for debug.
		/// </summary>
		/// <returns>Current folder name.</returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Recursive build folder hierarchy from provided path
		/// Folders without subfolders or audio files will be ignored.
		/// </summary>
		/// <param name="path">Search path.</param>
		/// <param name="parent">Parent folder.</param>
		private void SearchSubFolders(string path, FolderInfo parent)
		{
			BookScanService.AudioFiles
				.ToList()
				.ForEach(e => AudioFiles.AddRange(Directory.GetFiles(path, $"*{e}", SearchOption.TopDirectoryOnly)));

			BookScanService.ImageFiles
				.ToList()
				.ForEach(e => ImageFiles.AddRange(Directory.GetFiles(path, $"*{e}", SearchOption.TopDirectoryOnly)));

			foreach (var dir in Directory.GetDirectories(path))
			{
				var childFolder = new FolderInfo(dir, parent);
				if (childFolder.HasChildren || childFolder.HasAudio)
				{
					parent.Children.Add(childFolder);
				}
			}
		}
	}
}
// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using SQLite;

namespace AudioBookPlayer.Core.Model.Entities
{
	/// <summary>
	/// Folder to search books in.
	/// </summary>
	[Table("ScanFolder")]
	public class ScanFolder
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ScanFolder"/> class.
		/// </summary>
		public ScanFolder()
		{
			Path = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScanFolder"/> class.
		/// </summary>
		/// <param name="path">Scan folder path.</param>
		public ScanFolder(string path)
			: this()
		{
			Path = path;
		}

		/// <summary>
		/// Gets or sets current folder path.
		/// </summary>
		[PrimaryKey]
		[MaxLength(500)]
		public string Path { get; set; }
	}
}
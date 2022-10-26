// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AudioBookPlayer.Core.Model.Entities
{
	/// <summary>
	/// Book record.
	/// </summary>
	[Table("Book")]
	public class Book
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Book"/> class.
		/// </summary>
		public Book()
		{
			Files = new List<BookFile>();
		}

		/// <summary>
		/// Gets or sets book identifier.
		/// </summary>
		[PrimaryKey]
		[AutoIncrement]
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets book caption.
		/// </summary>
		[MaxLength(300)]
		[NotNull]
		public string Caption { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets book cover image path.
		/// </summary>
		[MaxLength(500)]
		public string? CoverImagePath { get; set; }

		/// <summary>
		/// Gets or sets book root folder path.
		/// </summary>
		[MaxLength(500)]
		[NotNull]
		public string FolderPath { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets book files list.
		/// </summary>
		[OneToMany]
		public List<BookFile> Files { get; set; }

		/// <summary>
		/// Add new BookFile to Book Files list.
		/// </summary>
		/// <param name="filePath">Book file path.</param>
		public void AddBookFile(string filePath)
		{
			Files.Add(new BookFile(filePath));
		}

		/// <summary>
		/// Debug display information.
		/// </summary>
		/// <returns>Current book folder path.</returns>
		public override string ToString()
		{
			return FolderPath;
		}
	}
}
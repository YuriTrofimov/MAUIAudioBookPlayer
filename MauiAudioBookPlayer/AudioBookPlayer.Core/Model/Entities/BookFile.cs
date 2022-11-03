// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AudioBookPlayer.Core.Model.Entities
{
	/// <summary>
	/// Book file record.
	/// </summary>
	[Table("BookFile")]
	public class BookFile
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BookFile"/> class.
		/// </summary>
		public BookFile()
		{
			Name = string.Empty;
			FilePath = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BookFile"/> class.
		/// </summary>
		/// <param name="filePath">Book file path.</param>
		public BookFile(string filePath)
			: this()
		{
			FilePath = filePath;
			Name = Path.GetFileNameWithoutExtension(filePath);
		}

		/// <summary>
		/// Gets or sets record identifier.
		/// </summary>
		[PrimaryKey]
		[AutoIncrement]
		[Column(nameof(ID))]
		public int? ID { get; set; }

		/// <summary>
		/// Gets or sets owning book record identifier.
		/// </summary>
		[ForeignKey(typeof(Book))]
		public int BookID { get; set; }

		/// <summary>
		/// Gets or sets book file name without extension.
		/// </summary>
		[MaxLength(300)]
		[NotNull]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets book file path.
		/// </summary>
		[MaxLength(500)]
		[NotNull]
		[Unique]
		public string FilePath { get; set; }

		/// <summary>
		/// Debug information.
		/// </summary>
		/// <returns>Current book file path.</returns>
		public override string ToString()
		{
			return FilePath;
		}
	}
}
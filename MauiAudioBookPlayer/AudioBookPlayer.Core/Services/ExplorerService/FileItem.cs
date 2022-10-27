// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using AudioBookPlayer.Core.Common;

namespace AudioBookPlayer.Core.Services.ExplorerService
{
	/// <summary>
	/// File explorer item.
	/// </summary>
	public class FileItem : ExplorerItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileItem"/> class.
		/// </summary>
		/// <param name="path">File path.</param>
		public FileItem(string path)
			: base(path)
		{
			Name = System.IO.Path.GetFileNameWithoutExtension(path);
			var extension = System.IO.Path.GetExtension(path);
			Audio = Constants.AudioFiles.Contains(extension.ToLower());
			Image = Constants.ImageFiles.Contains(extension.ToLower());
		}

		/// <summary>
		/// Gets a value indicating whether file is audio.
		/// </summary>
		public bool Audio { get; private set; }

		/// <summary>
		/// Gets a value indicating whether file is image.
		/// </summary>
		public bool Image { get; private set; }
	}
}
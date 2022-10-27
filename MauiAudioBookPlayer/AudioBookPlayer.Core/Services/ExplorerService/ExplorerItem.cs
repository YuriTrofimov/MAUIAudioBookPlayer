// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AudioBookPlayer.Core.Services.ExplorerService
{
	/// <summary>
	/// Explorer item
	/// </summary>
	public abstract class ExplorerItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExplorerItem"/> class.
		/// </summary>
		/// <param name="path">Item path.</param>
		public ExplorerItem(string path)
		{
			Path = path;
			Name = string.Empty;
		}

		/// <summary>
		/// Gets or sets item path.
		/// </summary>
		public string Path { get; protected set; }

		/// <summary>
		/// Gets or sets item name.
		/// </summary>
		public string Name { get; protected set; }
	}
}
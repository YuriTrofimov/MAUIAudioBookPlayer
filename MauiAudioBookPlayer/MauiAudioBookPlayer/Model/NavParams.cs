// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using AudioBookPlayer.Core.Model.Entities;

namespace MauiAudioBookPlayer.Model
{
	/// <summary>
	/// Navigation parameters dictionary.
	/// </summary>
	public class NavParams : Dictionary<string, object>
	{
		/// <summary>
		/// Gets or sets folder selected in file system explorer.
		/// </summary>
		public string SelectedFolder
		{
			get => GetValue() as string;
			set => SetValue(value);
		}

		/// <summary>
		/// Gets or sets selected book.
		/// </summary>
		public Book SelectedBook
		{
			get => GetValue() as Book;
			set => SetValue(value);
		}

		private object GetValue([CallerMemberName] string key = null)
		{
			if (string.IsNullOrEmpty(key) || !ContainsKey(key))
			{
				return null;
			}

			return this[key];
		}

		private void SetValue(object value, [CallerMemberName] string key = null)
		{
			if (string.IsNullOrEmpty(key))
			{
				return;
			}

			if (ContainsKey(key))
			{
				this[key] = value;
			}
			else
			{
				Add(key, value);
			}
		}
	}
}
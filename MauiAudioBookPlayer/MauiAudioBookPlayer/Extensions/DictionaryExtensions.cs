// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MauiAudioBookPlayer.Model;

namespace MauiAudioBookPlayer.Extensions
{
	/// <summary>
	/// Dictionary extensions class.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Convert navigation query dictionary to NavParams.
		/// </summary>
		/// <param name="query">Navigation query dictionary.</param>
		/// <returns>Navigation parameters dictionary.</returns>
		public static NavParams AsNavParams(this IDictionary<string, object> query)
		{
			var navParams = new NavParams();
			query.ToList().ForEach(r => navParams.Add(r.Key, r.Value));
			return navParams;
		}
	}
}
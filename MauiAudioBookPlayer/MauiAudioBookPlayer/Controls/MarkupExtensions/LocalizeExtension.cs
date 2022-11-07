// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommunityToolkit.Mvvm.DependencyInjection;
using MauiAudioBookPlayer.Resources.Strings;
using Microsoft.Extensions.Localization;

namespace MauiAudioBookPlayer.Controls.MarkupExtensions
{
	/// <summary>
	/// Localization markup extension.
	/// </summary>
	[ContentProperty(nameof(Key))]
	public class LocalizeExtension : IMarkupExtension
	{
		private readonly IStringLocalizer<LocalizableStrings> localizeResources;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizeExtension"/> class.
		/// </summary>
		public LocalizeExtension()
		{
			localizeResources = Ioc.Default.GetService<IStringLocalizer<LocalizableStrings>>();
		}

		/// <summary>
		/// Gets or sets localized string key.
		/// </summary>
		public string Key { get; set; } = string.Empty;

		/// <inheritdoc/>
		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

		private object ProvideValue(IServiceProvider serviceProvider)
		{
			string localizedText = localizeResources[Key];
			return localizedText;
		}
	}
}
// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MauiAudioBookPlayer.Controls
{
	/// <summary>
	/// Sleep mode toolbar item.
	/// </summary>
	internal class SleepModeToolbarItem : ToolbarItem
	{
		/// <summary>
		/// Bindable sleepmode property.
		/// </summary>
		public static readonly BindableProperty SleepModeEnabledProperty = BindableProperty.Create(nameof(SleepModeEnabled), typeof(bool), typeof(SleepModeToolbarItem), true, BindingMode.OneWay, propertyChanged: OnSleepModeChanged);

		/// <summary>
		/// Bindable sleep mode enabled image property.
		/// </summary>
		public static readonly BindableProperty SleepModeEnabledImageProperty = BindableProperty.Create(nameof(SleepModeEnabledImage), typeof(string), typeof(SleepModeToolbarItem), null, BindingMode.OneWay);

		/// <summary>
		/// Bindable sleep mode disabled image property.
		/// </summary>
		public static readonly BindableProperty SleepModeDisabledImageProperty = BindableProperty.Create(nameof(SleepModeDisabledImage), typeof(string), typeof(SleepModeToolbarItem), null, BindingMode.OneWay);

		/// <summary>
		/// Initializes a new instance of the <see cref="SleepModeToolbarItem"/> class.
		/// </summary>
		public SleepModeToolbarItem()
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether sleep mode enabled.
		/// </summary>
		public bool SleepModeEnabled
		{
			get => (bool)GetValue(SleepModeEnabledProperty);
			set => SetValue(SleepModeEnabledProperty, value);
		}

		/// <summary>
		/// Gets or sets sleep mode enabled image.
		/// </summary>
		public string SleepModeEnabledImage
		{
			get => (string)GetValue(SleepModeDisabledImageProperty);
			set => SetValue(SleepModeDisabledImageProperty, value);
		}

		/// <summary>
		/// Gets or sets sleep mode enabled image.
		/// </summary>
		public string SleepModeDisabledImage
		{
			get => (string)GetValue(SleepModeEnabledImageProperty);
			set => SetValue(SleepModeEnabledImageProperty, value);
		}

		/// <inheritdoc/>
		protected override void OnParentChanged()
		{
			base.OnParentChanged();
			UpdateImage();
		}

		private static void OnSleepModeChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var item = bindable as SleepModeToolbarItem;
			item.UpdateImage();
		}

		private void UpdateImage()
		{
			if (SleepModeEnabled)
			{
				Application.Current.Dispatcher.Dispatch(() => IconImageSource = SleepModeEnabledImage);
			}
			else
			{
				Application.Current.Dispatcher.Dispatch(() => IconImageSource = SleepModeDisabledImage);
			}
		}
	}
}
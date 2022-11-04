// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MauiAudioBookPlayer.Services
{
	/// <summary>
	/// Message box viewmodel service.
	/// </summary>
	public class MessageBoxService : IMessageBoxService
	{
		/// <summary>
		/// Display message box.
		/// </summary>
		/// <param name="caption">Message box caption.</param>
		/// <param name="message">Content.</param>
		/// <param name="cancel">Cancel button text.</param>
		/// <returns>Async task.</returns>
		public Task ShowMessageBoxAsync(string caption, string message, string cancel = "OK")
		{
			return Application.Current.MainPage.DisplayAlert(caption, message, cancel);
		}

		/// <summary>
		/// Display confirm box.
		/// </summary>
		/// <param name="caption">Confirm box caption.</param>
		/// <param name="message">Content.</param>
		/// <param name="confirm">Confirm button text.</param>
		/// <param name="cancel">Cancel button text.</param>
		/// <returns>True on confirm.</returns>
		public Task<bool> ShowConfirmBoxAsync(string caption, string message, string confirm = "Yes", string cancel = "No")
		{
			return Application.Current.MainPage.DisplayAlert(caption, message, confirm, cancel);
		}
	}
}
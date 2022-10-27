// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using UIKit;

namespace MauiAudioBookPlayer;

/// <summary>
/// Program class.
/// </summary>
public class Program
{
	/// <summary>
	/// This is the main entry point of the application.
	/// </summary>
	/// <param name="args">Arguments.</param>
	private static void Main(string[] args)
	{
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
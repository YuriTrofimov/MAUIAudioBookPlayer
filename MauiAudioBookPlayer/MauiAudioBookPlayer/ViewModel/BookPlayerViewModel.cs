// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.ComponentModel;
using AudioBookPlayer.Core.Model;
using AudioBookPlayer.Core.Model.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MauiAudio;
using MauiAudioBookPlayer.Extensions;
using MauiAudioBookPlayer.Model;
using MauiAudioBookPlayer.Services;

namespace MauiAudioBookPlayer.ViewModel
{
	/// <summary>
	/// Book player view model.
	/// </summary>
	public partial class BookPlayerViewModel : ObservableObject, IDisposable
	{
		private readonly IAppDataRepository repository;
		private readonly INativeAudioService audioService;
		private readonly IMessageBoxService messageBoxService;
		private readonly SettingsViewModel settingsViewModel;
		private readonly CancellationTokenSource timerToken;

		/// <summary>
		/// Book files collection.
		/// </summary>
		private readonly ObservableCollection<BookFile> files;

		private bool sliderDragging;
		private DateTime lastActivityTime;

		[ObservableProperty]
		private bool busy;

		[ObservableProperty]
		private bool controlsEnabled;

		[ObservableProperty]
		private Book book;

		[ObservableProperty]
		private BookFile selectedFile;

		[ObservableProperty]
		private double fileLength;

		[ObservableProperty]
		private double fileProgress;

		[ObservableProperty]
		private EPlayerState state;

		[ObservableProperty]
		private bool sliderVisible;

		[ObservableProperty]
		private string totalTime;

		[ObservableProperty]
		private string timeProgress;

		/// <summary>
		/// Overall book read progress.
		/// </summary>
		[ObservableProperty]
		private double readProgress;

		/// <summary>
		/// Overall book read progress message.
		/// </summary>
		[ObservableProperty]
		private string progressMessage;

		[ObservableProperty]
		private bool sleepModeEnabled;

		[ObservableProperty]
		private int maxInactivityMinutes;

		[ObservableProperty]
		private string sleepTimeLeft;

		/// <summary>
		/// Initializes a new instance of the <see cref="BookPlayerViewModel"/> class.
		/// </summary>
		public BookPlayerViewModel()
		{
			repository = Ioc.Default.GetService<IAppDataRepository>();
			audioService = Ioc.Default.GetService<INativeAudioService>();
			audioService.PlayEnded += AudioService_PlayEnded;

			messageBoxService = Ioc.Default.GetService<IMessageBoxService>();
			settingsViewModel = Ioc.Default.GetService<SettingsViewModel>();
			settingsViewModel.PropertyChanged += SettingsViewModel_PropertyChanged;

			files = new ObservableCollection<BookFile>();
			timerToken = new CancellationTokenSource();

			State = EPlayerState.Stop;
			maxInactivityMinutes = 10;
			TimerTask();
		}

		/// <summary>
		/// Initialize view model.
		/// </summary>
		/// <param name="bookToPlay">Book to play.</param>
		/// <returns>Async task.</returns>
		public async Task InitializeAsync(Book bookToPlay)
		{
			if (bookToPlay == null)
			{
				return;
			}

			Book = bookToPlay;
			lastActivityTime = DateTime.Now;

			await StopAsync();
			await ReloadFilesAsync();

			SleepModeEnabled = settingsViewModel.SleepTimerEnabled;
			MaxInactivityMinutes = settingsViewModel.SleepTimerPeriod;

			var progress = await BookProgress.LoadAsync(Book.FolderPath);

			void Init()
			{
				FileProgress = progress.Position;
				SelectedFile = files.FirstOrDefault(f => f.FilePath == progress.FilePath);
			}

			if (progress != null)
			{
				if (MainThread.IsMainThread)
				{
					Init();
				}
				else
				{
					MainThread.BeginInvokeOnMainThread(Init);
				}
			}
		}

		/// <summary>
		/// Save book progress.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task SaveProgress()
		{
			if (SelectedFile == null || Book == null)
			{
				return;
			}

			await BookProgress.SaveAsync(Book.FolderPath, SelectedFile.FilePath, FileProgress);
		}

		/// <summary>
		/// Stop current playback.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task StopAsync()
		{
			try
			{
				if (sliderDragging)
				{
					return;
				}

				if (audioService.IsPlaying)
				{
					await audioService.PauseAsync();
					await SaveProgress();
					State = EPlayerState.Pause;
					OnPropertyChanged(nameof(State));
				}
			}
			catch (Exception ex)
			{
				await DisplayError(ex);
			}
		}

		/// <summary>
		/// Stop timers. Clear resources.
		/// </summary>
		public async void Dispose()
		{
			timerToken.Cancel();
			await StopAsync();
			await audioService.DisposeAsync();
		}

		private async Task InitAudioFile()
		{
			try
			{
				Busy = true;
				if (SelectedFile == null)
				{
					return;
				}

				await audioService.InitializeAsync(SelectedFile.FilePath);
				FileLength = audioService.Duration;
				var readedFiles = files.IndexOf(SelectedFile) + 1;
				if (files.Count > 0)
				{
					ReadProgress = (double)readedFiles / (double)files.Count;
					ProgressMessage = $"Files readed {readedFiles} of {files.Count}";
				}
			}
			catch (Exception ex)
			{
				await DisplayError(ex);
			}
			finally
			{
				Busy = false;
			}
		}

		private async Task Play()
		{
			try
			{
				if (SelectedFile == null)
				{
					SelectedFile = files.FirstOrDefault();
				}

				if (SelectedFile != null)
				{
					await audioService.PlayAsync(FileProgress);
					FileLength = audioService.Duration;
					OnPropertyChanged(nameof(FileProgress));
					State = EPlayerState.Play;
				}
			}
			catch (Exception ex)
			{
				await DisplayError(ex);
			}
		}

		[RelayCommand]
		private void ToggleSleepMode()
		{
			SleepModeEnabled = !sleepModeEnabled;
		}

		[RelayCommand]
		private async void SliderDragStart()
		{
			try
			{
				await StopAsync();
			}
			finally
			{
				sliderDragging = true;
			}
		}

		[RelayCommand]
		private async void SliderDragStop()
		{
			try
			{
				lastActivityTime = DateTime.Now;
				await Play();
				await SaveProgress();
			}
			finally
			{
				sliderDragging = false;
			}
		}

		[RelayCommand]
		private async void TogglePlay()
		{
			try
			{
				Busy = true;
				lastActivityTime = DateTime.Now;
				audioService.Volume = 1.0f;
				if (audioService.IsPlaying)
				{
					await StopAsync();
				}
				else
				{
					await Play();
				}
			}
			finally
			{
				Busy = false;
			}
		}

		[RelayCommand]
		private async void PlayPrevious()
		{
			try
			{
				Busy = true;
				FileProgress = 0.0f;
				var currentIndex = files.IndexOf(SelectedFile);
				if (files.Count > 0)
				{
					if (currentIndex - 1 >= 0)
					{
						SelectedFile = files[currentIndex - 1];
						await Play();
					}
					else
					{
						await audioService.SetCurrentTime(0.0f);
					}
				}
			}
			finally
			{
				Busy = false;
			}
		}

		[RelayCommand]
		private async void PlayNext()
		{
			try
			{
				Busy = true;
				FileProgress = 0.0f;
				var currentIndex = files.IndexOf(SelectedFile);
				if (currentIndex + 1 < files.Count)
				{
					SelectedFile = files[currentIndex + 1];
					await Play();
				}
			}
			finally
			{
				Busy = false;
			}
		}

		private async Task ReloadFilesAsync()
		{
			files.Clear();
			var filesList = await repository.GetAllBookFilesAsync(Book);
			filesList.ForEach(f => files.Add(f));
		}

		/// <summary>
		/// Update file progress task.
		/// </summary>
		private async void TimerTask()
		{
			while (!timerToken.IsCancellationRequested)
			{
				if (audioService.IsPlaying && !sliderDragging)
				{
					await UpdateProgress();
				}

				await Task.Delay(1000, timerToken.Token);
			}
		}

		private async Task UpdateProgress()
		{
			if (SleepModeEnabled && (DateTime.Now - lastActivityTime).TotalSeconds > MaxInactivityMinutes)
			{
				if (audioService.Volume > 0.2f)
				{
					audioService.Volume -= 0.05;
				}
				else
				{
					await StopAsync();
				}
			}

			FileProgress = audioService.CurrentPosition;
		}

		partial void OnFileProgressChanged(double value)
		{
			TimeProgress = TimeSpan.FromSeconds(value).ToFormattedTime();

			var maxSeconds = TimeSpan.FromMinutes(MaxInactivityMinutes).TotalSeconds;
			var playedSeconds = (DateTime.Now - lastActivityTime).TotalSeconds;
			SleepTimeLeft = TimeSpan.FromSeconds(maxSeconds - playedSeconds).ToFormattedTime();
		}

		partial void OnFileLengthChanged(double value)
		{
			TotalTime = TimeSpan.FromSeconds(value).ToFormattedTime();
			SliderVisible = value > 0;
		}

		partial void OnBusyChanged(bool value)
		{
			ControlsEnabled = !value;
		}

		async partial void OnSelectedFileChanged(BookFile value)
		{
			await InitAudioFile();
		}

		partial void OnSleepModeEnabledChanged(bool value)
		{
			lastActivityTime = DateTime.Now;
		}

		private void AudioService_PlayEnded(object sender, EventArgs e)
		{
			PlayNext();
		}

		private async Task DisplayError(Exception ex)
		{
			await messageBoxService.ShowMessageBoxAsync("Error", ex.Message);
		}

		private void SettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			SleepModeEnabled = settingsViewModel.SleepTimerEnabled;
			MaxInactivityMinutes = settingsViewModel.SleepTimerPeriod;
		}
	}
}
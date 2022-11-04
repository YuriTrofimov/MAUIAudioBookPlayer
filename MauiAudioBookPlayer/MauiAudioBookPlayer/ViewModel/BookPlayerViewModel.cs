// Copyright (c) 2022 Yuri Trofimov.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
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
		private readonly CancellationTokenSource timerToken;
		private readonly Task timerTask;
		private bool sliderDragging;

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
		private ObservableCollection<BookFile> files;

		[ObservableProperty]
		private string totalTime;

		[ObservableProperty]
		private string timeLeft;

		[ObservableProperty]
		private string timeProgress;

		/// <summary>
		/// Initializes a new instance of the <see cref="BookPlayerViewModel"/> class.
		/// </summary>
		public BookPlayerViewModel()
		{
			repository = Ioc.Default.GetService<IAppDataRepository>();
			audioService = Ioc.Default.GetService<INativeAudioService>();
			messageBoxService = Ioc.Default.GetService<IMessageBoxService>();
			files = new ObservableCollection<BookFile>();
			timerToken = new CancellationTokenSource();
			timerTask = Task.Run(TimerTask, timerToken.Token);
			PropertyChanged += BookPlayerViewModel_PropertyChanged;
			audioService.PlayEnded += AudioService_PlayEnded;
			State = EPlayerState.Stop;
		}

		/// <summary>
		/// Initialize view model.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task InitializeAsync()
		{
			await Stop();
			await ReloadFilesAsync();

			async void Init()
			{
				var progress = await BookProgress.LoadAsync(Book.FolderPath);
				if (progress != null)
				{
					SelectedFile = Files.FirstOrDefault(f => f.FilePath == progress.FilePath);
				}
			}

			if (MainThread.IsMainThread)
			{
				Init();
			}
			else
			{
				MainThread.BeginInvokeOnMainThread(Init);
			}
		}

		/// <summary>
		/// Save book progress.
		/// </summary>
		/// <returns>Async task.</returns>
		public async Task SaveProgress()
		{
			await BookProgress.SaveAsync(Book.FolderPath, SelectedFile.FilePath, FileProgress);
		}

		/// <summary>
		/// Stop timers. Clear resources.
		/// </summary>
		public async void Dispose()
		{
			timerToken.Cancel();
			await Stop();
			await audioService.DisposeAsync();
		}

		private async Task InitAudioFile(double progress = 0.0f)
		{
			try
			{
				if (SelectedFile == null)
				{
					return;
				}

				await audioService.InitializeAsync(SelectedFile.FilePath);
				FileProgress = progress;
			}
			catch (Exception ex)
			{
				await DisplayError(ex);
			}
		}

		[RelayCommand]
		private async Task Play()
		{
			try
			{
				if (SelectedFile == null)
				{
					SelectedFile = Files.FirstOrDefault();
				}

				if (SelectedFile != null)
				{
					await audioService.PlayAsync(FileProgress);
					State = EPlayerState.Play;
				}
			}
			catch (Exception ex)
			{
				await DisplayError(ex);
			}
		}

		[RelayCommand]
		private async Task Stop()
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
					State = EPlayerState.Pause;
				}
			}
			catch (Exception ex)
			{
				await DisplayError(ex);
			}
		}

		[RelayCommand]
		private async void SliderDragStart()
		{
			await Stop();
			sliderDragging = true;
		}

		[RelayCommand]
		private async void SliderDragStop()
		{
			sliderDragging = false;
			await Play();
		}

		[RelayCommand]
		private async void TogglePlay()
		{
			if (State == EPlayerState.Play)
			{
				await Stop();
			}
			else
			{
				await Play();
			}
		}

		[RelayCommand]
		private async void PlayPrevious()
		{
			var currentIndex = Files.IndexOf(SelectedFile);
			if (Files.Count > 0)
			{
				if (currentIndex - 1 >= 0)
				{
					SelectedFile = Files[currentIndex - 1];
					await Play();
				}
				else
				{
					await audioService.SetCurrentTime(0.0f);
				}
			}
		}

		[RelayCommand]
		private async void PlayNext()
		{
			var currentIndex = Files.IndexOf(SelectedFile);
			if (currentIndex + 1 < Files.Count)
			{
				SelectedFile = Files[currentIndex + 1];
				await Play();
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
		/// <returns>Async task.</returns>
		private async Task TimerTask()
		{
			void UpdateProgress()
			{
				FileLength = audioService.Duration;
				FileProgress = audioService.CurrentPosition;
			}

			while (!timerToken.IsCancellationRequested)
			{
				if (audioService.IsPlaying && !sliderDragging)
				{
					if (MainThread.IsMainThread)
					{
						UpdateProgress();
					}
					else
					{
						MainThread.BeginInvokeOnMainThread(UpdateProgress);
					}
				}

				await Task.Delay(500, timerToken.Token);
			}
		}

		private async void BookPlayerViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(SelectedFile):
					await InitAudioFile();
					break;

				case nameof(FileProgress):
					TimeLeft = TimeSpan.FromSeconds(FileLength - FileProgress).ToFormattedTime();
					TimeProgress = TimeSpan.FromSeconds(FileProgress).ToFormattedTime();
					break;

				case nameof(FileLength):
					TotalTime = TimeSpan.FromSeconds(FileLength).ToFormattedTime();
					break;
			}

			SliderVisible = FileLength > 0;
		}

		private void AudioService_PlayEnded(object sender, EventArgs e)
		{
			PlayNext();
		}

		private async Task DisplayError(Exception ex)
		{
			await messageBoxService.ShowMessageBoxAsync("Error", ex.Message);
		}
	}
}
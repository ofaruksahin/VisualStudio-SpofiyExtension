using Autofac;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using VSIXSpotify.AddIn.Core;
using VSIXSpotify.AddIn.Core.IRepository;
using VSIXSpotify.AddIn.Core.Spotify;
using VSIXSpotify.AddIn.Infrastructure;

namespace VSIXSpotify.AddIn.UI
{
    public partial class SpotifyControl : UserControl
    {
        private DTE _dte = null;
        private IAuthService authService = null;
        private ISpotifyService spotifyService = null;
        private DeviceList devices = null;
        private Device selectedDevice = null;
        private CurrentPlaybackState currentPlaybackState = null;
        private Timer timerRefreshToken = null;
        private Timer timerPlaybackState = null;
        private int timerRefreshTokenDelay = 1;
        private int timerPlaybackStateDelay = 10;
        private object timerRefreshTokenLock = new object();
        private object timerPlaybackStateLock = new object();

        public SpotifyControl()
        {
            this.InitializeComponent();
            this.Loaded += SpotifyControl_Loaded;
            this.ToolTipOpening += SpotifyControl_ToolTipOpening;
            this.ToolTipClosing += SpotifyControl_ToolTipClosing;
            this.Unloaded += SpotifyControl_Unloaded;
        }

        private void SpotifyControl_ToolTipClosing(object sender, ToolTipEventArgs e)
        {
            authorizationBrowser.LoadCompleted -= AuthorizationBrowser_LoadCompleted;
            timerRefreshToken = null;
            timerPlaybackState = null;
        }

        private void SpotifyControl_Unloaded(object sender, RoutedEventArgs e)
        {
            authorizationBrowser.LoadCompleted -= AuthorizationBrowser_LoadCompleted;
            timerRefreshToken = null;
            timerPlaybackState = null;
        }

        private void SpotifyControl_Loaded(object sender, RoutedEventArgs e)
        {
            _dte = DTEHelper.GetObjectDTE();
            ThreadHelper.ThrowIfNotOnUIThread();
            DTEHelper.AddEvents(_dte.Events.SolutionEvents);
            _dte.Events.SolutionEvents.Opened += SolutionEvents_Opened;

            ContainerHelper
                .Build()
                .TryResolve<IAuthService>(out authService);

            ContainerHelper
                .Build()
                .TryResolve<ISpotifyService>(out spotifyService);

            authService.DeleteFile();

            if (_dte.Solution.IsOpen)
                SolutionOpened();
            timerRefreshToken = new Timer(TimerRefreshTokenDoWork, null, 0, (int)TimeSpan.FromMinutes(timerRefreshTokenDelay).TotalMilliseconds);
            timerPlaybackState = new Timer(TimerPlaybackStateDoWork, null, 0, (int)TimeSpan.FromSeconds(timerPlaybackStateDelay).TotalMilliseconds);
        }

        private void SpotifyControl_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            if (_dte.Solution.IsOpen)
                SolutionOpened();
            timerRefreshToken = new Timer(TimerRefreshTokenDoWork, null, 0, (int)TimeSpan.FromMinutes(timerRefreshTokenDelay).TotalMilliseconds);
            timerPlaybackState = new Timer(TimerPlaybackStateDoWork, null, 0, (int)TimeSpan.FromSeconds(timerPlaybackStateDelay).TotalMilliseconds);
        }

        private void SolutionEvents_Opened()
        {
            SolutionOpened();
        }

        private void SolutionOpened()
        {
            authorizationBrowser.LoadCompleted -= AuthorizationBrowser_LoadCompleted;
            authorizationBrowser.LoadCompleted += AuthorizationBrowser_LoadCompleted;
            var isAuthenticated = authService.IsAuthenticated();
            if (isAuthenticated)
            {
                HideLogInTab();
                return;
            }
            else
            {
                ShowLoginTab();
                return;
            }
        }

        private async void AuthorizationBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.AbsolutePath == "/callback")
            {
                var code = string.Empty;
                var queries = e.Uri.Query.Split('=').Select(f => f.TrimStart('?').TrimStart('|')).ToList();
                var indexOf = queries.IndexOf("code");
                if (indexOf < 0)
                    return;
                code = queries[indexOf + 1];
                var isTrue = await authService.GetToken(code);
                if (isTrue)
                {
                    var isAuthenticated = authService.IsAuthenticated();
                    if (isAuthenticated)
                        HideLogInTab();
                    else
                        ShowLoginTab();
                }
                else
                {
                    ShowLoginTab();
                }
            }
        }

        #region UIEvents
        private void HideLogInTab()
        {
            authorizationTabItem.Visibility = Visibility.Hidden;
            authorizationBrowser.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 1;
            if (spotifyService != null)
            {
                GetDevices();
            }
        }

        private void ShowLoginTab()
        {
            devices = new DeviceList();
            selectedDevice = null;
            currentPlaybackState = null;
            authorizationTabItem.Visibility = Visibility.Visible;
            authorizationBrowser.Visibility = Visibility.Visible;
            tabControl.SelectedIndex = 0;
            string redirectUrl = Options.AuthorizationUrl;
            authorizationBrowser.Source = new System.Uri(redirectUrl + "/redirect");
        }

        private async void GetDevices()
        {
            devices = await spotifyService.GetDevices();
            deviceListView.DataContext = devices.Devices;
            selectedDevice = devices?.Devices?.FirstOrDefault(f => f.IsActive);
            if (selectedDevice != null)
                SetCurrentPlaybackState();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            selectedDevice = devices?.Devices?.FirstOrDefault(f => f.IsActive);
        }

        private async void SetCurrentPlaybackState()
        {
            currentPlaybackState = await spotifyService.GetCurrentPlaybackState();
            if (currentPlaybackState._Item != null && currentPlaybackState._Item.Album != null)
            {
                if (currentPlaybackState._Item.Album.Images != null && currentPlaybackState._Item.Album.Images.Any())
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Thumbnail.DataContext = currentPlaybackState._Item.Album.Images.FirstOrDefault();
                    });
                }
            }
            if (currentPlaybackState._Item != null && currentPlaybackState._Item.Artists != null)
            {
                if (currentPlaybackState._Item.Artists != null && currentPlaybackState._Item.Artists.Any())
                {
                    var artistNames = currentPlaybackState._Item.Artists.Select(f => f.Name);
                    this.Dispatcher.Invoke(() =>
                    {
                        SelectedMusicArtist.Text = string.Join("&", artistNames);
                    });
                }
            }
            if (currentPlaybackState._Item != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    SelectedMusic.Text = currentPlaybackState._Item.Name;
                });
            }
        }

        private async void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDevice != null)
            {
                await spotifyService.PreviousSong(selectedDevice);
                SetCurrentPlaybackState();
            }
        }

        private async void btnPauseOrPlay_Click(object sender, RoutedEventArgs e)
        {
            if(currentPlaybackState!=null && selectedDevice != null)
            {
                await spotifyService.PlayOrPause(currentPlaybackState, selectedDevice);
                SetCurrentPlaybackState();
            }
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDevice != null)
            {
                await spotifyService.NextSong(selectedDevice);
                SetCurrentPlaybackState();
            }
        }
        #endregion

        #region TimerEvents
        private void TimerRefreshTokenDoWork(object state)
        {
            if (!Monitor.TryEnter(timerRefreshTokenLock))
                return;

            if (authService != null)
            {
                if (authService.IsAuthenticated())
                {
                    var isOk = authService.RefreshToken().Result;
                    if (!isOk)
                    {
                        ShowMessage("Session is expired. Please try open tool");
                    }
                }
            }

            Monitor.Exit(timerRefreshTokenLock);
        }

        private void TimerPlaybackStateDoWork(object state)
        {
            if (!Monitor.TryEnter(timerPlaybackStateLock))
                return;

            if (authService != null)
            {
                if (authService.IsAuthenticated())
                {
                    if (selectedDevice != null)
                    {
                        SetCurrentPlaybackState();
                    }
                }
            }


            Monitor.Exit(timerPlaybackStateLock);
        }

        private void ShowMessage(string Message) => MessageBox.Show(Message);
        #endregion    
    }
}
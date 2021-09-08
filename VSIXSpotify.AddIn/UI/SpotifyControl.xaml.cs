using Autofac;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VSIXSpotify.AddIn.Core;
using VSIXSpotify.AddIn.Core.IRepository;
using VSIXSpotify.AddIn.Infrastructure;

namespace VSIXSpotify.AddIn.UI
{
    public partial class SpotifyControl : UserControl
    {
        private DTE _dte = null;
        private IAuthService authService = null;
        private ISpotifyService spotifyService = null;


        public SpotifyControl()
        {
            this.InitializeComponent();
            this.Loaded += SpotifyControl_Loaded;
            this.ToolTipOpening += SpotifyControl_ToolTipOpening;
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
        }

        private void SpotifyControl_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            if (_dte.Solution.IsOpen)
                SolutionOpened();
        }

        private void SolutionEvents_Opened()
        {
            SolutionOpened();
        }

        private void SolutionOpened()
        {
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
                var isGetToken = await authService.GetToken(code);
                if (isGetToken)
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
            authorizationTabItem.Visibility = Visibility.Visible;
            authorizationBrowser.Visibility = Visibility.Visible;
            tabControl.SelectedIndex = 0;
            string redirectUrl = Options.AuthorizationUrl;
            authorizationBrowser.Source = new System.Uri(redirectUrl + "/redirect");
        }

        private async void GetDevices()
        {
            var devices = await spotifyService.GetDevices();
        }
        #endregion
    }
}
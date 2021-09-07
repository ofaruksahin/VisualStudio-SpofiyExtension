﻿using Autofac;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using VSIXSpotify.AddIn.Core.IRepository;
using VSIXSpotify.AddIn.Infrastructure;

namespace VSIXSpotify.AddIn.UI
{
    public partial class SpotifyControl : UserControl
    {
        private DTE _dte = null;
        private IAuthService authService = null;
        public SpotifyControl()
        {
            this.InitializeComponent();
            this.Loaded += SpotifyControl_Loaded;
            this.Unloaded += SpotifyControl_Unloaded;
        }

        private void SpotifyControl_Loaded(object sender, RoutedEventArgs e)
        {
            HideLogInTab();            
            _dte = DTEHelper.GetObjectDTE();
            ThreadHelper.ThrowIfNotOnUIThread();
            DTEHelper.AddEvents(_dte.Events.SolutionEvents);
            _dte.Events.SolutionEvents.Opened += SolutionEvents_Opened;
            if (_dte.Solution.IsOpen)
                SolutionOpened();
            ContainerHelper
                .Build()
                .TryResolve<IAuthService>(out authService);
        }

        private void SpotifyControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= SpotifyControl_Loaded;
            this.Unloaded -= SpotifyControl_Unloaded;
            authorizationBrowser.Navigated -= AuthorizationBrowser_Navigated;
            ThreadHelper.ThrowIfNotOnUIThread();
            _dte.Events.SolutionEvents.Opened -= SolutionEvents_Opened;
            DTEHelper.Dispose();
            ContainerHelper.Dispose();
        }

        private void SolutionEvents_Opened()
        {
            SolutionOpened();
        }

        private void AuthorizationBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        private void SolutionOpened()
        {
            authorizationBrowser.Navigated += AuthorizationBrowser_Navigated;
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

      

        #region UIEvents
        private void HideLogInTab()
        {
            authorizationTabItem.Visibility = Visibility.Hidden;
            authorizationBrowser.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 1;
        }

        private void ShowLoginTab()
        {
            authorizationTabItem.Visibility = Visibility.Visible;
            authorizationBrowser.Visibility = Visibility.Visible;
            tabControl.SelectedIndex = 0;
            //authorizationBrowser.Source = new System.Uri("");
        }
        #endregion
    }
}
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using zombiesnu.DayZeroLauncher.App.Core;
using zombiesnu.DayZeroLauncher.App.Ui.Controls;
using MonoTorrent.Common;

namespace zombiesnu.DayZeroLauncher.App.Ui
{
	/// <summary>
	/// Interaction logic for UpdatesView.xaml
	/// </summary>
	public partial class UpdatesView : UserControl
	{
		public UpdatesView()
		{
			InitializeComponent();
		}

		private void Done_Click(object sender, RoutedEventArgs e)
		{
			((UpdatesViewModel) DataContext).IsVisible = false;
		}

		private void CheckNow_Click(object sender, RoutedEventArgs e)
		{
			((UpdatesViewModel) DataContext).CheckForUpdates();
		}

		private void DownloadArma2_Click(object sender, RoutedEventArgs e)
		{
			((UpdatesViewModel) DataContext).Arma2Updater.InstallLatestVersion();
		}

		private void DownloadDayZ_Click(object sender, RoutedEventArgs e)
		{
			((UpdatesViewModel) DataContext).DayZUpdater.InstallLatestVersion();
		}

		private void ApplyDayZeroLauncherUpdateNow_Click(object sender, RoutedEventArgs e)
		{
			App.ApplyUpdateIfNeccessary();
		}

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            string torrentUrl;
            if (!GameUpdater.HttpGet("http://www.zombies.nu/dayzerotorrent.txt", out torrentUrl))
            {
                InfoPopup popup = new InfoPopup();
                popup.Headline.Content = "An Error occured.";
                popup.Message.Content = "Could not contact Zombies.nu.\nPlease try again.";
                popup.Owner = popup.Owner = MainWindow.GetWindow(this.Parent);
                popup.Title = "Error";
                popup.Show();
                return;
            }
            else
            {
                TorrentState state = TorrentUpdater.CurrentState();
                if (state == TorrentState.Stopped)
                {
                    TorrentUpdater verifier = new TorrentUpdater(torrentUrl); // Sets up launcher to start checking files.
                    verifier.StartTorrents(1);
                }
                FileVerifierPopup popup = new FileVerifierPopup();
                popup.Owner = MainWindow.GetWindow(this.Parent);
                popup.Headline.Content = "Please Wait";
                popup.Title = "Please Wait";

                popup.Show();
            }
        }
	}
}

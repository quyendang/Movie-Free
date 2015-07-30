using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using Microsoft.Advertising.Mobile.UI;
using System.Diagnostics;
using Coding4Fun.Toolkit.Controls;

namespace FreeApp
{
    public partial class PlayerPage : PhoneApplicationPage
    {
        public string stream;
        public string _name;
        private DispatcherTimer dispatcherTimer;
        public PlayerPage()
        {
            InitializeComponent();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 20);
            dispatcherTimer.Start();
            try
            {
                if (NavigationContext.QueryString.TryGetValue("URL", out stream))
                {
                    NavigationContext.QueryString.TryGetValue("NAME", out _name);
                    if (stream.Contains("http"))
                    {
                        player.Source = new Uri("http://www.phimmoi.net/player.php?url=" + HttpUtility.UrlDecode(stream), UriKind.RelativeOrAbsolute);
                        player.Play();
                    }
                    else
                    {
                        player.Source = new Uri(HttpUtility.UrlDecode(stream), UriKind.RelativeOrAbsolute);
                        player.Play();
                    }
                }
            }
            catch
            {
            }
        }
        private void adControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            adControl.Visibility = System.Windows.Visibility.Collapsed;
            dispatcherTimer.Stop();
        }
        private void player_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ToastPrompt tost = new ToastPrompt()
            {
                Title = "Watching:",
                Message = _name
            };
            tost.Show();
        }
    }
}
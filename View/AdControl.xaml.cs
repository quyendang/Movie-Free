using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;

using System.Threading.Tasks;
using System.Windows.Threading;

namespace FreeApp.View
{
    public partial class AdControl : UserControl
    {
        public AdControl()
        {
            InitializeComponent();
            
        }

        private void adControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
           // adControl2.Refresh();
           // MessageBox.Show(e.Error.Message);
            adControl.Visibility = System.Windows.Visibility.Collapsed;
            adControl2.Visibility = System.Windows.Visibility.Visible;
        }

        private void adControl2_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            
            adControl.Visibility = System.Windows.Visibility.Visible;
            adControl2.Visibility = System.Windows.Visibility.Collapsed;
        }

    }
}

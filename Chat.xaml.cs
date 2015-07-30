using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using Microsoft.Phone.Info;

namespace FreeApp
{
    public partial class Chat : PhoneApplicationPage
    {

        public Chat()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                App.sk.sendMessage(DeviceStatus.DeviceName.ToString() + ":" + enter.Text);
                this.enter.Text = "";
                ContactListBox.Focus();
            }
        }

    }
}
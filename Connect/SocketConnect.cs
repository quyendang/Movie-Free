using Coding4Fun.Toolkit.Controls;
using FreeApp.Utils;
using Microsoft.Phone.Tasks;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FreeApp.Connect
{
    public class SocketConnect
    {
        private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 240, 248, 255));
        private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 135, 189));
        private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));
        public int i = 0;
        public Socket mSocket;
        public void connectSocket()
        {
            try
            {
                IO.Options _option = new IO.Options();
                _option.Timeout = 5000;
                _option.Reconnection = true;
                _option.ReconnectionDelay = 0;

                mSocket = IO.Socket("http://lucstudio.com:3000", _option);
                mSocket.On(Socket.EVENT_CONNECT, () =>
                {
                    Debug.WriteLine("OK");
                });
                mSocket.On(Socket.EVENT_CONNECT_ERROR, () =>
                {
                    Debug.WriteLine("Connect Error!");
                });
                mSocket.On(Socket.EVENT_CONNECT_TIMEOUT, () =>
                {
                    Debug.WriteLine("Coonect time out");
                });
                mSocket.On(Socket.EVENT_ERROR, () =>
                {
                    Debug.WriteLine("ERROR");
                    mSocket.On(Socket.EVENT_CONNECT, () =>
                    {
                        Debug.WriteLine("OK");
                    });
                });
                mSocket.On("chat message", (data) =>
                {
                    Dispatcher dispatcher = Deployment.Current.Dispatcher;
                    dispatcher.BeginInvoke(() =>
                    {
                        if (data.ToString().Contains("<ADMIN>"))
                        {
                            ToastPrompt tost = new ToastPrompt()
                            {
                                Title = "ADMIN",
                                Message = data.ToString().Replace("<ADMIN>", "")
                            };
                            tost.Show();
                            App.ViewModel.ChatItems.Add(new ViewModels.ChatItem() { MessageText = data.ToString().Replace("<ADMIN>", "") });
                        }
                        else
                        {
                            if (data.ToString().Contains("ShowAds"))
                            {
                                i = Convert.ToInt32(UIHelper.GetTxtBtwn(data.ToString(), "<AD>", "</AD>", 0));
                                var toast = new ToastPrompt
                                {
                                    IsAppBarVisible = false,
                                    Background = _aliceBlueSolidColorBrush,
                                    Foreground = _cornFlowerBlueSolidColorBrush,
                                    Title = App.listAds[i].Title,
                                    Message = App.listAds[i].Message,
                                    FontSize = 20,
                                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
                                    ImageSource =
                                        new BitmapImage(new Uri(App.listAds[i].Image, UriKind.RelativeOrAbsolute))
                                };
                                toast.MillisecondsUntilHidden = 7000;
                                toast.Tap += toasts_Tap;
                                toast.Show();
                            }
                            else
                            {
                                App.ViewModel.ChatItems.Add(new ViewModels.ChatItem() { MessageText = data.ToString() });
                            }
                        }

                    });
                });
                mSocket.Connect();
            }
            catch (Exception e)
            {
                Debug.WriteLine("IO exception  " + e.Message);
                mSocket.Close();
                mSocket.Open();
            }
        }
        private void toasts_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri(App.listAds[i].Url, UriKind.Absolute);

            webBrowserTask.Show();
        }
        public void sendMessage(string mess)
        {
            mSocket.Emit("chat message", mess);
        }
    }
}

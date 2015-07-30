using Coding4Fun.Toolkit.Controls;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace FreeApp.Connect
{
    public class SocketConnect
    {
        public Socket mSocket;
        public void connectSocket()
        {
            try
            {
                IO.Options _option = new IO.Options();
                _option.Timeout = 5000;
                _option.Reconnection = true;
                _option.ReconnectionDelay = 0;

                mSocket = IO.Socket("http://lucstudio.cloudapp.net:3000", _option);
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
                            App.ViewModel.ChatItems.Add(new ViewModels.ChatItem() { MessageText = data.ToString() });
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
        public void sendMessage(string mess)
        {
            mSocket.Emit("chat message", mess);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Networking.BackgroundTransfer;
using System.Collections.ObjectModel;
using System.Threading;
using Windows.Storage;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using FreeApp.ViewModels;
using FreeApp.Utils;
using Coding4Fun.Toolkit.Controls;

namespace FreeApp
{
    public partial class BackgroundTransferList : PhoneApplicationPage
    {
        private string select = "1";
        public BackgroundTransferList()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }
        private List<DownloadOperation> activeDownloads;
        private CancellationTokenSource cancelToken = new CancellationTokenSource();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("select", out select);
            StorageFolder fd = await ApplicationData.Current.LocalFolder.CreateFolderAsync("shared", CreationCollisionOption.OpenIfExists);
            StorageFolder fd2 = await fd.CreateFolderAsync("transfers", CreationCollisionOption.OpenIfExists);
            var files = await fd2.GetFilesAsync();
            App.ViewModel.FileItems.Clear();
            foreach (var item in files)
            {
                string _name = item.Name.Replace(".mp4", "");
                string _path = item.Path;
                string _day = item.DateCreated.ToString();
                string _filename = item.Path;
                App.ViewModel.FileItems.Add(new ItemViewModel() { Title = _name, URL = _path, Information = _day, Quality = _filename });
            }
            FileList.ItemsSource = App.ViewModel.FileItems;
            MainPivot.SelectedIndex = Convert.ToInt32(select);
            await DiscoverActiveDownloadsAsync();

        }
        private async Task DiscoverActiveDownloadsAsync()
        {
            activeDownloads = new List<DownloadOperation>();

            IReadOnlyList<DownloadOperation> downloads = null;
            try
            {
                downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return;
            }


            if (downloads.Count > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (DownloadOperation download in downloads)
                {
                    Debug.WriteLine(String.Format(CultureInfo.CurrentCulture,
                        "Discovered background download: {0}, Status: {1}", download.Guid,
                        download.Progress.Status));

                    tasks.Add(HandleDownloadAsync(download, false));
                }

                await Task.WhenAll(tasks);
            }
        }

        private async Task HandleDownloadAsync(DownloadOperation download, bool start)
        {
            try
            {
                TransferModel transfer = new TransferModel();
                transfer.DownloadOperation = download;
                transfer.Source = download.RequestedUri.ToString();
                transfer.Destination = download.ResultFile.Path;
                transfer.BytesReceived = download.Progress.BytesReceived;
                transfer.TotalBytesToReceive = download.Progress.TotalBytesToReceive;
                transfer.Progress = 0;
                transfer.Title = download.ResultFile.Name.Replace(".mp4", "");
                transfer.PhanTram = "0 %";
                App.ViewModel.transfers.Add(transfer);
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                await download.AttachAsync().AsTask(cancelToken.Token, progressCallback);

                ResponseInformation response = download.GetResponseInformation();
                Debug.WriteLine(String.Format(CultureInfo.CurrentCulture, "Completed: {0}, Status Code: {1}",
                    download.Guid, response.StatusCode));
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Canceled: " + download.Guid);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                App.ViewModel.transfers.Remove(App.ViewModel.transfers.First(p => p.DownloadOperation == download));
                activeDownloads.Remove(download);
            }
        }

        private void DownloadProgress(DownloadOperation download)
        {
            try
            {
                TransferModel transfer = App.ViewModel.transfers.First(p => p.DownloadOperation == download);
                transfer.Progress = (int)((download.Progress.BytesReceived * 100) / download.Progress.TotalBytesToReceive);
                transfer.BytesReceived = download.Progress.BytesReceived;
                transfer.TotalBytesToReceive = download.Progress.TotalBytesToReceive;
                transfer.PhanTram = (int)((download.Progress.BytesReceived * 100) / download.Progress.TotalBytesToReceive) + " %";
                transfer.FileSize = Math.Round((float)(download.Progress.TotalBytesToReceive / (1024 * 1024)), 2).ToString() + "MB";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        //private void btnPause_Click(object sender, RoutedEventArgs e)
        //{

        //    foreach (TransferModel transfer in transfers)
        //    {
        //        if (transfer.DownloadOperation.Progress.Status == BackgroundTransferStatus.Running)
        //        {
        //            transfer.DownloadOperation.Pause();
        //        }
        //    }
        //}

        //private void btnResume_Click(object sender, RoutedEventArgs e)
        //{

        //    foreach (TransferModel transfer in transfers)
        //    {
        //        if (transfer.DownloadOperation.Progress.Status == BackgroundTransferStatus.PausedByApplication)
        //        {
        //            transfer.DownloadOperation.Resume();
        //        }
        //    }
        //}


        private void BtnSubscribe_Click(object sender, RoutedEventArgs e)
        {
            TransferModel myobject = (sender as Button).DataContext as TransferModel;

            //Get the selected ListBoxItem container instance of the item whose marks button is pressed
            ListBoxItem pressedItem = this.TransferList.ItemContainerGenerator.ContainerFromItem(myobject) as ListBoxItem;

            //Checks whether it is not null 
            if (pressedItem != null)
            {
                //Now you can get the name and marks of selected student from the myobject

                //Display the name and marks of Selected Student in the textblocks given below the listbox.
                //Studentnameblock.Text = myobject.name;
                //marksblock.Text = myobject.marks.ToString(); ;
                if ((sender as Button).Style == Application.Current.Resources[(object)"SubscribeCheck"] as Style)
                {
                    (sender as Button).Style = Application.Current.Resources[(object)"SubscribeUncheckedDark"] as Style;
                    if (myobject.DownloadOperation.Progress.Status == BackgroundTransferStatus.PausedByApplication)
                    {
                        myobject.DownloadOperation.Resume();
                    }
                }
                else
                {
                    (sender as Button).Style = Application.Current.Resources[(object)"SubscribeCheck"] as Style;
                    if (myobject.DownloadOperation.Progress.Status == BackgroundTransferStatus.Running)
                    {
                        myobject.DownloadOperation.Pause();
                    }
                }

            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                cancelToken.Cancel();
                cancelToken.Dispose();

                cancelToken = new CancellationTokenSource();
            }
            catch { }
            //MessageBox.Show(FileList.Items.Count.ToString());
        }
        private async void deleteb_Click(object sender, EventArgs e)
        {
            try
            {
                StorageFolder fd = await ApplicationData.Current.LocalFolder.CreateFolderAsync("shared", CreationCollisionOption.OpenIfExists);
                StorageFolder fd2 = await fd.CreateFolderAsync("transfers", CreationCollisionOption.OpenIfExists);
                await fd2.DeleteAsync(StorageDeleteOption.PermanentDelete);
                StorageFolder fd3 = await ApplicationData.Current.LocalFolder.CreateFolderAsync("shared", CreationCollisionOption.OpenIfExists);
                StorageFolder fd4 = await fd3.CreateFolderAsync("transfers", CreationCollisionOption.OpenIfExists);
                var files = await fd4.GetFilesAsync();
                App.ViewModel.FileItems.Clear();
                foreach (var item in files)
                {
                    string _name = item.Name.Replace(".mp4", "");
                    string _path = item.Path;
                    string _day = item.DateCreated.ToString();
                    string _filename = item.Path;
                    App.ViewModel.FileItems.Add(new ItemViewModel() { Title = _name, URL = _path, Information = _day, Quality = _filename });
                }
                FileList.ItemsSource = App.ViewModel.FileItems;
            }
            catch
            {

            }
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 0)
            {
                ApplicationBar.Mode = ApplicationBarMode.Default;
                ApplicationBar.Mode = ApplicationBarMode.Default;
                ApplicationBar.Buttons.Clear();
                ApplicationBarIconButton deleteb = new ApplicationBarIconButton();
                deleteb.Text = "delete all!";
                deleteb.IconUri = new Uri("/Assets/Images/delete.png", UriKind.Relative);
                deleteb.Click += deleteb_Click;
                ApplicationBar.Buttons.Add(deleteb);
            }
            if (MainPivot.SelectedIndex == 1)
            {
                ApplicationBar.Mode = ApplicationBarMode.Default;
                ApplicationBar.Buttons.Clear();
                ApplicationBarIconButton cancelb = new ApplicationBarIconButton();
                cancelb.Text = "cancel all!";
                cancelb.IconUri = new Uri("/Assets/cancel.png", UriKind.Relative);
                cancelb.Click += ApplicationBarIconButton_Click;
                ApplicationBar.Buttons.Add(cancelb);
            }
        }

        private async void playbtn_Click(object sender, RoutedEventArgs e)
        {
            ItemViewModel myobject = (sender as Button).DataContext as ItemViewModel;

            //Get the selected ListBoxItem container instance of the item whose marks button is pressed
            ListBoxItem pressedItem = this.FileList.ItemContainerGenerator.ContainerFromItem(myobject) as ListBoxItem;

            //Checks whether it is not null 
            if (pressedItem != null)
            {
                Debug.WriteLine(myobject.Quality);
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("shared\\transfers\\" + UIHelper.GetTxtBtwn(myobject.Quality, "LocalState\\shared\\transfers\\", ".mp4", 0) + ".mp4");
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                StorageFolder fd = await ApplicationData.Current.LocalFolder.CreateFolderAsync("shared", CreationCollisionOption.OpenIfExists);
                StorageFolder fd2 = await fd.CreateFolderAsync("transfers", CreationCollisionOption.OpenIfExists);
                var files = await fd2.GetFilesAsync();
                App.ViewModel.FileItems.Clear();
                foreach (var item in files)
                {
                    string _name = item.Name.Replace(".mp4", "");
                    string _path = item.Path;
                    string _day = item.DateCreated.ToString();
                    string _filename = item.Path;
                    App.ViewModel.FileItems.Add(new ItemViewModel() { Title = _name, URL = _path, Information = _day, Quality = _filename });
                }
                FileList.ItemsSource = App.ViewModel.FileItems;
                //NavigationService.Navigate(new Uri("/PlayerPage.xaml?NAME=" + myobject.Title + "&URL=" + HttpUtility.UrlEncode(myobject.URL), UriKind.Relative));
            }
            //StorageFolder fd = await ApplicationData.Current.LocalFolder.CreateFolderAsync("shareds", CreationCollisionOption.OpenIfExists);
            // var file = fd.GetFileAsync(UIHelper.GetTxtBtwn(myobject.Quality,"LocalState\\shareds\\",".mp4",0)+".mp4");

            // C:\Data\Users\DefApps\APPDATA\Local\Packages\6294f019-a34d-422f-b6d6-35e4a326df5c_teznym40phx6y\LocalState\shareds\bigbuck (4).mp4



        }

        private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FileList.SelectedItem != null && FileList != null)
            {
                ItemViewModel Chon = (ItemViewModel)FileList.SelectedItem;
                string stream = Chon.URL;
                string name = Chon.Title;
                NavigationService.Navigate(new Uri("/PlayerPage.xaml?NAME=" + name + "&URL=" + HttpUtility.UrlEncode(stream), UriKind.Relative));

            }
            FileList.SelectedItem = null;
        }
    }
}
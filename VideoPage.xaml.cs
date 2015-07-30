using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using FreeApp.ViewModels;
using HtmlAgilityPack;
using System.Net.Http;
using FreeApp.Utils;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Windows.Controls;
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media;
using Windows.Storage;
using Windows.Networking.BackgroundTransfer;
using Microsoft.Phone.Tasks;

namespace FreeApp
{
    public partial class VideoPage : PhoneApplicationPage
    {
        private string _namePage = "";
        private string _urlPage = "";
        private string _avatar = "";
        private string _stream = "";
        private ToastPrompt toastPrompt;
        public VideoPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            WrapVirtualizationStrategyDefinition strategyDefinition = new WrapVirtualizationStrategyDefinition();
            strategyDefinition.Orientation = System.Windows.Controls.Orientation.Horizontal;
            this.MovieListBox.VirtualizationStrategyDefinition = (VirtualizationStrategyDefinition)strategyDefinition;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (NavigationContext.QueryString.TryGetValue("name", out _namePage))
                {
                    NavigationContext.QueryString.TryGetValue("url", out _urlPage);
                    NavigationContext.QueryString.TryGetValue("avatar", out _avatar);
                    this.MovieName.Text = HttpUtility.UrlDecode(this._namePage);
                    this.coverImage.ImageSource = new BitmapImage(new Uri(HttpUtility.UrlDecode(this._avatar), UriKind.RelativeOrAbsolute));
                    GetEpisodes(_urlPage);
                }
            }
            catch
            {
            }
        }
        public async void GetEpisodes(string Url)
        {
            App.ViewModel.EpisodeItems.Clear();
            App.ViewModel.RelatedItems.Clear();
            App.ViewModel.IsLoading = true;

            try
            {
                string html = "";
                using (var client = new HttpClient())
                {
                    html = await client.GetStringAsync("http://www.phimmoi.net/" + Url + "xem-phim.html");
                    this._stream = UIHelper.GetTxtBtwn(html.ToString(), "currentEpisode.url='", "';", 0);
                    string episodeJson = UIHelper.GetTxtBtwn(html, "episodeJson", "</script>", 0);
                    episodeJson = episodeJson.Replace("{\"episodeId", "\n\n{\"episodeId");
                    MatchCollection Results = Regex.Matches(episodeJson, "episodeId(.*)\n");
                    foreach (var item in Results)
                    {
                        string part = UIHelper.GetTxtBtwn(item.ToString(), "number\":", ",", 0);
                        string title = UIHelper.GetTxtBtwn(item.ToString(), "part\":", ",", 0);
                        string link = UIHelper.GetTxtBtwn(item.ToString(), "url\":\"", "\",", 0);
                        App.ViewModel.EpisodeItems.Add(new ItemViewModel() { Title = "Episode/Part " + part + "." + title, URL = HttpUtility.HtmlDecode(link) });
                    }
                    html = UIHelper.GetTxtBtwn(html, "ul class=\"list-movie\">", "<span class=\"close", 0);
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(HttpUtility.HtmlDecode(html));
                    var nodes = htmlDocument.DocumentNode.SelectNodes("//li[starts-with(@class, 'movie-item')]");
                    foreach (var div in nodes)
                    {
                        string thumbnail = div.SelectSingleNode(".//div[@class='movie-thumbnail']").Attributes["style"].Value.Replace("background:url(", "").Replace("; background-size: cover;", "");
                        string link = div.SelectSingleNode(".//a[@class='block-wrapper']").Attributes["href"].Value;
                        if (thumbnail.Contains("url="))
                        {
                            thumbnail = UIHelper.GetTxtBtwn(thumbnail, "url=", ")", 0);
                        }
                        else { thumbnail = thumbnail.Replace(")", ""); }
                        string title = div.SelectSingleNode(".//span[@class='movie-title-2']").InnerText.Trim();
                        App.ViewModel.RelatedItems.Add(new ItemViewModel() { Title = title, URL = link, IMAGE = thumbnail, ImageSource = new Uri(thumbnail, UriKind.RelativeOrAbsolute) });
                    }


                }
            }
            catch { }
            App.ViewModel.IsLoading = false;
        }

        private void ToggleRoundButtonImageClick(object sender, RoutedEventArgs e)
        {
            App.ViewModel.WriteXML(this._urlPage, this._namePage, this._avatar, this._stream);
            ToastPrompt tost = new ToastPrompt()
            {
                Title = "Success:",
                Message = "Add the video to favorites successfully."
            };
            tost.Show();
            //App.ViewModel.WriteXML(selectedUrl, selectedTitle, selectedImage, selectedInformation);
        }
        private void InitToastPrompt(string title = "Error", string message = "Failed to load the weather data.", TextWrapping wrap = (TextWrapping) 0)
        {
            if (this.toastPrompt == null)
                this.toastPrompt = new ToastPrompt();
            this.toastPrompt.Background = (Brush)Application.Current.Resources[(object)"PhoneAccentBrush"];
            this.toastPrompt.Foreground = (Brush)new SolidColorBrush(Colors.White);
            this.toastPrompt.Title = title;
            this.toastPrompt.Message = message;
            this.toastPrompt.TextWrapping = wrap;
            this.toastPrompt.Show();
        }



        private void MovieListBox_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            ItemViewModel movieCategory = this.MovieListBox.SelectedItem as ItemViewModel;
            this.MovieName.Text = HttpUtility.UrlDecode(movieCategory.Title);
            this.coverImage.ImageSource = new BitmapImage(new Uri(HttpUtility.UrlDecode(movieCategory.IMAGE), UriKind.RelativeOrAbsolute));
            GetEpisodes(movieCategory.URL);
            this.RootPivot.SelectedIndex = 0;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

        }

        private void download_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemViewModel myobject = (sender as Button).DataContext as ItemViewModel;

                //Get the selected ListBoxItem container instance of the item whose marks button is pressed
                ListBoxItem pressedItem = this.espiList.ItemContainerGenerator.ContainerFromItem(myobject) as ListBoxItem;

                //Checks whether it is not null 
                if (pressedItem != null)
                {
                    GetUrl(myobject.URL, this._namePage + " - " + myobject.Title);

                }
            }
            catch { }
        }
        private async void GetUrl(string linkf, string name)
        {

            try
            {

                StorageFile destinationFile;
                StorageFolder fd = await ApplicationData.Current.LocalFolder.CreateFolderAsync("shared", CreationCollisionOption.OpenIfExists);
                StorageFolder fd2 = await fd.CreateFolderAsync("transfers", CreationCollisionOption.OpenIfExists);
                destinationFile = await fd2.CreateFileAsync(name.Replace("(", "- ").Replace(")", "").Replace("/", "-").Replace(".", "-") + ".mp4", CreationCollisionOption.GenerateUniqueName);
                //destinationFile = await KnownFolders.MusicLibrary.CreateFileAsync("love.mp3", CreationCollisionOption.GenerateUniqueName);
                BackgroundDownloader backgroundDownloader = new BackgroundDownloader();
                DownloadOperation download = backgroundDownloader.CreateDownload(new Uri("http://www.phimmoi.net/player.php?url=" + linkf.Replace("\\/", "/"), UriKind.RelativeOrAbsolute), destinationFile);
                ToastPrompt tost = new ToastPrompt()
                {
                    Title = "Success:",
                    Message = "This file starting download."
                };
                tost.Show();
                await download.StartAsync();

            }
            catch
            {
                ToastPrompt tost = new ToastPrompt()
                {
                    Title = "Error:",
                    Message = "have a problem when start downloading, please try again!"
                };
                tost.Show();
                //Toast2.Message = "have a problem when start downloading, please try again!";
            }
        }
        private void RoundButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PlayerPage.xaml?NAME=" + HttpUtility.UrlEncode(this._namePage) + "&URL=" + HttpUtility.UrlEncode(_stream.Replace("\\/", "/")), UriKind.Relative));
        }

        private void RoundButton_Click_1(object sender, RoutedEventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = "Watch movie with Movie Free on Windows Phone";
            shareLinkTask.LinkUri = new Uri("http://windowsphone.com/s?appid=46b622c1-56a9-4635-828b-8ce622709c37", UriKind.Absolute);
            shareLinkTask.Message = "Watch movie with Movie Free on Windows Phone.\r\n\r\nYou can download application in: http://windowsphone.com/s?appid=46b622c1-56a9-4635-828b-8ce622709c37";
            shareLinkTask.Show();
        }

        private void RoundButton_Click_2(object sender, RoutedEventArgs e)
        {
            GetUrl(_stream, this._namePage);
        }

        private void espiList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (espiList.SelectedItem != null && espiList != null)
            {
                ItemViewModel Chon = (ItemViewModel)espiList.SelectedItem;
                this._stream = Chon.URL.Replace("\\/", "/");
                NavigationService.Navigate(new Uri("/PlayerPage.xaml?NAME=" + HttpUtility.UrlEncode(Chon.Title) + "&URL=" + HttpUtility.UrlEncode(Chon.URL.Replace("\\/", "/")), UriKind.Relative));
            }
            // espiList.SelectedItem = null;
        }

    }

    public class RootObject
    {
        public List<Espidode> data { get; set; }
    }
    public class Espidode
    {
        public int episodeId { get; set; }
        public string url { get; set; }
        public int number { get; set; }
        public int part { get; set; }
        public int displayOrder { get; set; }
        public string serverId { get; set; }
        public string subUrl { get; set; }
        public string vttUrl { get; set; }
        public string playTech { get; set; }
    }

}
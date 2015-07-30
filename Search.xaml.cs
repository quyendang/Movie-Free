using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using HtmlAgilityPack;
using FreeApp.ViewModels;

namespace FreeApp
{
    public partial class Search : PhoneApplicationPage
    {
        private string Key;
        public Search()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (NavigationContext.QueryString.TryGetValue("Key", out Key))
                {
                    ketTxt.Text = Key;
                    this.SearchF(Key);
                }
            }
            catch
            {
            }
        }
        public async void SearchF(string k)
        {
            App.ViewModel.Directors.Clear();
            App.ViewModel.IsLoading = true;
            try
            {
                string html = "";
                var client = new HttpClient();
                html = await client.GetStringAsync(string.Format("http://www.phimmoi.net/tim-kiem/{0}/", k));
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(HttpUtility.HtmlDecode(html));
                var nodes = htmlDocument.DocumentNode.SelectNodes("//li[starts-with(@class, 'movie-item')]");
                foreach (var div in nodes)
                {
                    string lenght = div.SelectSingleNode(".//span[@class='movie-title-chap']").InnerText.Trim();
                    string thumbnail = div.SelectSingleNode(".//div[@class='movie-thumbnail']").Attributes["style"].Value.Replace("background:url(", "").Replace("); background-size: cover;", "");
                    string link = div.SelectSingleNode(".//a[@class='block-wrapper']").Attributes["href"].Value;
                    string title = div.SelectSingleNode(".//span[@class='movie-title-2']").InnerText.Trim();
                    string quality;
                    //Debug.WriteLine(div.OuterHtml);
                    try
                    {
                        quality = div.SelectSingleNode(".//span[@class='ribbon']").InnerText.Trim();
                    }
                    catch
                    {
                        quality = "HD";
                    }
                    App.ViewModel.Directors.Add(new ItemViewModel() { Title = title, URL = link, ImageSource = new Uri(thumbnail, UriKind.RelativeOrAbsolute), Information = App.ViewModel.FilterLenght(lenght), IMAGE = thumbnail, Quality = App.ViewModel.Filterquality(quality) });

                }
            }
            catch
            {
                App.ViewModel.Mess = "please check internet connection!";
            }
            // MessageBox.Show(Items.Count.ToString());
            App.ViewModel.IsLoading = false;
        }

        private void MovieListBox_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            ItemViewModel movieCategory = this.MovieListBox.SelectedItem as ItemViewModel;
            NavigationService.Navigate(new Uri("/VideoPage.xaml?name=" + HttpUtility.UrlEncode(movieCategory.Title) + "&url=" + HttpUtility.UrlEncode(movieCategory.URL) + "&avatar=" + HttpUtility.UrlEncode(movieCategory.ImageSource.ToString()), UriKind.Relative));
        }
    }
}
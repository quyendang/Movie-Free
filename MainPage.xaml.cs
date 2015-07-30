using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using System.Collections;
using FreeApp.ViewModels;
using System.Windows.Controls.Primitives;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Tasks;
using System.ComponentModel;
using Microsoft.Devices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net.Http;
using System.Text.RegularExpressions;
using FreeApp.Utils;

namespace FreeApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 240, 248, 255));
        private readonly SolidColorBrush _naturalBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 135, 189));
        private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 100, 149, 237));
        private List<Ads> listAds = new List<Ads>();
        public int intPage = 0;
        public string stream;
        public string url = "phim-chieu-rap/";
        public int _pageNumber = 1;
        private string Ca = "us";
        private string Ca2 = "us";
        private int flag = 0;
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            WrapVirtualizationStrategyDefinition strategyDefinition = new WrapVirtualizationStrategyDefinition();
            strategyDefinition.Orientation = System.Windows.Controls.Orientation.Horizontal;
            this.MovieListBox.VirtualizationStrategyDefinition = (VirtualizationStrategyDefinition)strategyDefinition;
            this.TVListBox.VirtualizationStrategyDefinition = (VirtualizationStrategyDefinition)strategyDefinition;
            this.listparkCountryCategories.ItemsSource = (IEnumerable)App.ViewModel.ReleasedCountry;
            this.listparkCountryCategories2.ItemsSource = (IEnumerable)App.ViewModel.ReleasedCountry;
            this.LstBxYear.Visibility = this.TglBtnYearSections.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.LstBxCategories.Visibility = this.TglBtnNationals.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.GenreListBox.Visibility = this.TglBtnGenres.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.LstBxCategories.ItemsSource = (IEnumerable)App.ViewModel.ReleasedCountry;
            this.LstBxYear.ItemsSource = (IEnumerable)App.ViewModel.ReleasedYear;
            this.GenreListBox.ItemsSource = (IEnumerable)App.ViewModel.MovieCategorys;
            App.sk.connectSocket();

        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            base.OnBackKeyPress(e);
            this.flag = this.flag + 1;
            if (this.flag < (this.listAds.Count + 1) && this.listAds.Count != 0)
            {
                var toast = new ToastPrompt
                {
                    IsAppBarVisible = false,
                    Background = _aliceBlueSolidColorBrush,
                    Foreground = _cornFlowerBlueSolidColorBrush,
                    Title = this.listAds[this.flag - 1].Title,
                    Message = this.listAds[this.flag - 1].Message,
                    FontSize = 20,
                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
                    ImageSource =
                        new BitmapImage(new Uri(this.listAds[this.flag - 1].Image, UriKind.RelativeOrAbsolute))
                };
                toast.MillisecondsUntilHidden = 7000;
                toast.Tap += toasts_Tap;
                toast.Show();
                var t = new ToastPrompt
                {
                    Title = "warning",
                    Message = string.Format("Press BACK {0} times again to Exit!", (this.listAds.Count + 1 - this.flag)),
                };
                t.Show();
                e.Cancel = true;
            }
            else
            {
            }

        }
        public async void LoadAds()
        {
            try
            {
                this.listAds.Clear();
                string html = "";
                var client = new HttpClient();
                html = await client.GetStringAsync("http://lucstudio.cloudapp.net/Ads.xml");
                MatchCollection Resultss = Regex.Matches(html.ToString(), "<App>(.*)");
                foreach (Match result in Resultss)
                {
                    string _url = UIHelper.GetTxtBtwn(result.ToString(), "<url>", "</url>", 0);
                    string _name = UIHelper.GetTxtBtwn(result.ToString(), "<name>", "</name>", 0);
                    string _image = UIHelper.GetTxtBtwn(result.ToString(), "<image>", "</image>", 0);
                    string _version = UIHelper.GetTxtBtwn(result.ToString(), "<message>", "</message", 0);
                    this.listAds.Add(new Ads() { Image = _image, Message = _version, Title = _name, Url = _url });
                }
            }
            catch
            { }
        }
        private void toasts_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri(this.listAds[this.flag - 1].Url, UriKind.Absolute);

            webBrowserTask.Show();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            try
            {
                App.ViewModel.LoadData();
                App.ViewModel.Mess = "welcome back!";
                App.ViewModel.LoadFilmHot(this.Ca);
                App.ViewModel.LoadTVHot(this.Ca2);
                this.LoadAds();
            }
            catch { }
            //App.ViewModel.LoadXML();
        }
        private void hotClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Movie.xaml?name=Cinema Movies&type=&url=phim-chieu-rap/", UriKind.Relative));
        }

        private void tv_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Movie.xaml?name=TV Series&type=&url=phim-bo/", UriKind.Relative));
        }

        private void favoriteBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Fa.xaml", UriKind.Relative));
        }

        private void history_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Chat.xaml", UriKind.Relative));
        }

        private void down_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BackgroundTransferList.xaml?select=1", UriKind.Relative));
        }

        private void downloadPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BackgroundTransferList.xaml?select=0", UriKind.Relative));
        }

        private void MovieListBox_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            ItemViewModel movieCategory = this.MovieListBox.SelectedItem as ItemViewModel;
            NavigationService.Navigate(new Uri("/VideoPage.xaml?name=" + HttpUtility.UrlEncode(movieCategory.Title) + "&url=" + HttpUtility.UrlEncode(movieCategory.URL) + "&avatar=" + HttpUtility.UrlEncode(movieCategory.ImageSource.ToString()), UriKind.Relative));
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Movie.xaml?name=Cinema Movies&type=&url=phim-chieu-rap/", UriKind.Relative));
        }

        private void listparkCountryCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieCategory movieCategory = this.listparkCountryCategories.SelectedItem as MovieCategory;
            if (!(movieCategory.Url != this.Ca))
                return;
            this.Ca = movieCategory.Url;
            App.ViewModel.LoadFilmHot(this.Ca);
        }

        private void listparkCountryCategories2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieCategory movieCategory = this.listparkCountryCategories2.SelectedItem as MovieCategory;
            if (!(movieCategory.Url != this.Ca))
                return;
            this.Ca = movieCategory.Url;
            App.ViewModel.LoadTVHot(this.Ca);
        }

        private void TVListBox_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            ItemViewModel movieCategory = this.TVListBox.SelectedItem as ItemViewModel;
            NavigationService.Navigate(new Uri("/VideoPage.xaml?name=" + HttpUtility.UrlEncode(movieCategory.Title) + "&url=" + HttpUtility.UrlEncode(movieCategory.URL) + "&avatar=" + HttpUtility.UrlEncode(movieCategory.ImageSource.ToString()), UriKind.Relative));
        }

        private void AppBarPin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LstBxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieCategory movieCategory = this.LstBxYear.SelectedItem as MovieCategory;
            NavigationService.Navigate(new Uri("/Movie.xaml?name=" + movieCategory.Title + "&type=year&url=" + movieCategory.Url, UriKind.Relative));
        }

        private void GenreListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieCategory movieCategory = this.GenreListBox.SelectedItem as MovieCategory;
            NavigationService.Navigate(new Uri("/Movie.xaml?name=" + movieCategory.Title + "&type=genres&url=" + movieCategory.Url, UriKind.Relative));
        }

        private void LstBxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieCategory movieCategory = this.LstBxCategories.SelectedItem as MovieCategory;
            NavigationService.Navigate(new Uri("/Movie.xaml?name=" + movieCategory.Title + "&type=national&url=" + movieCategory.Url, UriKind.Relative));
        }
        public void SetWhenClick(ToggleButton t1, ToggleButton t2)
        {
            t2.IsChecked = false;
            t1.IsChecked = false;
        }
        private void TglBtnGenres_Click(object sender, RoutedEventArgs e)
        {
            this.GenreListBox.Visibility = this.TglBtnGenres.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.LstBxCategories.Visibility = Visibility.Collapsed;
            this.LstBxYear.Visibility = System.Windows.Visibility.Collapsed;
            SetWhenClick(TglBtnNationals, TglBtnNationals);
        }

        private void TglBtnYearSections_Click(object sender, RoutedEventArgs e)
        {
            this.LstBxYear.Visibility = this.TglBtnYearSections.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.LstBxCategories.Visibility = Visibility.Collapsed;
            this.GenreListBox.Visibility = System.Windows.Visibility.Collapsed;
            SetWhenClick(TglBtnNationals, TglBtnGenres);
        }

        private void TglBtnNationals_Click(object sender, RoutedEventArgs e)
        {
            this.LstBxCategories.Visibility = this.TglBtnNationals.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.GenreListBox.Visibility = Visibility.Collapsed;
            this.LstBxYear.Visibility = System.Windows.Visibility.Collapsed;
            SetWhenClick(TglBtnGenres, TglBtnYearSections);
        }

        private void ScrlVwrNav_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Movie.xaml?name=TV Series&type=&url=phim-bo/", UriKind.Relative));
        }

        

        private void Search_Click(object sender, EventArgs e)
        {
            var input = new InputPrompt
            {
                Title = "Search Input",
                Message = "Enter a key:",
            };

            input.Completed += PopUpPromptStringCompleted;

            input.Show();
        }
        void PopUpPromptStringCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            NavigationService.Navigate(new Uri("/Search.xaml?Key=" + e.Result, UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "[Movie Free] I have an issue!";
            emailComposeTask.Body = "Please enter your problem!";
            emailComposeTask.To = "x@banh.us";

            emailComposeTask.Show();
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            MarketplaceReviewTask rr = new MarketplaceReviewTask();
            rr.Show();
        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = "Watch movie with Movie Free on Windows Phone";
            shareLinkTask.LinkUri = new Uri("http://windowsphone.com/s?appid=46b622c1-56a9-4635-828b-8ce622709c37", UriKind.Absolute);
            shareLinkTask.Message = "Watch movie with Movie Free on Windows Phone.\r\n\r\nYou can download application in: http://windowsphone.com/s?appid=46b622c1-56a9-4635-828b-8ce622709c37";
            shareLinkTask.Show();
        }

        private void ApplicationBarMenuItem_Click_3(object sender, EventArgs e)
        {
            var about = new AboutPrompt();
            about.Completed += PopUpPromptObjectCompleted;

            about.Show("Vl Studio.", "Sorry I dont use ! ", "x@banh.us", "http://lucstudio.com.com");
        }

        private void PopUpPromptObjectCompleted(object sender, PopUpEventArgs<object, PopUpResult> e)
        {
            return;
        }

    }
    public class Ads
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
    }
}
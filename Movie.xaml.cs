using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections;
using FreeApp.ViewModels;
using Telerik.Windows.Controls;
using Microsoft.Phone.Tasks;
using FreeApp.Utils;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace FreeApp
{
    public partial class Movie : PhoneApplicationPage
    {
        private string _namePage = "";
        private string _urlPage = "";
        private int _pageNum = 1;
        private string type = "";
        private string ca = "us";
        public Movie()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            WrapVirtualizationStrategyDefinition strategyDefinition = new WrapVirtualizationStrategyDefinition();
            strategyDefinition.Orientation = System.Windows.Controls.Orientation.Horizontal;
            this.MovieListBox.VirtualizationStrategyDefinition = (VirtualizationStrategyDefinition)strategyDefinition;
            this.listparkCountryCategories2.ItemsSource = (IEnumerable)App.ViewModel.ReleasedCountry;
        }
        public void startanimation()
        {
            // MessageBox.Show("i");
            Storyboard sbFadeIn = new Storyboard();
            sbFadeIn.Completed += new EventHandler(sb_Completed);
            FadeInOut(LayoutRoot.Background, sbFadeIn, true);
        }
        public void FadeInOut(DependencyObject target, Storyboard sb, bool isFadeIn)
        {
            Duration d = new Duration(TimeSpan.FromMilliseconds(2000));
            DoubleAnimation daFade = new DoubleAnimation();
            daFade.Duration = d;
            if (isFadeIn)
            {
                daFade.To = 0.4;
            }
            else
            {
                daFade.From = 0.1;
                daFade.To = 0.4;
            }

            sb.Duration = d;
            sb.Children.Add(daFade);
            Storyboard.SetTarget(daFade, target);
            Storyboard.SetTargetProperty(daFade, new PropertyPath("Opacity"));
            sb.Begin();
        }
        private void sb_Completed(object sender, EventArgs e)
        {
            BitmapImage bitmapImage = App.ViewModel.BackGround;
            ImageBrush imageBrush = new ImageBrush()
            {
                Opacity = 0.3,
                ImageSource = bitmapImage,
                Stretch = Stretch.UniformToFill
            };

            LayoutRoot.Background = imageBrush;
            // RootPanorama.Background.Opacity = 0.4d;
            Storyboard sbFadeOut = new Storyboard();
            FadeInOut(LayoutRoot.Background, sbFadeOut, false);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                //startanimation();
                if (NavigationContext.QueryString.TryGetValue("name", out _namePage))
                {
                    NavigationContext.QueryString.TryGetValue("url", out _urlPage);
                    NavigationContext.QueryString.TryGetValue("type", out type);
                    this.pageName.Text = this._namePage;
                    if (type == "year")
                    {
                        listparkCountryCategories2.Visibility = System.Windows.Visibility.Collapsed;
                        App.ViewModel.LoadMovie(this._urlPage, _pageNum, null, type);
                    }
                    if (type == "national")
                    {
                        App.ViewModel.LoadMovie(this._urlPage, _pageNum, this._urlPage, type);
                    }
                    else
                    {
                        App.ViewModel.LoadMovie(this._urlPage, _pageNum, this.ca, "");
                    }

                }
                try
                {
                    if (!IsolatedStorageHelper.GetPrimitive<bool>("ReminderReview") && !App.NeedReview)
                    {
                        CheckBox checkBox = new CheckBox()
                        {
                            Content = "Do not ask me again",
                            Margin = new Thickness(0, 14, 0, -2)
                        };

                        Microsoft.Phone.Controls.TiltEffect.SetIsTiltEnabled(checkBox, true);


                        //Create a new custom message box
                        CustomMessageBox messageBox = new CustomMessageBox()
                        {
                            Caption = "Rate and Review",
                            Message = "Thank you for using this application.\nWould you like to give some to rate and review this application. \n(Review to help Developer!)",
                            Content = checkBox,
                            LeftButtonContent = "ok",
                            RightButtonContent = "cancel",
                            IsFullScreen = false
                        };

                        //Define the dismissed event handler
                        messageBox.Dismissed += (s1, e1) =>
                        {
                            switch (e1.Result)
                            {
                                case CustomMessageBoxResult.LeftButton:
                                    new MarketplaceReviewTask().Show();
                                    break;
                                case CustomMessageBoxResult.RightButton:
                                case CustomMessageBoxResult.None:
                                    if ((bool)checkBox.IsChecked)
                                    {
                                        IsolatedStorageHelper.SavePrimitive<bool>("ReminderReview", true);
                                    }
                                    else
                                    {

                                    }
                                    break;
                                default:
                                    break;
                            }
                        };

                        messageBox.Show();
                    }
                }
                catch
                { }
            }
            catch
            {
            }
        }
        private void MovieListBox_DataRequested(object sender, EventArgs e)
        {
            if (App.ViewModel.Items == null || App.ViewModel.Items.Count <= 0 || this._pageNum <= 0)
                return;
            else
            {
                this._pageNum++;
                App.ViewModel.LoadMovie(this._urlPage, this._pageNum, this.ca, type);
            }
        }
        private void listparkCountryCategories2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieCategory movieCategory = this.listparkCountryCategories2.SelectedItem as MovieCategory;
            if (!(movieCategory.Url != this.ca))
                return;
            this.ca = movieCategory.Url;
            this._pageNum = 1;
            App.ViewModel.LoadMovie(this._urlPage, this._pageNum, this.ca, type);
        }

        private void MovieListBox_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            ItemViewModel movieCategory = this.MovieListBox.SelectedItem as ItemViewModel;
            NavigationService.Navigate(new Uri("/VideoPage.xaml?name=" + HttpUtility.UrlEncode(movieCategory.Title) + "&url=" + HttpUtility.UrlEncode(movieCategory.URL) + "&avatar=" + HttpUtility.UrlEncode(movieCategory.ImageSource.ToString()), UriKind.Relative));
        }
    }
}
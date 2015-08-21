using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml;
using System.Diagnostics;
using FreeApp.ViewModels;
using FreeApp.Utils;
using System.Net.Http;
using FreeApp.Resources;
using System.Windows.Threading;
using HtmlAgilityPack;
using System.Net;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FreeApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.transfers = new ObservableCollection<TransferModel>();
            this.ChatItems = new ObservableCollection<ChatItem>();
            this.HotItems = new ObservableCollection<ItemViewModel>();
            this.TVItems = new ObservableCollection<ItemViewModel>();
            this.Years = new ObservableCollection<ItemViewModel>();
            this.Directors = new ObservableCollection<ItemViewModel>();
            this.Nationals = new ObservableCollection<ItemViewModel>();
            this.Genres = new ObservableCollection<ItemViewModel>();
            this.HomeItems = new ObservableCollection<ItemViewModel>();
            this.RelatedItems = new ObservableCollection<ItemViewModel>();
            this.EpisodeItems = new ObservableCollection<ItemViewModel>();
            this.SuggestItems = new ObservableCollection<ItemViewModel>();
            this.Like = new ObservableCollection<ItemViewModel>();
            this.QualityItems = new ObservableCollection<ItemViewModel>();
            this.ActorItems = new ObservableCollection<ItemViewModel>();
            this.FileItems = new ObservableCollection<ItemViewModel>();
            IsLoading = true;
            this.IsMovie = true;
            Mess = "welcome back";


        }
        private List<MovieCategory> _releasedCountry;
        private List<MovieCategory> _movieCategorys;
        private List<MovieCategory> _releasedYear;
        public List<MovieCategory> ReleasedYear
        {
            get
            {
                if (this._releasedYear == null)
                    this._releasedYear = this.CreateReleasedYear();
                return this._releasedYear;
            }
        }
        public List<MovieCategory> MovieCategorys
        {
            get
            {
                if (this._movieCategorys == null)
                    this._movieCategorys = this.createCategorys();
                return this._movieCategorys;
            }
        }
        public List<MovieCategory> ReleasedCountry
        {
            get
            {
                if (this._releasedCountry == null)
                    this._releasedCountry = this.CreateReleasedCountry();
                return this._releasedCountry;
            }
        }
        private List<MovieCategory> CreateReleasedCountry()
        {
            List<MovieCategory> list = new List<MovieCategory>();
            MovieCategory movieCategory1 = new MovieCategory()
            {
                RootCateory = "Country",
                Title = "US",
                Url = "us"
            };
            list.Add(movieCategory1);
            MovieCategory movieCategory2 = new MovieCategory()
            {
                RootCateory = "Country",
                Title = "UK",
                Url = "uk"
            };
            list.Add(movieCategory2);
            MovieCategory movieCategory3 = new MovieCategory()
            {
                RootCateory = "Country",
                Title = "Canada",
                Url = "ca"
            };
            list.Add(movieCategory3);
            MovieCategory movieCategory4 = new MovieCategory()
            {
                RootCateory = "Country",
                Title = "Australia",
                Url = "au"
            };
            list.Add(movieCategory4);
            MovieCategory movieCategory5 = new MovieCategory()
            {
                RootCateory = "Country",
                Title = "India",
                Url = "in"
            };
            list.Add(movieCategory5);
            return list;
        }
        private List<MovieCategory> createCategorys()
        {
            List<MovieCategory> list = new List<MovieCategory>();
            MovieCategory movieCategory1 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Action",
                Url = "the-loai/phim-hanh-dong/"
            };
            list.Add(movieCategory1);
            MovieCategory movieCategory2 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Adventure",
                Url = "the-loai/phim-phieu-luu/"
            };
            list.Add(movieCategory2);
            MovieCategory movieCategory3 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Animation",
                Url = "the-loai/phim-hoat-hinh/"
            };
            list.Add(movieCategory3);
            MovieCategory movieCategory4 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Biography",
                Url = "the-loai/phim-vien-tuong/"
            };
            list.Add(movieCategory4);
            MovieCategory movieCategory5 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Comedy",
                Url = "the-loai/phim-hai-huoc/"
            };
            list.Add(movieCategory5);
            MovieCategory movieCategory6 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Crime",
                Url = "the-loai/phim-hinh-su/"
            };
            list.Add(movieCategory6);
            MovieCategory movieCategory7 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Documentary",
                Url = "the-loai/phim-tai-lieu/"
            };
            list.Add(movieCategory7);
            MovieCategory movieCategory8 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Family",
                Url = "the-loai/phim-gia-dinh/"
            };
            list.Add(movieCategory8);
            MovieCategory movieCategory9 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Horror",
                Url = "the-loai/phim-kinh-di/"
            };
            list.Add(movieCategory9);
            MovieCategory movieCategory10 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Kids",
                Url = "the-loai/phim-hoat-hinh/"
            };
            list.Add(movieCategory10);
            MovieCategory movieCategory11 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Mystery",
                Url = "the-loai/phim-than-thoai/"
            };
            list.Add(movieCategory11);
            MovieCategory movieCategory12 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "Sport & Music",
                Url = "the-loai/phim-the-thao-am-nhac/"
            };
            list.Add(movieCategory12);
            MovieCategory movieCategory13 = new MovieCategory()
            {
                RootCateory = "GENRE",
                Title = "War",
                Url = "the-loai/phim-chien-tranh/"
            };
            list.Add(movieCategory13);
            return list;
        }
        private List<MovieCategory> CreateReleasedYear()
        {
            List<MovieCategory> list = new List<MovieCategory>();
            int num1 = 2015;
            for (int index = 0; index < 5; ++index)
            {
                int num2 = num1 - index;
                MovieCategory movieCategory2 = new MovieCategory()
                {
                    RootCateory = "Released Year",
                    Title = num2.ToString(),
                    Url = num2.ToString()
                };
                list.Add(movieCategory2);
            }
            return list;
        }

        public ObservableCollection<TransferModel> transfers { get; set; }
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        public ObservableCollection<ChatItem> ChatItems { get; private set; }
        public ObservableCollection<ItemViewModel> HotItems { get; private set; }
        public ObservableCollection<ItemViewModel> TVItems { get; private set; }
        public ObservableCollection<ItemViewModel> Years { get; private set; }
        public ObservableCollection<ItemViewModel> Genres { get; private set; }
        public ObservableCollection<ItemViewModel> Directors { get; private set; }
        public ObservableCollection<ItemViewModel> Nationals { get; private set; }
        public ObservableCollection<ItemViewModel> HomeItems { get; private set; }
        public ObservableCollection<ItemViewModel> RelatedItems { get; private set; }
        public ObservableCollection<ItemViewModel> EpisodeItems { get; private set; }
        public ObservableCollection<ItemViewModel> SuggestItems { get; private set; }
        public ObservableCollection<ItemViewModel> QualityItems { get; private set; }
        public ObservableCollection<ItemViewModel> ActorItems { get; private set; }
        public ObservableCollection<ItemViewModel> Like { get; private set; }

        public ObservableCollection<ItemViewModel> FileItems { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        public string _mess = "welcome back!";
        private bool _isMovie = false;
        public string Messagetxt
        { get; set; }
        public string Linkgoto
        { get; set; }
        public string Titlelink
        { get; set; }
        public string Mess
        {
            get { return _mess; }
            set { _mess = value; NotifyPropertyChanged("Mess"); }
        }
        public void ReAllWriteXML()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("FAVO.xml", FileMode.Create, myIsolatedStorage))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        using (XmlWriter write = XmlWriter.Create(isoStream, settings))
                        {
                            write.WriteStartElement("p", "film", "urn:film");
                            write.WriteEndDocument();
                            write.Flush();
                        }
                    }
                }
                App.ViewModel.Like.Add(new ItemViewModel() { });
            }
            catch { }
        }

        public SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            byte R = Convert.ToByte(hexaColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexaColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexaColor.Substring(5, 2), 16);
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(0xFF, R, G, B));
            return scb;
        }
        public bool IsMovie
        {
            get
            {
                return _isMovie;
            }
            set
            {
                _isMovie = value;
                NotifyPropertyChanged("IsMovie");
            }
        }
        public void LoadRelated(string Content)
        {
            this.RelatedItems.Clear();
            try
            {
                MatchCollection Results = Regex.Matches(Content, "<related>(.*)");
                foreach (Match result in Results)
                {
                    string name = UIHelper.GetTxtBtwn(result.ToString(), "<title><![CDATA[", "]", 0).Replace("&#8211;", "").Replace("&#038;", "").Replace("&#8217;", "").Replace("amp;", "").Trim();
                    string image = UIHelper.GetTxtBtwn(result.ToString(), "<image><![CDATA[", "]", 0);
                    string _url = UIHelper.GetTxtBtwn(result.ToString(), "<link><![CDATA[", "]", 0);
                    string _quality = "unknown";
                    App.ViewModel.RelatedItems.Add(new ItemViewModel() { Title = name, URL = _url, ImageSource = new Uri(image, UriKind.RelativeOrAbsolute), Information = "unknown", Quality = _quality, IMAGE = image });
                }
            }
            catch { }
        }
        public void LoadEPI(string Content)
        {
            this.EpisodeItems.Clear();
            try
            {

                MatchCollection Resultss = Regex.Matches(Content, "<episode>(.*)");
                foreach (Match result in Resultss)
                {
                    string _url = UIHelper.GetTxtBtwn(result.ToString(), "<url><![CDATA[", "]", 0);
                    string _quality = UIHelper.GetTxtBtwn(result.ToString(), "<epis><![CDATA[", "]", 0).Replace("&#8211;", "").Replace("&#038;", "").Replace("&#8217;", "").Replace("amp;", "").Trim();
                    string _image = UIHelper.GetTxtBtwn(Content, "<image><![CDATA[", "]", 0);
                    if (_url == "" || _url == " " || _url == null || _quality == null || _quality == "" || _quality == " ")
                    { }
                    else
                    {
                        App.ViewModel.EpisodeItems.Add(new ItemViewModel() { URL = _url, Information = _quality, ImageSource = new Uri(_image, UriKind.RelativeOrAbsolute), IMAGE = _image, Key = "movie" });
                    }
                }
            }
            catch { }
        }
        public async void LoadFilm(string URL, int page)
        {
            if (page == 1)
            {
                this.Items.Clear();
            }
            else
            {
            }
            IsLoading = true;
            try
            {
                string html = "";
                using (var client = new HttpClient())
                {
                    html = await client.GetStringAsync(string.Format("{0}page-{1}.html", URL.Trim(), page));

                }
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(HttpUtility.HtmlDecode(html));
                var nodes = htmlDocument.DocumentNode.SelectNodes("//li[starts-with(@class, 'movie-item')]");
                foreach (var div in nodes)
                {
                    // MessageBox.Show(div.OuterHtml);
                    string lenght = div.SelectSingleNode(".//span[@class='movie-title-chap']").InnerText.Trim();
                    string thumbnail = div.SelectSingleNode(".//div[@class='movie-thumbnail']").Attributes["style"].Value.Replace("background:url(", "").Replace("); background-size: cover;", "");
                    string link = div.SelectSingleNode(".//a[@class='block-wrapper']").Attributes["href"].Value;
                    string title = div.SelectSingleNode(".//span[@class='movie-title-2']").InnerText.Trim();
                    string quality;
                    Debug.WriteLine(div.OuterHtml);
                    try
                    {
                        quality = div.SelectSingleNode(".//span[@class='ribbon']").InnerText.Trim();
                    }
                    catch
                    {
                        quality = "HD";
                    }

                    this.Items.Add(new ItemViewModel() { Title = title, URL = link, ImageSource = new Uri(thumbnail, UriKind.RelativeOrAbsolute), Information = FilterLenght(lenght), IMAGE = thumbnail, Quality = Filterquality(quality) });
                }
            }
            catch
            {
                App.ViewModel.Mess = "please check internet connection!";
            }
            MessageBox.Show(Items.Count.ToString());
            this.IsLoading = false;
            this.IsDataLoaded = true;
        }
        public async void LoadMovie(string URL, int page, string ca, string type)
        {
            if (page == 1)
            {
                this.Items.Clear();
            }
            else
            {
            }
            IsLoading = true;
            try
            {
                string html = "";
                var client = new HttpClient();
                if (type == "national")
                {
                    html = await client.GetStringAsync(string.Format("http://phimmoi.net/quoc-gia/{0}/page-{1}.html", ca, page));
                }
                else
                {
                    if (type == "year")
                    {
                        html = await client.GetStringAsync(string.Format("http://www.phimmoi.net/phim-{0}/page-{1}.html", URL, page));
                    }
                    else
                    {
                        html = await client.GetStringAsync(string.Format("http://www.phimmoi.net/{0}{1}/page-{2}.html", URL, ca, page));

                    }
                }

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(HttpUtility.HtmlDecode(html));
                var nodes = htmlDocument.DocumentNode.SelectNodes("//li[starts-with(@class, 'movie-item')]");
                foreach (var div in nodes)
                {
                    // MessageBox.Show(div.OuterHtml);
                    string lenght = div.SelectSingleNode(".//span[@class='movie-title-chap']").InnerText.Trim();
                    string thumbnail = div.SelectSingleNode(".//div[@class='movie-thumbnail']").Attributes["style"].Value.Replace("background:url(", "").Replace("); background-size: cover;", "");
                    string link = div.SelectSingleNode(".//a[@class='block-wrapper']").Attributes["href"].Value;
                    string title = div.SelectSingleNode(".//span[@class='movie-title-2']").InnerText.Trim();
                    string quality;
                    Debug.WriteLine(div.OuterHtml);
                    try
                    {
                        quality = div.SelectSingleNode(".//span[@class='ribbon']").InnerText.Trim();
                    }
                    catch
                    {
                        quality = "HD";
                    }

                    this.Items.Add(new ItemViewModel() { Title = title, URL = link, ImageSource = new Uri(thumbnail, UriKind.RelativeOrAbsolute), Information = FilterLenght(lenght), IMAGE = thumbnail, Quality = Filterquality(quality) });
                }
            }
            catch
            {
                App.ViewModel.Mess = "please check internet connection!";
            }
            //MessageBox.Show(Items.Count.ToString());
            this.IsLoading = false;
            this.IsDataLoaded = true;
        }
        public async void LoadFilmHot(string Ca)
        {
            this.HotItems.Clear();
            IsLoading = true;
            try
            {
                string html = "";
                var client = new HttpClient();
                html = await client.GetStringAsync(string.Format("http://phimmoi.net/phim-chieu-rap/{0}/", Ca));
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
                    Debug.WriteLine(div.OuterHtml);
                    try
                    {
                        quality = div.SelectSingleNode(".//span[@class='ribbon']").InnerText.Trim();
                    }
                    catch
                    {
                        quality = "HD";
                    }
                    if (this.HotItems.Count < 16)
                    {
                        this.HotItems.Add(new ItemViewModel() { Title = title, URL = link, ImageSource = new Uri(thumbnail, UriKind.RelativeOrAbsolute), Information = FilterLenght(lenght), IMAGE = thumbnail, Quality = Filterquality(quality) });
                    }

                }
                
            }
            catch
            {
                App.ViewModel.Mess = "please check internet connection!";
            }
            // MessageBox.Show(Items.Count.ToString());
            //this.IsLoading = false;
            this.IsDataLoaded = true;
        }
        public async void LoadTVHot(string Ca)
        {
            this.TVItems.Clear();
            //IsLoading = true;
            try
            {
                string html = "";
                using (var client = new HttpClient())
                {
                    html = await client.GetStringAsync(string.Format("http://phimmoi.net/phim-bo/{0}/", Ca));

                }
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(HttpUtility.HtmlDecode(html));
                var nodes = htmlDocument.DocumentNode.SelectNodes("//li[starts-with(@class, 'movie-item')]");
                foreach (var div in nodes)
                {
                    IsolatedStorageHelper.SavePrimitive<string>("LastMovie", (HotItems.Count == 0) ? div.SelectSingleNode(".//span[@class='movie-title-2']").InnerText.Trim() : IsolatedStorageHelper.GetPrimitive<string>("LastMovie"));
                    string lenght = div.SelectSingleNode(".//span[@class='movie-title-chap']").InnerText.Trim();
                    string thumbnail = div.SelectSingleNode(".//div[@class='movie-thumbnail']").Attributes["style"].Value.Replace("background:url(", "").Replace("); background-size: cover;", "");
                    string link = div.SelectSingleNode(".//a[@class='block-wrapper']").Attributes["href"].Value;
                    string title = div.SelectSingleNode(".//span[@class='movie-title-2']").InnerText.Trim();
                    string quality;
                    Debug.WriteLine(div.OuterHtml);
                    try
                    {
                        quality = div.SelectSingleNode(".//span[@class='ribbon']").InnerText.Trim();
                    }
                    catch
                    {
                        quality = "HD";
                    }
                    if (this.TVItems.Count < 12)
                    {
                        this.TVItems.Add(new ItemViewModel() { Title = title, URL = link, ImageSource = new Uri(thumbnail, UriKind.RelativeOrAbsolute), Information = FilterLenght(lenght), IMAGE = thumbnail, Quality = Filterquality(quality) });
                    }

                }
            }
            catch
            {
                App.ViewModel.Mess = "please check internet connection!";
            }
            this.IsLoading = false;
            this.IsDataLoaded = true;
        }
        public string Filterquality(string quality)
        {
            quality = quality.Replace("Bản đẹp", "HD");
            quality = quality.Replace("Tập", "Esp");
            string[] arr2 = { "Vietsub", "Thuyết minh", "Lồng tiếng", "Bản chuẩn", "Không cắt", "+", "-" };
            foreach (var item in arr2)
            {
                quality = quality.Replace(item, "");
            }
            return quality.Trim();
        }
        public string FilterLenght(string lenght)
        {
            lenght = lenght.Replace("phút/tập", "m/esp");
            lenght = lenght.Replace("phút", "m");
            return lenght;
        }
        public async void LoadInfo(string name, string url, string image)
        {


            IsLoading = true;
            this.RelatedItems.Clear();
            this.EpisodeItems.Clear();
            string html = "";
            using (var client = new HttpClient())
            {
                html = await client.GetStringAsync(url);

            }
            if (html.Contains("<ul class=\"movie-parts\">"))
            {
                this.IsMovie = true;
                /// Neu La TV Shows
                /// 
                //MessageBox.Show("a");
                var tap = UIHelper.GetTxtBtwn(html, "<ul class=\"movie-parts\">", "</ul>", 0);
                MatchCollection Resultss = Regex.Matches(tap, "<li>(.*)");
                foreach (Match result in Resultss)
                {
                    string _url = UIHelper.GetTxtBtwn(result.ToString(), "href=\"", "\">", 0);
                    string _quality = "Episode " + UIHelper.GetTxtBtwn(result.ToString(), "\">", "</a>", 0).Replace("&#8211;", "").Replace("&#038;", "").Replace("&#8217;", "").Trim();
                    this.EpisodeItems.Add(new ItemViewModel() { Title = name, URL = _url, Information = _quality, ImageSource = new Uri(image, UriKind.RelativeOrAbsolute) });
                }
                //this.IsMovie = false;
                // return false;
            }
            else
            {
                // this.IsMovie = false;
            }


            this.IsLoading = false;
            this.IsDataLoaded = true;
        }
        public void WriteXML(string selectedIndex, string selectedNAME, string selectedIMA, string selectedTAP)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("FAVO.xml", FileMode.Create, myIsolatedStorage))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        using (XmlWriter write = XmlWriter.Create(isoStream, settings))
                        {
                            //ghi cai moi
                            write.WriteStartElement("p", "film", "urn:film");
                            write.WriteStartElement("URL", "");
                            write.WriteString(selectedIndex);
                            write.WriteEndElement();
                            write.WriteStartElement("Title", "");
                            write.WriteString(selectedNAME);
                            write.WriteEndElement();
                            write.WriteStartElement("IMAGE", "");
                            write.WriteString(selectedIMA);
                            write.WriteEndElement();
                            write.WriteStartElement("Information", "");
                            write.WriteString(selectedTAP);
                            write.WriteEndElement();
                            int y = App.ViewModel.Like.Count;
                            //ghi lai cai cu
                            for (int i = 0; i < y; i++)
                            {
                                if (App.ViewModel.Like[i].URL == selectedIndex)
                                {
                                    i++;
                                }
                                write.WriteStartElement("URL", "");
                                write.WriteString(App.ViewModel.Like[i].URL);
                                write.WriteEndElement();
                                write.WriteStartElement("Title", "");
                                write.WriteString(App.ViewModel.Like[i].Title);
                                write.WriteEndElement();
                                write.WriteStartElement("IMAGE", "");
                                write.WriteString(App.ViewModel.Like[i].IMAGE);
                                write.WriteEndElement();
                                write.WriteStartElement("Information", "");
                                write.WriteString(App.ViewModel.Like[i].Information);
                                write.WriteEndElement();
                            }
                            write.WriteEndDocument();
                            write.Flush();
                        }
                    }
                }
                this.Like.Add(new ItemViewModel() { URL = selectedIndex, IMAGE = selectedIMA, Title = selectedNAME, Information = selectedTAP, ImageSource = new Uri(selectedIMA, UriKind.RelativeOrAbsolute), Key = "f" });
                // MessageBox.Show("OK!");
            }
            catch
            {
            }
        }
        public void LoadXML()
        {
            this.Like.Clear();
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    IsolatedStorageFileStream isoFileStream = myIsolatedStorage.OpenFile("FAVO.xml", FileMode.Open);
                    using (StreamReader reader = new StreamReader(isoFileStream))
                    {
                        var html = reader.ReadToEnd();
                        MatchCollection Results = Regex.Matches(html, "<URL>(.*)\n(.*)\n(.*)\n(.*)");
                        foreach (Match result in Results)
                        {
                            string name = UIHelper.GetTxtBtwn(result.ToString(), "<Title>", "</Title>", 0);
                            string day = UIHelper.GetTxtBtwn(result.ToString(), "<Information>", "</Information>", 0);
                            string ima = UIHelper.GetTxtBtwn(result.ToString(), "<IMAGE>", "</IMAGE>", 0);
                            string link = UIHelper.GetTxtBtwn(result.ToString(), "<URL>", "</URL>", 0);
                            this.Like.Add(new ItemViewModel() { Title = name.ToUpper(), IMAGE = ima, URL = link, Information = day, ImageSource = new Uri(ima, UriKind.RelativeOrAbsolute), Key = "f" });
                        }
                    }
                }
            }
            catch
            {
            }
            this.IsDataLoaded = true;
        }
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }
        private BitmapImage bg = null;
        public BitmapImage BackGround
        {
            get
            {
                return bg;
            }
            set
            {
                bg = value;
                NotifyPropertyChanged("BackGround");
            }
        }
        private BitmapImage bg2 = null;
        public BitmapImage AdsBackGround
        {
            get
            {
                return bg2;
            }
            set
            {
                bg2 = value;
                NotifyPropertyChanged("AdsBackGround");
            }
        }
        private string titleads = null;
        public string AdsTitle
        {
            get
            {
                return titleads;
            }
            set
            {
                titleads = value;
                NotifyPropertyChanged("AdsTitle");
            }
        }
        public string idads = null;
        public string AdsId
        {
            get
            {
                return idads;
            }
            set
            {
                idads = value;
                NotifyPropertyChanged("AdsId");
            }
        }
        private string crtitle = null;
        public string CerrentTitle
        {
            get
            {
                return crtitle;
            }
            set
            {
                crtitle = value;
                NotifyPropertyChanged("CerrentTitle");
            }
        }
        private string crUrl = null;
        public string CerrentUrl
        {
            get
            {
                return crUrl;
            }
            set
            {
                crUrl = value;
                NotifyPropertyChanged("CerrentUrl");
            }
        }
        private ImageBrush rootbg = null;
        public ImageBrush RootBackGround
        {
            get
            {
                return rootbg;
            }
            set
            {
                rootbg = value;
                NotifyPropertyChanged("RootBackGround");
            }
        }
        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");

            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        /// 
        #region Load Data
        public void LoadData()
        {
            this.Genres.Clear();
            this.Directors.Clear();
            this.Nationals.Clear();
            this.Years.Clear();
            this.HomeItems.Clear();
            this.HomeItems.Add(new ItemViewModel() { Title = "released year", URL = "5", Key = "Y" });
            this.HomeItems.Add(new ItemViewModel() { Title = "genres", URL = "6", Key = "G" });
            this.HomeItems.Add(new ItemViewModel() { Title = "national", URL = "7", Key = "N" });
            this.Years.Add(new ItemViewModel() { URL = " phim-le/2015/", Title = "2015", Key = "Y" });
            this.Years.Add(new ItemViewModel() { URL = " phim-le/2014/", Title = "2014", Key = "Y" });
            this.Years.Add(new ItemViewModel() { URL = " phim-le/2013/", Title = "2013", Key = "Y" });
            this.Years.Add(new ItemViewModel() { URL = " phim-le/2012/", Title = "2012", Key = "Y" });
            this.Years.Add(new ItemViewModel() { URL = " phim-le/2011/", Title = "2011", Key = "Y" });
            this.Years.Add(new ItemViewModel() { URL = " phim-le/2010/", Title = "2010", Key = "Y" });
            this.Years.Add(new ItemViewModel() { URL = " phim-le/2009/", Title = "2009", Key = "Y" });
            this.Years.Add(new ItemViewModel() { URL = " phim-le/-2009/", Title = "<2009", Key = "Y" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/us/", Title = "USA" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/au/", Title = "Australia" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/in/", Title = "India" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/ca/", Title = "Canada" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/cn/", Title = "China" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/fr/", Title = "France" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/jp/", Title = "Japan" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/th/", Title = "Thailand" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/uk/", Title = "UK" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/vn/", Title = "Viet Nam" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/kr/", Title = "Korean" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/hk/", Title = "Hong Kong" });
            this.Nationals.Add(new ItemViewModel() { URL = "quoc-gia/tw/", Title = "Taiwan" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-hanh-dong/", Title = "Action" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-phieu-luu/", Title = "Adventure" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-hoat-hinh/", Title = "Animation" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-tai-lieu/", Title = "Biography" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-hai-huoc/", Title = "Comedy" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-than-thoai/", Title = "Drama" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-gia-dinh/", Title = "Family" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-vien-tuong/", Title = "Fantasy" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-co-trang/", Title = "History" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-kinh-di/", Title = "Horror" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-the-thao-am-nhac/", Title = "Music" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-hinh-su/", Title = "News" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-tam-ly/", Title = "Romance" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-vien-tuong/", Title = "Sci-Fi" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-the-thao-am-nhac/", Title = "Sport" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-kinh-di/", Title = "Thriller" });
            this.Genres.Add(new ItemViewModel() { URL = "the-loai/phim-chien-tranh/", Title = "War" });
            // Sample data; replace with real data
            this.IsDataLoaded = true;
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
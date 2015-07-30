using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FreeApp.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _title;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        private string _key;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (value != _key)
                {
                    _key = value;
                    NotifyPropertyChanged("Key");
                }
            }
        }
        private Uri imageSource;
        public Uri ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    NotifyPropertyChanged("ImageSource");
                }
            }
        }
        private string _url;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                if (value != _url)
                {
                    _url = value;
                    NotifyPropertyChanged("URL");
                }
            }
        }
        private string _info;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Information
        {
            get
            {
                return _info;
            }
            set
            {
                if (value != _info)
                {
                    _info = value;
                    NotifyPropertyChanged("Information");
                }
            }
        }
        private string _quality;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Quality
        {
            get
            {
                return _quality;
            }
            set
            {
                if (value != _quality)
                {
                    _quality = value;
                    NotifyPropertyChanged("Quality");
                }
            }
        }

        private string _image;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string IMAGE
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    NotifyPropertyChanged("IMAGE");
                }
            }
        }

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

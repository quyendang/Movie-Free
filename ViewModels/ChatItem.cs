using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeApp.ViewModels
{
    public class ChatItem : INotifyPropertyChanged
    {
        private string _messageText;
        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                if (value != _messageText)
                {
                    _messageText = value;
                    NotifyPropertyChanged("MessageText");
                }
            }
        }

        private string _createAt;
        public string CreateAt
        {
            get
            {
                return _createAt;
            }
            set
            {
                if (value != _createAt)
                {
                    _createAt = value;
                    NotifyPropertyChanged("CreateAt");
                }
            }
        }

        private string _senderID;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string SenderID
        {
            get
            {
                return _senderID;
            }
            set
            {
                if (value != _senderID)
                {
                    _senderID = value;
                    NotifyPropertyChanged("SenderID");
                }
            }
        }
        private Uri _avarta;
        public Uri Avatar
        {
            get
            {
                return _avarta;
            }
            set
            {
                if (value != _avarta)
                {
                    _avarta = value;
                    NotifyPropertyChanged("Avatar");
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;

namespace FreeApp.ViewModels
{
    public class TransferModel : INotifyPropertyChanged
    {
        public DownloadOperation DownloadOperation { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Title { get; set; }

        private int _progress;
        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                RaisePropertyChanged("Progress");
            }
        }
        private Uri _bImage = new Uri("", UriKind.RelativeOrAbsolute);
        private Uri BImage
        {
            get { return _bImage; }
            set
            {
                _bImage = value;
                RaisePropertyChanged("BImage");
            }
        }
        private string _phantram;
        public string PhanTram
        {
            get { return _phantram; }
            set
            {
                _phantram = value;
                RaisePropertyChanged("PhanTram");
            }
        }
        private string _filesize;
        public string FileSize
        {
            get
            {
                return _filesize;
            }
            set
            {
                _filesize = value;
                RaisePropertyChanged("FileSize");
            }
        }
        private ulong _totalBytesToReceive;
        public ulong TotalBytesToReceive
        {
            get
            {
                return _totalBytesToReceive;
            }
            set
            {
                _totalBytesToReceive = value;
                RaisePropertyChanged("TotalBytesToReceive");
            }
        }

        private ulong _bytesReceived;
        public ulong BytesReceived
        {
            get
            {
                return _bytesReceived;
            }
            set
            {
                _bytesReceived = value;
                RaisePropertyChanged("BytesReceived");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}

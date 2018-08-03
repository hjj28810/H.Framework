using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace H.Framework.WPF.Infrastructure.Mvvm
{
    public class BasicViewModel : BasicEntity
    {
        private string _WarnText;
        private bool _isShowWarn;
        private bool _isLoadingBusy;

        public string WarnText
        {
            get { return _WarnText; }
            set
            {
                _WarnText = value;
                NotifyPropertyChanged("WarnText");
                ShowWarn();
            }
        }

        public bool IsLoadingBusy
        {
            get { return _isLoadingBusy; }
            set
            {
                if (_isLoadingBusy != value)
                {
                    _isLoadingBusy = value;
                    NotifyPropertyChanged("IsLoadingBusy");
                }
            }
        }

        public bool IsShowWarn
        {
            get { return _isShowWarn; }
            set
            {
                if (_isShowWarn != value)
                {
                    _isShowWarn = value;
                    NotifyPropertyChanged("IsShowWarn");
                }
            }
        }

        private void ShowWarn()
        {
            IsShowWarn = true;
        }
    }
}
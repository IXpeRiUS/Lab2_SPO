using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class StartText : INotifyPropertyChanged
    {
        private string _sText;
        public string SText
        {
            get { return _sText; }
            set
            {
                _sText = value;
                OnPropertyChanged("SText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

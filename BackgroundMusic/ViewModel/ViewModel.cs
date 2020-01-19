using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BackgroundMusic.ViewModel
{
    public class ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

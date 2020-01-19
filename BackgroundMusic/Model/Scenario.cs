using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BackgroundMusic.Model
{
    public class Scenario : INotifyPropertyChanged
    {
        public Scenario()
        {
            Audios = new List<Audio>();
        }

        public List<Audio> Audios { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

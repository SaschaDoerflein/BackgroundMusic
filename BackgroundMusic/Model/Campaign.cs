using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using BackgroundMusic.Annotations;

namespace BackgroundMusic.Model
{
    public class Campaign : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Scenario> _scenarios;

        public string Path { get; set; }
        public string Name { get; set; }


        public Campaign(string path, string name)
        {
            Path = path;
            Name = name;
        }

        public ObservableCollection<Scenario> Scenarios
        {
            get { return _scenarios; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

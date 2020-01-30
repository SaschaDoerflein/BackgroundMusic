using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BackgroundMusic.Model;
using BackgroundMusic.ViewModel;

namespace BackgroundMusic.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BackgroundMusicViewModel backgroundMusicViewModel = new BackgroundMusicViewModel();
            this.Title = "BackgroundMusic v" + backgroundMusicViewModel.VersionNumber;
        }

        private void MenuItemPlay(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Audio audio = menuItem.DataContext as Audio;
            //audio.Play();
        }

    }
}

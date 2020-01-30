using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BackgroundMusic.Model;

namespace BackgroundMusic.ViewModel
{
    public class MenuViewModel : ViewModel
    {
        private BackgroundMusicViewModel _backgroundMusicViewModel;

        public MenuViewModel(BackgroundMusicViewModel backgroundMusicViewModel)
        {
            _backgroundMusicViewModel = backgroundMusicViewModel;
        }

        public ICommand NewCampaignCommand
        {
            get
            {
                return new RelayCommand(
                    param =>
                    {
                        Campaign newCampaign = new Campaign();
                        //TODO:
                    },
                    param => true
                );
            }
        }

    }
}

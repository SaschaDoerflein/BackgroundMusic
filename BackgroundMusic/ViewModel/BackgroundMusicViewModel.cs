using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using BackgroundMusic.InOut;
using BackgroundMusic.Model;
using BackgroundMusic = BackgroundMusic.Model.BackgroundMusic;
using File = System.IO.File;

namespace BackgroundMusic.ViewModel
{
    public class BackgroundMusicViewModel : ViewModel
    {
        private const string GlobalSavePath = @"Save.dat";
        private Campaign _campaign;
        private Model.BackgroundMusic _backgroundMusic;

        public Campaign Campaign
        {
            get => _campaign;
            set
            {
                _campaign = value;
                _backgroundMusic.LastOpenedCampaignPath = Path.Combine(_campaign.Path, _campaign.Name, ".dat");
                WriteGlobalSave(GlobalSavePath, _backgroundMusic);
                OnPropertyChanged(nameof(Campaign));
            }
        }

        public string VersionNumber => GetVersionNumber();

        public BackgroundMusicViewModel()
        {
            _backgroundMusic = ReadGlobalSave(GlobalSavePath);
            var lastOpenedCampaign = GetLastCampaign(_backgroundMusic.LastOpenedCampaignPath);
            Campaign = lastOpenedCampaign;
        }

        public void WriteGlobalSave(string pathToGlobalSave, Model.BackgroundMusic backgroundMusic)
        {
            if (File.Exists(pathToGlobalSave))
            {
                FileHandler.WriteToXmlFile(pathToGlobalSave, backgroundMusic);
            }
        }

        public Model.BackgroundMusic ReadGlobalSave(string pathToGlobalSave)
        {
            Model.BackgroundMusic backgroundMusic;

            if (File.Exists(pathToGlobalSave))
            {
                backgroundMusic = FileHandler.ReadFromXmlFile<Model.BackgroundMusic>(pathToGlobalSave);
            }
            else
            {
                backgroundMusic = CreateNewBackgroundMusicSave(pathToGlobalSave);
            }

            return backgroundMusic;
        }

        private static Model.BackgroundMusic CreateNewBackgroundMusicSave(string pathToGlobalSave)
        {
            Model.BackgroundMusic backgroundMusic;

            var campaignPath = Path.GetDirectoryName(pathToGlobalSave);
            var campaign = new Campaign(campaignPath, "New Campaign");
            FileHandler.WriteToXmlFile(campaignPath, campaign);

            backgroundMusic = new Model.BackgroundMusic {LastOpenedCampaignPath = campaignPath};

            FileHandler.WriteToXmlFile(pathToGlobalSave, backgroundMusic);
            return backgroundMusic;
        }

        private Campaign GetLastCampaign(string pathToLastCampaign)
        {
            var campaign = FileHandler.ReadFromXmlFile<Campaign>(pathToLastCampaign);

            return campaign;
        }

        private string GetVersionNumber()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
            var version = fileVersionInfo.FileVersion;
            return version;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Input;
using BackgroundMusic.AudioHandler;
using BackgroundMusic.Model;

namespace BackgroundMusic.ViewModel
{
    public class CampaignViewModel : ViewModel
    {
        private Campaign _campaign;
        private ObservableCollection<Audio> _audios;
        private ObservableCollection<Scenario> _scenarios;
        private Scenario _currentScenario;

        private ICommand _loadAudiosCommand;
        private ICommand _addAudioToScenarioAtmosCommand;

        public ObservableCollection<Audio> Audios
        {
            get => _audios;

            set
            {
                if (Audios != value)
                {
                    _audios = value;
                    OnPropertyChanged(nameof(Audios));
                }
            }
        }

        public ObservableCollection<Scenario> Scenarios
        {
            get => _scenarios;

            set
            {
                if (Scenarios != value)
                {
                    _scenarios = value;
                    OnPropertyChanged(nameof(Scenarios));
                }
            }
        }

        public Scenario CurrentScenario
        {
            get => _currentScenario;

            set
            {
                if (CurrentScenario != value)
                {
                    _currentScenario = value;
                    OnPropertyChanged(nameof(CurrentScenario));
                }
            }
        }

        public ICommand LoadAudiosCommand
        {
            get
            {
                if (_loadAudiosCommand == null)
                {
                    _loadAudiosCommand = new RelayCommand(p => ExecuteLoadAudiosCommand());
                }
                return _loadAudiosCommand;
            }
        }
        public ICommand AddAudioToScenarioAtmosCommand
        {
            get
            {
                return new RelayCommand(
                    param =>
                    {
                        var selectedAudio = (Audio)param;

                        _currentScenario.Atmos.Add(selectedAudio);
                    },
                    param => true
                );
            }
        }

        public CampaignViewModel(string campaignPath)
        {
            _campaign = new Campaign(campaignPath, "New Campaign");
            _audios = new ObservableCollection<Audio>();
            _audios.CollectionChanged += Audios_CollectionChanged;
        }

        private void Audios_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (AudioViewModel vm in e.NewItems)
                {
                    throw new NotImplementedException();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (AudioViewModel vm in e.OldItems)
                {
                    throw new NotImplementedException();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                throw new NotImplementedException();
            }
        }

        private void ExecuteLoadAudiosCommand()
        {
            _campaign.GetAudiosFromDirectory(new NAudioHandler(_campaign.Path));
        }

    }
}

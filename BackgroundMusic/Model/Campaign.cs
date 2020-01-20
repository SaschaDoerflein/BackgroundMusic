using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using BackgroundMusic.Annotations;
using BackgroundMusic.AudioHandler;

namespace BackgroundMusic.Model
{
    public class Campaign { 

        private readonly List<Scenario> _scenarios;

        public List<Audio> PlayedAudios
        {
            get
            {
                var playedAudios = new List<Audio>();
                foreach (var scenario in Scenarios)
                {
                    foreach (var audio in scenario.Audios)
                    {
                        if (audio.State == AudioState.Play)
                        {
                            playedAudios.Add(audio);
                        }
                        
                    }
                }

                return playedAudios;
            }

        }
        public string Path { get; set; }
        public string Name { get; set; }


        public Campaign(string path, string name)
        {
            Path = path;
            Name = name;
        }

        public List<Scenario> Scenarios => _scenarios;

        public List<Audio> GetAudiosFromDirectory(IAudioHandler audioHandler)
        {
            var filePaths = GetFilePaths(audioHandler);

            var audios = CreateAudios(audioHandler, filePaths);

            return audios;
        }

        //TODO: Create Soundboard to control overall effects like mute or toggle
        public void ToggleAllPlayedAudios()
        {
            foreach (var audio in PlayedAudios)
            {
                audio.Stop();
            }
        }

        public void Mute()
        {
            throw new NotImplementedException();
        }

        private static List<Audio> CreateAudios(IAudioHandler audioHandler, List<string> filePaths)
        {
            var audios = new List<Audio>();
            foreach (var filePath in filePaths)
            {
                var audio = new Audio(audioHandler.CreateNewInstance(filePath));
            }

            return audios;
        }

        private List<string> GetFilePaths(IAudioHandler audioHandler)
        {
            var filePaths = new List<string>();
            foreach (var supportedFileExtension in audioHandler.SupportedFileExtensions)
            {
                filePaths.AddRange(Directory.GetFiles(Path, supportedFileExtension.ToString()));
            }

            return filePaths;
        }
    }
}

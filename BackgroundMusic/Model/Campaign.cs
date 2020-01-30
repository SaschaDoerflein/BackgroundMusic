using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using BackgroundMusic.Annotations;
using BackgroundMusic.AudioHandler;

namespace BackgroundMusic.Model
{
    public class Campaign { 

        private List<Audio> _playedAudios;

        
        public List<Audio> PlayedAudios
        {
            get
            {
                var playedAudios = new List<Audio>();
                foreach (var scenario in Scenarios)
                {
                    foreach (var audio in scenario.AllAudios)
                    {
                        if (audio.State == AudioState.Play)
                        {
                            playedAudios.Add(audio);
                        }
                    }
                }

                return playedAudios;
            }

            set => _playedAudios = value;
        }

        public List<Audio> AllAudios { get; set; }

        public string Path { get; set; }
        public string Name { get; set; }

        public Campaign()
        {
            PlayedAudios = new List<Audio>();
            Scenarios = new List<Scenario>();
        }

        public Campaign(string path, string name)
        {
            Path = path;
            Name = name;
            PlayedAudios = new List<Audio>();
            Scenarios = new List<Scenario>();
        }
        
        public List<Scenario> Scenarios { get; set; }
        
        public List<Audio> GetAudiosFromDirectory(NAudioHandler audioHandler)
        {
            var filePaths = GetFilePaths(audioHandler);

            var audios = CreateAudios(audioHandler, filePaths);

            return audios;
        }


        public void Mute()
        {
            throw new NotImplementedException();
        }

        private static List<Audio> CreateAudios(NAudioHandler audioHandler, List<string> filePaths)
        {
            var audios = new List<Audio>();
            foreach (var filePath in filePaths)
            {
                var audio = new Audio(audioHandler.CreateNewInstance(filePath));
            }

            return audios;
        }

        private List<string> GetFilePaths(NAudioHandler audioHandler)
        {
            var filePaths = new List<string>();
            foreach (var supportedFileExtension in audioHandler.SupportedFileExtensions)
            {
                filePaths.AddRange(Directory.GetFiles(Path, supportedFileExtension.ToString(), SearchOption.AllDirectories));
            }

            return filePaths;
        }

        

    }
}

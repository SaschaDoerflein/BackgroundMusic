using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Input;
using BackgroundMusic.AudioHandler;
using BackgroundMusic.InOut;

namespace BackgroundMusic.Model
{
    [Serializable]
    public class Audio

    {
        private NAudioHandler _audioHandler;

        public Audio()
        {

        }
        
        public Audio(NAudioHandler audioHandler)
        {
            _audioHandler = audioHandler;
            Path = audioHandler.Path;
            Name = audioHandler.AudioName;
            FileExtension = audioHandler.FileExtension;
            State = AudioState.Stopped;
            
        }

        public string Name { get; set; }
        public string Path { get; set; }

        public Key Shortcut { get; set; }

        private NAudioHandler AudioHandler
        {
            get
            {
                if (_audioHandler == null)
                {
                    _audioHandler = new NAudioHandler(FullPath);
                }else if (!_audioHandler.IsInitialized)
                {
                    _audioHandler = new NAudioHandler(FullPath);
                }
                
                return _audioHandler;
            }

            set
            {
                _audioHandler = value;
                Name = _audioHandler.AudioName;
                Path = _audioHandler.Path;
                FileExtension = _audioHandler.FileExtension;
            } 
        }
 
        public string FullPath
        {
            get
            {
                var fullPath = Path + @"\\" + Name+"."+FileExtension;
                return fullPath;
            }
        }
        
        public AudioState State { get; set; }

        public File.FileExtension FileExtension { get; set; }
        public TimeSpan TimePosition => _audioHandler.TimePosition;
        public TimeSpan TotalDuration => _audioHandler.TotalDuration;

        public bool IsRepeating => _audioHandler.IsRepeating;

        public void Play()
        {
            State = AudioState.Play;
            AudioHandler.Play();
        }

        public void Stop()
        {
            State = AudioState.Stopped;
            AudioHandler.Stop();
        }

        public void Pause()
        {
            State = AudioState.Paused;
            AudioHandler.Pause();
        }
       
    }

    public enum AudioState
    {
        Play,
        Paused,
        Stopped
    }
   
}

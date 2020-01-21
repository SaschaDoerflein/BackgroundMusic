using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using BackgroundMusic.AudioHandler;

namespace BackgroundMusic.Model
{
    [Serializable]
    public class Audio

    {
        private readonly IAudioHandler _audioHandler;

        public Audio(IAudioHandler audioHandler)
        {
            _audioHandler = audioHandler;
            State = AudioState.Stopped;
        }

        public string Name => _audioHandler.AudioName;
        public string Path => _audioHandler.Path;

        public string FullPath
        {
            get
            {
                var fullPath = Path + @"\\" + Name;
                return fullPath;
            }
        }

        public AudioState State { get; private set; }

        public FileExtension FileExtension => _audioHandler.FileExtension;
        public TimeSpan TimePosition => _audioHandler.TimePosition;
        public TimeSpan TotalDuration => _audioHandler.TotalDuration;

        public bool IsRepeating => _audioHandler.IsRepeating;

        public void Play()
        {
            State = AudioState.Play;
            _audioHandler.Play();
        }

        public void Stop()
        {
            State = AudioState.Stopped;
            _audioHandler.Stop();
        }

        public void Pause()
        {
            State = AudioState.Paused;
            _audioHandler.Pause();
        }

    }

    public enum AudioState
    {
        Play,
        Paused,
        Stopped
    }
}

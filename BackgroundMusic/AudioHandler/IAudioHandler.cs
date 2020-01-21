using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using BackgroundMusic.Model;

namespace BackgroundMusic.AudioHandler
{
    public interface IAudioHandler
    {
        public IAudioHandler CreateNewInstance(string path);
        public string Path { get; }
        public string AudioName { get; }
        public FileExtension FileExtension { get; }
        public ImmutableArray<FileExtension> SupportedFileExtensions { get; }
        TimeSpan TotalDuration { get; }
        TimeSpan TimePosition { get; set; }
        bool IsRepeating { get; set; }
        void Play();
        void Pause();
        void Stop();
        void Dispose();

    }

    public enum FileExtension
    {
    Mp3,
    Wav
    }
}

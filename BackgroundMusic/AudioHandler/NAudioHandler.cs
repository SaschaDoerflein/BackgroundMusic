using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using BackgroundMusic.Model;
using NAudio.Wave;

namespace BackgroundMusic.AudioHandler
{
    public class NAudioHandler : IAudioHandler
    {
        

        private IWavePlayer _waveOutDevice;
        private WaveChannel32 _outputChannel;

        public NAudioHandler(string audioFilePath)
        {


            var pathNameSplitIndex = audioFilePath.LastIndexOf(@"\", StringComparison.Ordinal)+1;
            var fileExtensionSplitIndex = audioFilePath.LastIndexOf(".", StringComparison.Ordinal) + 1;

            Path = audioFilePath.Substring(0, pathNameSplitIndex);
            AudioName = audioFilePath.Substring(pathNameSplitIndex, fileExtensionSplitIndex);
            var fileExtensionString = audioFilePath.Substring(fileExtensionSplitIndex, audioFilePath.Length - Path.Length);
            foreach (var supportedFileExtension in SupportedFileExtensions)
            {
                if (string.Equals(fileExtensionString, supportedFileExtension.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    FileExtension = supportedFileExtension;
                    break;
                }
            }

            this._outputChannel = InitInputStream(audioFilePath);
            this._waveOutDevice = InitOutDevice(this._outputChannel);
        }

        public IAudioHandler CreateNewInstance(string path)
        {
            NAudioHandler audioHandler = new NAudioHandler(path);
            return audioHandler;
        }

        public string Path { get; }
        public string AudioName { get; }
        public FileExtension FileExtension { get; }

        public ImmutableArray<FileExtension> SupportedFileExtensions
        {
            get
            {
                return ImmutableArray.Create(new FileExtension[] { FileExtension.Mp3, FileExtension.Wav });
            }
        } 

        public TimeSpan TotalDuration => _outputChannel?.TotalTime ?? TimeSpan.Zero;

        public TimeSpan TimePosition
        {
            get => _outputChannel?.CurrentTime ?? TimeSpan.Zero;
            set => _outputChannel.CurrentTime = value;
        }

        public void Play()
        {
            if (TimePosition >= TotalDuration)
            {
                this.Stop();
                _waveOutDevice?.Play();
            }
            else if (_waveOutDevice?.PlaybackState != PlaybackState.Playing)
            {
                _waveOutDevice?.Play();
            }
        }

        public void Pause()
        {
            _waveOutDevice?.Pause();
        }

        public void Stop()
        {
            _waveOutDevice?.Stop();
            TimePosition = TimeSpan.Zero;
        }

        private WaveChannel32 InitInputStream(string audioFilePath)
        {
            var readerStream = CreateReaderStream(audioFilePath);
            if (readerStream == null)
            {
                throw new InvalidOperationException("Unsupported extension");
            }

            var volumeStream = new WaveChannel32(readerStream);
            volumeStream.PadWithZeroes = false;
            return volumeStream;
        }

        private WaveStream CreateReaderStream(string audioFilePath)
        {
            WaveStream readerStream = null;
            if (audioFilePath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {
                readerStream = new WaveFileReader(audioFilePath);
                if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
                {
                    readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
                    readerStream = new BlockAlignReductionStream(readerStream);
                }
            }
            else if (audioFilePath.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
            {
                readerStream = new Mp3FileReader(audioFilePath);
            }
            return readerStream;
        }

        private IWavePlayer InitOutDevice(IWaveProvider volumeStream)
        {
            var waveOutDevice = new WaveOut();
            waveOutDevice.Init(volumeStream);
            waveOutDevice.PlaybackStopped += new EventHandler<StoppedEventArgs>(PlaybackStoppedEvent);
            return waveOutDevice;
        }

        private void PlaybackStoppedEvent(object sender, StoppedEventArgs e)
        {
            Stop();
        }

        public void Dispose()
        {
            _waveOutDevice?.Stop();
            _outputChannel?.Close(); 
            _outputChannel = null;
            _waveOutDevice?.Dispose();
            _waveOutDevice = null;

            GC.Collect();
        }
    }
}

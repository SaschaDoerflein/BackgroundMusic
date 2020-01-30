using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Timers;
using BackgroundMusic.InOut;
using BackgroundMusic.Model;
using NAudio.Wave;

namespace BackgroundMusic.AudioHandler
{
    public class NAudioHandler
    {
        private IWavePlayer _waveOutDevice;
        private WaveChannel32 _outputChannel;
        private Timer _timer;
        private Random _randomGenerator;

        public NAudioHandler()
        {

        }

        public NAudioHandler(string audioFilePath)
        {
            _randomGenerator = new Random();
            var pathNameSplitIndex = audioFilePath.LastIndexOf(@"\", StringComparison.Ordinal)+1;
            var fileExtensionSplitIndex = audioFilePath.LastIndexOf(".", StringComparison.Ordinal) + 1;

            Path = audioFilePath.Substring(0, pathNameSplitIndex);

            var fileNameLenght = fileExtensionSplitIndex - pathNameSplitIndex-1;

            AudioName = audioFilePath.Substring(pathNameSplitIndex, fileNameLenght);
            var fileExtensionString = audioFilePath.Substring(fileExtensionSplitIndex, (audioFilePath.Length - (Path.Length+AudioName.Length)-1));
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
            IsInitialized = true;
        }


        public TimeSpan WaitTime { get; set; }
        public TimeSpan RandomWaitTime { get; set; }
        public string Path { get; }
        public string AudioName { get; }
        public File.FileExtension FileExtension { get; }
        public NAudioHandler CreateNewInstance(string path)
        {
            NAudioHandler audioHandler = new NAudioHandler(path);
            return audioHandler;
        }
        public ImmutableArray<File.FileExtension> SupportedFileExtensions
        {
            get
            {
                return ImmutableArray.Create(new File.FileExtension[] { File.FileExtension.Mp3, File.FileExtension.Wav });
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

        public bool IsRepeating { get; set; }
        public bool IsInitialized { get; set; }

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
            if (IsRepeating)
            {

                _timer = new Timer();
                _timer.Elapsed += new ElapsedEventHandler(PlayAudio);
                _timer.Interval = WaitTime.TotalMilliseconds+GetRandomWaitingTime();
                _timer.Enabled = true;
                Play();
            }
        }

        private int GetRandomWaitingTime()
        {
            var randomWaitingTime = 0;

            randomWaitingTime = _randomGenerator.Next(0, (int)RandomWaitTime.TotalMilliseconds);

            return randomWaitingTime;
        }

        public void PlayAudio(object source, ElapsedEventArgs e)
        {
            Play();
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

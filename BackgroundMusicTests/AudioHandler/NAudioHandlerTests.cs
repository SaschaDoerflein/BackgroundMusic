using System;
using System.Collections.Generic;
using System.Text;
using BackgroundMusic.AudioHandler;
using NUnit.Framework;

namespace BackgroundMusicTests.AudioHandler
{
    [TestFixture]
    public class NAudioHandlerTests
    {
        [Datapoint] 
        private const string SoundFilePath = 
            @"D:\Projects\BackgroundMusic\BackgroundMusicTests\Resources\Sounds\thunder_strike_1-Mike_Koenig-739781745.mp3";


        [Test]
        [Category("Unit")]
        public void NAudioHandler_TotalDuration_ConstructorTest()
        {
            var nAudioHandler = new NAudioHandler(SoundFilePath);

            Assert.AreEqual(21, nAudioHandler.TotalDuration.Seconds);
        }

        [Theory]
        [Category("Unit")]
        public void NAudioHandler_AudioName_ConstructorTest()
        {
            var nAudioHandler = new NAudioHandler(SoundFilePath);
            
            Assert.AreEqual(@"thunder_strike_1-Mike_Koenig-739781745.mp3", nAudioHandler.AudioName);
        }

        [Theory]
        [Category("Unit")]
        public void NAudioHandler_Path_ConstructorTest()
        {
            var nAudioHandler = new NAudioHandler(SoundFilePath);
            const string exceptedPath = @"D:\Projects\BackgroundMusic\BackgroundMusicTests\Resources\Sounds\";
            Assert.AreEqual(@exceptedPath, nAudioHandler.AudioName);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using BackgroundMusic.Annotations;
using BackgroundMusic.AudioHandler;
using BackgroundMusic.InOut;
using BackgroundMusic.Model;
using NUnit.Framework;

namespace BackgroundMusicTests.InOut
{
    public class TestObject
    {
        public int Math { get; set; }
        public string Text { get; set; }

        public List<InnerTestObject> Texts { get; set; }

        public TestObject()
        {
            Texts = new List<InnerTestObject>();
        }
    }

    public class InnerTestObject
    {
        public List<string> Texts { get; set; }
    }

    [TestFixture]
    public class SaveFileSerializerTests
    {

        [Test]
        [Category("Integration")]
        public void SaveFileSerializer_Serialize_Deserialize_Campaign()
        {
            var campaign = new Campaign();
            
            var nAudioHandler = new NAudioHandler(@"TestSounds\Chop.mp3");
            var audio = new Audio(nAudioHandler);

            audio.Play();
            Scenario scenario = new Scenario();
            scenario.Atmos.Add(audio);
            campaign.Scenarios.Add(scenario);
            Assert.AreEqual(1,campaign.PlayedAudios.Count);
            var serializeCampaign = Serializer.Serialize<Campaign>(campaign);
            var deserializeCampaign = Serializer.Deserialize<Campaign>(serializeCampaign);
            deserializeCampaign.PlayedAudios[0].Play();
            Assert.AreNotEqual(null, serializeCampaign);
        }


        [Test]
        [Category("Unit")]
        public void SaveFileSerializer_Serialize_Campaign()
        {
            var campaign = new Campaign();
            var serializeCampaign = Serializer.Serialize<Campaign>(campaign);

            Assert.AreNotEqual(null,serializeCampaign);
        }

        [Test]
        [Category("Unit")]
        public void SaveFileSerializer_Serialize_Scenario()
        {
            var scenario = new Scenario();
            var serializeScenario = Serializer.Serialize<Scenario>(scenario);

            Assert.AreNotEqual(null, serializeScenario);
        }

        [Test]
        [Category("Unit")]
        public void SaveFileSerializer_Serialize_TestObject()
        {
            TestObject testObject = new TestObject();
            testObject.Text = "Bla";
            List<string> texts = new List<string>(); 
            texts.Add("blub");
            List<InnerTestObject> innerTestObjects = new List<InnerTestObject>();
            
            InnerTestObject innerObject = new InnerTestObject();
            innerTestObjects.Add(innerObject);
            innerObject.Texts = texts;
            testObject.Texts = innerTestObjects;
            var serializeObject = Serializer.Serialize<TestObject>(testObject);

            Assert.AreEqual(null, serializeObject);
        }

        [Test]
        [Category("Unit")]
        public void SaveFileSerializer_Derialize_TestObject()
        {
            TestObject testObject = new TestObject();
            testObject.Text = "Bla";
            List<string> texts = new List<string>();
            texts.Add("blub");
            List<InnerTestObject> innerTestObjects = new List<InnerTestObject>();

            InnerTestObject innerObject = new InnerTestObject();
            innerTestObjects.Add(innerObject);
            innerObject.Texts = texts;
            testObject.Texts = innerTestObjects;
            var serializeObject = Serializer.Serialize<TestObject>(testObject);

            TestObject deserializeTestObject = Serializer.Deserialize<TestObject>(serializeObject);

            Assert.AreEqual(null, deserializeTestObject);
        }

        [Test]
        [Category("Unit")]
        public void SaveFileSerializer_GetLastCampaign_Test()
        {
            
        }
    }
}

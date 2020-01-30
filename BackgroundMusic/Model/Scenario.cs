using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BackgroundMusic.Model
{
    public class Scenario 
    {
        public Scenario()
        {
            Atmos = new List<Audio>();
            Sounds = new List<Audio>();
        }

        public List<Audio> AllAudios
        {
            get
            {
                List<Audio> allAudios = new List<Audio>();
                allAudios.AddRange(Atmos);
                allAudios.AddRange(Sounds);
                return allAudios;
            }
        }

        public List<Audio> Atmos { get; set; }

        public List<Audio> Sounds { get; set; }

        public void PlayAtmos()
        {
            foreach (var atmo in Atmos)
            {
                atmo.Play();
            }
        }

        public void StopAtmos()
        {
            foreach (var atmo in Atmos)
            {
                atmo.Stop();
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMaze
{

    class SoundHelper
    {
        #region singleton thread safe
        private static readonly SoundHelper instance = new SoundHelper();
        private SoundHelper() { }
        static SoundHelper()               //explicit static .ctor to tell C# compiler 
        {                                    //not to mark type as beforefieldinit
            instance.Initialize();
        }
        public static SoundHelper Instance
        {
            get { return instance; }
        }
        #endregion singleton

        public Parametres.EtatsMusic etat_music;
        public Parametres.EtatsEffet etat_effect;

        public SoundEffectInstance sound_classic;
        public SoundEffectInstance sound_ambiance;
        public SoundEffectInstance sound_victory;
        public SoundEffectInstance sound_platform;

        float son_max;

        public void Initialize()
        {
            son_max = 0.2f;

            sound_classic = ScreenManager.Instance.Content.Load<SoundEffect>(@"Sounds\music").CreateInstance();
            sound_ambiance = ScreenManager.Instance.Content.Load<SoundEffect>(@"Sounds\ambiance").CreateInstance();
            sound_victory = ScreenManager.Instance.Content.Load<SoundEffect>(@"Sounds\victory").CreateInstance();
            sound_platform = ScreenManager.Instance.Content.Load<SoundEffect>(@"Sounds\platform").CreateInstance();

            sound_classic.IsLooped = true;
            sound_ambiance.IsLooped = true;
            sound_victory.IsLooped = false;
            sound_platform.IsLooped = false;

            sound_classic.Volume = 0;
            sound_ambiance.Volume = 0;
            sound_victory.Volume = 0;
            sound_platform.Volume = 0;

            sound_classic.Play();
            sound_ambiance.Play();
        }

        public void Dispose()
        {
            sound_classic.Dispose();
            sound_ambiance.Dispose();
            sound_platform.Dispose();
            sound_victory.Dispose();
        }

                #region Music
        public void PlayClassic(GameTime gameTime = null)
        {
            if (etat_music != Parametres.EtatsMusic.sound_classic) return;
            if (sound_classic.Volume == son_max) return;

            if (sound_ambiance.Volume > 0)
            {
                if (sound_ambiance.Volume < 0.002f * gameTime.ElapsedGameTime.Milliseconds)
                {
                    sound_classic.Volume = son_max;
                    sound_ambiance.Volume = 0;
                }
                else
                {
                    sound_classic.Volume += (son_max/500) * gameTime.ElapsedGameTime.Milliseconds;
                    sound_ambiance.Volume -= (son_max / 500) * gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            else
            {
                sound_classic.Volume = son_max;
            }
        }

        public void PlayAmbiance(GameTime gameTime = null)
        {
            if (etat_music != Parametres.EtatsMusic.sound_ambiance) return;
            if (sound_ambiance.Volume == son_max) return;

            if (sound_classic.Volume > 0)
            {
                if (sound_classic.Volume < 0.0004f * gameTime.ElapsedGameTime.Milliseconds)
                {
                    sound_classic.Volume = 0;
                    sound_ambiance.Volume = son_max;
                }
                else
                {
                    sound_classic.Volume -= (son_max / 500) * gameTime.ElapsedGameTime.Milliseconds;
                    sound_ambiance.Volume += (son_max / 500) * gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            else
            {
                sound_ambiance.Volume = son_max;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (etat_music == Parametres.EtatsMusic.sound_ambiance)
                PlayAmbiance(gameTime);

            else if (etat_music == Parametres.EtatsMusic.sound_classic)
                PlayClassic(gameTime);
        }

        public void ActiveClassic()
        {
            etat_music = Parametres.EtatsMusic.sound_classic;
        }

        public void ActiveAmbiance()
        {
            etat_music = Parametres.EtatsMusic.sound_ambiance;
        }

        public void ChangeTurnOff()
        {
            if (etat_music == Parametres.EtatsMusic.sound_ambiance)
                etat_music = Parametres.EtatsMusic.sound_classic;
            else if (etat_music == Parametres.EtatsMusic.sound_classic)
            {
                etat_music = Parametres.EtatsMusic.inactif;
                VolumeOff();
            }
            else
            {
                etat_music = Parametres.EtatsMusic.sound_ambiance;
                VolumeOn();
            }
        }

        public void VolumeOn()
        {
            if(etat_music == Parametres.EtatsMusic.sound_classic)
                sound_classic.Volume = son_max;
            else if(etat_music == Parametres.EtatsMusic.sound_ambiance)
                sound_ambiance.Volume = son_max;
        }

        public void VolumeOff()
        {
            sound_classic.Volume = 0;
            sound_ambiance.Volume = 0;
        }

        public bool isMusicOn()
        {
            if (SoundHelper.Instance.sound_ambiance.Volume > 0 ||
                SoundHelper.Instance.sound_classic.Volume > 0)
                return true;

            return false;
        }
#endregion

#region Effects
        public void PlayPlatformEffect()
        {
            sound_platform.Play();
        }

        public void PlayVictoryEffect()
        {
            sound_victory.Play();
        }

        public void EffectsOn()
        {
            etat_effect = Parametres.EtatsEffet.actif;

            sound_platform.Volume = son_max;
            sound_victory.Volume = son_max;
        }

        public void EffectsOff()
        {
            etat_effect = Parametres.EtatsEffet.inactif;

            sound_platform.Volume = 0;
            sound_victory.Volume = 0;
        }

        public bool isEffectsOn()
        {
            if (SoundHelper.Instance.sound_platform.Volume > 0 ||
                SoundHelper.Instance.sound_victory.Volume > 0)
                return true;

            return false;
        }

#endregion
    }
}

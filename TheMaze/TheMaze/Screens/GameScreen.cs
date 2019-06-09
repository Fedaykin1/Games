using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace TheMaze
{
    public abstract class GameScreen
    {
        public ContentManager Content { get; private set; }
        public static Scores scores;
        public static Parametres parametres;

        public GameScreen()
        {
            if (File.Exists(@"json\scores.json"))
                scores = JsonHelper<Scores>.Read(@"json\scores.json");
            else
                scores = new Scores();

            if (File.Exists(@"json\parametres.json"))
                parametres = JsonHelper<Parametres>.Read(@"json\parametres.json");
            else
                parametres = new Parametres();
        }

        public virtual void LoadContent()
        {
            this.Content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "content");
        }

        public virtual void UnloadContent()
        {
            JsonHelper<Scores>.Write(scores, @"json\scores.json");
            JsonHelper<Parametres>.Write(parametres, @"json\parametres.json");

            Content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch i_spriteBatch)
        {
        }
    }

    [DataContract]
    public class Scores
    {
        [DataMember(Name ="scores")]
        public Hashtable ht_scores = new Hashtable();
    }

    [DataContract]
    public class Parametres                 // valeurs possibles
    {
        public enum enum_fond { eau, lave};
        public enum EtatsMusic { inactif, sound_classic, sound_ambiance };
        public enum EtatsEffet { inactif, actif };

        [DataMember]
        public string num_design = "0"; // 0/1/2/3

        [DataMember]
        public enum_fond type_fond = enum_fond.eau;

        [DataMember]
        public EtatsMusic music = EtatsMusic.sound_ambiance;

        [DataMember]
        public EtatsEffet effects = EtatsEffet.actif;
    }

}

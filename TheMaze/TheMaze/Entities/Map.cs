using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMaze.Entities
{
    class Map
    {
        #region singleton thread safe
        private static readonly Map instance = new Map();
        private Map() { }
        static Map()                //explicit static .ctor to tell C# compiler 
        {                           //not to mark type as beforefieldinit
        }
        public static Map Instance
        {
            get { return instance; }
        }
        #endregion singleton

        string num_fond;
        Parametres.enum_fond type_fond;

        public MapInfo Cadre = null;
        public MapInfo Fond = null;

        public class MapInfo
        {
            public MapInfo(Texture2D i_texture, Vector2 i_position)
            {
                texture = i_texture;
                position = i_position;
            }

            public Vector2 Position
            {
                get { return position; }
                set { position = value; }
            }
            private Vector2 position;

            public Texture2D Texture
            {
                get { return texture; }
                set { texture = value; }
            }
            private Texture2D texture;

            public float Speed
            {
                get { return speed; }
                set { speed = value; }
            }
            private float speed = 0.0f;
        }

        public void LoadContent()
        {
            num_fond = GameScreen.parametres.num_design;
            type_fond = GameScreen.parametres.type_fond;

            Texture2D texture_cadre = ScreenManager.Instance.Content.Load<Texture2D>("Graphics/cadre");
            Texture2D texture_fond = ScreenManager.Instance.Content.Load<Texture2D>("Graphics/Tilesheets/" + type_fond + "_" + num_fond);

            Cadre = new MapInfo(texture_cadre, Vector2.Zero);
            Fond = new MapInfo(texture_fond, Vector2.Zero);

            Fond.Speed = -0.2f;
        }

        public void UnloadContent()
        {
            GameScreen.parametres.num_design = num_fond;
            GameScreen.parametres.type_fond = type_fond;
        }

        public void Update()
        {
            Fond.Position = new Vector2(Fond.Position.X + Fond.Speed, Fond.Position.Y);

            if (Fond.Position.X <= -Fond.Texture.Width)
                Fond.Position = new Vector2(0, Fond.Position.Y);
        }

        public void ChangeDesignFond(string i_num_fond)
        {
            num_fond = i_num_fond;
            GameScreen.parametres.num_design = num_fond;
            Texture2D texture_fond = ScreenManager.Instance.Content.Load<Texture2D>("Graphics/Tilesheets/" + type_fond + "_" + num_fond);

            Fond = new MapInfo(texture_fond, Vector2.Zero);
            Fond.Speed = -0.2f;
        }

        public void ChangeTypeFond(string i_element)
        {
            Texture2D texture_fond;

            switch (i_element)
            {
                case "fire":
                    type_fond = Parametres.enum_fond.lave;
                    GameScreen.parametres.type_fond = type_fond;
                    texture_fond = ScreenManager.Instance.Content.Load<Texture2D>("Graphics/Tilesheets/" + type_fond + "_" + num_fond);
                    break;

                case "water":
                default:
                    type_fond = Parametres.enum_fond.eau;
                    GameScreen.parametres.type_fond = type_fond;
                    texture_fond = ScreenManager.Instance.Content.Load<Texture2D>("Graphics/Tilesheets/" + type_fond + "_" + num_fond);
                    break;
            }

            Fond = new MapInfo(texture_fond, Vector2.Zero);
            Fond.Speed = -0.2f;
        }
    }
}

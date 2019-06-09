using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TheMaze.Entities
{
    class Actor
    {
        #region singleton thread safe
        private static readonly Actor instance = new Actor();
        private Actor() { }
        static Actor()               //explicit static .ctor to tell C# compiler 
        {                           //not to mark type as beforefieldinit
        }
        public static Actor Instance
        {
            get { return instance; }
        }
        #endregion singleton

        public bool moving = false;
        public Platform platform;
        float position_sortie;

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }
        Texture2D image = null;

        public void LoadContent()
        {
            position_sortie = -1;

            image = ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Actor\actor");

            Actors actors = JsonHelper<Actors>.Read(@"json\actors.json");

            int num_level = LevelManager.Instance.levelIndex;
            platform = LevelManager.Instance.currentLevel.Plateform(actors[num_level]);
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if(LevelManager.Instance.currentLevel.victory == true && position_sortie == -1)
                position_sortie = LevelManager.Instance.offset_x + 2 * Tile.Width;
            else if(position_sortie > 0)
                position_sortie--;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float pos_x = LevelManager.Instance.offset_x + platform.position.X * Platform.Width + 2 * Tile.Width;
            float pos_y = LevelManager.Instance.offset_y + platform.position.Y * Platform.Height + 2 * Tile.Height;

            if (moving == false && !LevelManager.Instance.currentLevel.victory)
            {
                spriteBatch.Draw(   image, 
                                    new Vector2(pos_x, pos_y), 
                                    null, 
                                    Color.White, 
                                    0, 
                                    new Vector2(0, image.Height-Tile.Height), 
                                    1, 
                                    SpriteEffects.FlipHorizontally, 
                                    0);
            }

            else if(LevelManager.Instance.currentLevel.victory && position_sortie > 0)
            {
                spriteBatch.Draw(image,
                                new Vector2((float)Math.Floor(position_sortie), pos_y),
                                null,
                                Color.White,
                                0,
                                new Vector2(0, image.Height - Tile.Height),
                                1,
                                SpriteEffects.FlipHorizontally,
                                0);
            }
        }

        public void ChangePosition(Platform i_platform)
        {
            platform = i_platform;
        }
    }

    [DataContract]
    class Actors
    {
        [DataMember]
        public Vector2[] actors;

        public Vector2 this[int index]
        {
            get { return actors[index]; }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using TheMaze.Entities;

namespace TheMaze
{
    public class MapManager
    {
        #region singleton thread safe
        private static readonly MapManager instance = new MapManager();
        private MapManager() { }
        static MapManager()               //explicit static .ctor to tell C# compiler 
        {                                    //not to mark type as beforefieldinit
        }
        public static MapManager Instance
        {
            get { return instance; }
        }
        #endregion singleton

        public ContentManager Content { get; private set; }

        public void LoadContent()
        {
            Map.Instance.LoadContent();
        }

        public void UnloadContent()
        {
            Map.Instance.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (ScreenManager.Instance.Game.IsActive == false) return;

            Map.Instance.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Map.Instance.Fond.Texture, Map.Instance.Fond.Position, Color.White);
            if (Map.Instance.Fond.Position.X < 0)
                spriteBatch.Draw(Map.Instance.Fond.Texture, new Vector2(Map.Instance.Fond.Position.X + Map.Instance.Fond.Texture.Width, 0), Color.White);

            spriteBatch.Draw(Map.Instance.Cadre.Texture, Map.Instance.Cadre.Position, Color.White);
        }
    }
}

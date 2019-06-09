using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using TheMaze.Entities;

namespace TheMaze
{
    public class LevelManager
    {
        #region singleton thread safe
        private static readonly LevelManager instance = new LevelManager();
        private LevelManager() { }
        static LevelManager()               //explicit static .ctor to tell C# compiler 
        {                                    //not to mark type as beforefieldinit
            instance.levelIndex = 0;
        }
        public static LevelManager Instance
        {
            get { return instance; }
        }
        #endregion singleton

        public GraphicsDevice GraphicsDevice { get; private set; }
        public Texture2D tileset;
        public Texture2D exit;

        private string tileset_name = "Graphics/Tilesheets/tilesheet_" + GameScreen.parametres.num_design;

        public int offset_x = 32;
        public int offset_y = 2 * 32;

        public readonly int DIM_CARRE = 3;

        public int levelIndex;
        public Level currentLevel;

        public void LoadContent()
        {
            tileset = ScreenManager.Instance.Content.Load<Texture2D>(tileset_name);
            exit = ScreenManager.Instance.Content.Load<Texture2D>("Graphics/exit");

            currentLevel = new Level();
            currentLevel.LoadContent();
        }

        public void UnloadContent()
        {
            currentLevel.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (ScreenManager.Instance.Game.IsActive == false) return;

            currentLevel.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentLevel.Draw(spriteBatch);
        }


        public void ChangeDesign()
        {
            if (tileset.Name == "Graphics/Tilesheets/tilesheet_0")
            {
                tileset_name = "Graphics/Tilesheets/tilesheet_1";
                GameScreen.parametres.num_design = "1";
                Map.Instance.ChangeDesignFond("1");
            }
            else if (tileset.Name == "Graphics/Tilesheets/tilesheet_1")
            {
                tileset_name = "Graphics/Tilesheets/tilesheet_2";
                GameScreen.parametres.num_design = "2";
                Map.Instance.ChangeDesignFond("2");
            }
            else if (tileset.Name == "Graphics/Tilesheets/tilesheet_2")
            {
                tileset_name = "Graphics/Tilesheets/tilesheet_3";
                GameScreen.parametres.num_design = "3";
                Map.Instance.ChangeDesignFond("3");
            }
            else
            {
                tileset_name = "Graphics/Tilesheets/tilesheet_0";
                GameScreen.parametres.num_design = "0";
                Map.Instance.ChangeDesignFond("0");
            }

            tileset = ScreenManager.Instance.Content.Load<Texture2D>(tileset_name);
        }

        public void NextLevel()
        {
            if(levelIndex == Levels.NBR_LEVELS-1 && currentLevel.victory == true)
            {
                ScreenManager.Instance.NextScreen();
                levelIndex = 0;
            }
            else if(levelIndex < Levels.NBR_LEVELS-1)
            {
                UnloadContent();

                levelIndex = levelIndex + 1;
                LoadContent();

                MapManager.Instance.LoadContent();
                Actor.Instance.LoadContent();
            }
        }

        public void PreviousLevel()
        {
            if (levelIndex > 0)
            {
                UnloadContent();

                levelIndex = levelIndex - 1;
                LoadContent();

                MapManager.Instance.LoadContent();
                Actor.Instance.LoadContent();
            }
        }
    }
}

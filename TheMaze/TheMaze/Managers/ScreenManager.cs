using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TheMaze
{
    public class ScreenManager
    {
        #region singleton thread safe
        private static readonly ScreenManager instance = new ScreenManager();
        private ScreenManager() { }
        static ScreenManager()               //explicit static .ctor to tell C# compiler 
        {                                    //not to mark type as beforefieldinit
        }
        public static ScreenManager Instance
        {
            get { return instance; }
        }
        #endregion singleton

        public Vector2 Dimension { get; private set; }
        public ContentManager Content { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Game Game { get; private set; }

        List<GameScreen> Screens;
        int screenIndex;
        public GameScreen currentScreen;

        public SpriteFont sprite_font;

        public void Initialize(Game i_game)
        {
            Game = i_game;
            GraphicsDevice = i_game.GraphicsDevice;

            Dimension = new Vector2(544, 576);
            Screens = new List<GameScreen>() {  new SplashScreen(),
                                                new MenuScreen(),
                                                new LevelScreen(GraphicsDevice),
                                                new EndScreen() };

            screenIndex = 0;
            currentScreen = Screens[screenIndex];
        }

        public void NextScreen()
        {
            currentScreen.UnloadContent();

            screenIndex = screenIndex + 1;
            currentScreen = Screens[screenIndex];

            currentScreen.LoadContent();
        }

        public void PreviousScreen()
        {
            currentScreen.UnloadContent();

            screenIndex = screenIndex - 1;
            currentScreen = Screens[screenIndex];

            currentScreen.LoadContent();
        }

        public void FirstScreen()
        {
            currentScreen.UnloadContent();

            screenIndex = 0;
            currentScreen = Screens[screenIndex];

            currentScreen.LoadContent();
        }

        //test si la souris est dans la surface de l'application
        public bool EnterScreen()
        {
            bool ret = false;
            MouseState current_state = Mouse.GetState();

            Rectangle mouse_rect = new Rectangle(current_state.X, current_state.Y, 1, 1);
            Rectangle screen_rect = new Rectangle(0, 0, (int)Dimension.X, (int)Dimension.Y);
            if (mouse_rect.Intersects(screen_rect))
                return true;

            return ret;
        }

        public void LoadContent(ContentManager content)
        {
            this.Content = new ContentManager(content.ServiceProvider, "content");

            sprite_font = Content.Load<SpriteFont>(@"Fonts\moves");

            currentScreen.LoadContent();
        }

        public void UnloadContent()
        {
            SoundHelper.Instance.Dispose();

            currentScreen.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (Game.IsActive == false) return;

            currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }
    }
}

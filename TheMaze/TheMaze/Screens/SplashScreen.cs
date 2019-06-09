using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace TheMaze
{
    public class SplashScreen : GameScreen
    {
        Texture2D image;
        string path = "Content/Graphics/title.png";

        MouseState previous_state;
        MouseState current_state;

        public override void LoadContent()
        {
            base.LoadContent();

            using (FileStream stream = new FileStream(path, FileMode.Open))
                image = Texture2D.FromStream(ScreenManager.Instance.GraphicsDevice, stream);

            previous_state = Mouse.GetState();
            current_state = previous_state;

            SoundHelper.Instance.ActiveClassic();
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            current_state = Mouse.GetState();
            if (current_state.LeftButton == ButtonState.Released &&
                previous_state.LeftButton == ButtonState.Pressed &&
                ScreenManager.Instance.EnterScreen())
            {
                ScreenManager.Instance.NextScreen();
            }
            previous_state = current_state;

            SoundHelper.Instance.Update(gameTime);
        }

        public override void Draw(SpriteBatch i_spriteBatch)
        {
            i_spriteBatch.Draw(image, new Vector2(ScreenManager.Instance.Dimension.X / 2 - image.Width / 2,
                                                  ScreenManager.Instance.Dimension.Y / 2 - image.Height / 2),
                                                  Color.White);
        }
    }
}

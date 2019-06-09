using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace TheMaze
{
    public class EndScreen : GameScreen
    {
        Texture2D image;
        string path = "Content/Graphics/portrait.png";

        MouseState previous_state;
        MouseState current_state;

        public override void LoadContent()
        {
            base.LoadContent();

            using (FileStream stream = new FileStream(path, FileMode.Open))
                image = Texture2D.FromStream(ScreenManager.Instance.GraphicsDevice, stream);

            SoundHelper.Instance.ActiveClassic();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            current_state = Mouse.GetState();
            if (current_state.LeftButton == ButtonState.Released &&
                previous_state.LeftButton == ButtonState.Pressed &&
                ScreenManager.Instance.EnterScreen())
            {
                ScreenManager.Instance.FirstScreen();
            }
            previous_state = current_state;

            SoundHelper.Instance.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, new Vector2(ScreenManager.Instance.Dimension.X / 2 - image.Width / 2,
                                                  ScreenManager.Instance.Dimension.Y / 3 - image.Height / 2),
                                                  Color.White);

            // Affichage des meilleurs scores
            string text = ">>> CONGRATULATION !!! <<<";
            Vector2 position_titre = new Vector2(ScreenManager.Instance.Dimension.X / 2, 2 * ScreenManager.Instance.Dimension.Y / 3);
            Vector2 origin = new Vector2(ScreenManager.Instance.sprite_font.MeasureString(text).X * 0.5f,
                                         ScreenManager.Instance.sprite_font.MeasureString(text).Y * 0.5f);

            spriteBatch.DrawString(ScreenManager.Instance.sprite_font, text, position_titre,
                                    Color.Teal, 0, origin, 2, SpriteEffects.None, 0);
        }
    }
}

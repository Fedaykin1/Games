using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Linq;
using TheMaze.Entities;

namespace TheMaze
{
    public class MenuScreen : GameScreen
    {
        Texture2D image;
        string path = @"Content\Graphics\portrait.png";
        
        int offset_y;

        MouseState previous_state;
        MouseState current_state;

        public static Levels levels;

        public override void LoadContent()
        {
            base.LoadContent();

            using (FileStream stream = new FileStream(path, FileMode.Open))
                image = Texture2D.FromStream(ScreenManager.Instance.GraphicsDevice, stream);

            previous_state = Mouse.GetState();
            current_state = previous_state;

            SoundHelper.Instance.ActiveClassic();

            levels = JsonHelper<Levels>.Read(@"json\levels.json");
            Levels.NBR_LEVELS = levels.levels.Count();
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, new Vector2(ScreenManager.Instance.Dimension.X / 2 - image.Width / 2,
                                                  ScreenManager.Instance.Dimension.Y / 3 - image.Height / 2),
                                                  Color.White);

            // Affichage des meilleurs scores
            string text = ">>> BEST SCORES <<<";
            Vector2 position_titre = new Vector2(ScreenManager.Instance.Dimension.X / 2, ScreenManager.Instance.Dimension.Y / 2);
            Vector2 origin = new Vector2(ScreenManager.Instance.sprite_font.MeasureString(text).X * 0.5f, 
                                         ScreenManager.Instance.sprite_font.MeasureString(text).Y * 0.5f);

            spriteBatch.DrawString( ScreenManager.Instance.sprite_font, text, position_titre, 
                                    Color.Teal, 0, origin, 2, SpriteEffects.None, 0);

            offset_y = 32;
            Vector2 position_scores;
            for (int index = 0; index <= Levels.NBR_LEVELS-1; index++)
            {
                text = "Level  {0} :  {1}";
                position_scores = new Vector2(ScreenManager.Instance.Dimension.X / 2, ScreenManager.Instance.Dimension.Y / 2 + offset_y);
                origin = new Vector2(ScreenManager.Instance.sprite_font.MeasureString(text).X * 0.5f, 
                                     ScreenManager.Instance.sprite_font.MeasureString(text).Y * 0.5f);
                string score;
                if(GameScreen.scores.ht_scores.ContainsKey(index))
                    score = string.Format("{0} {1}", GameScreen.scores.ht_scores[index].ToString(), " moves !");
                else
                    score = "not yet succeeded";

                spriteBatch.DrawString( ScreenManager.Instance.sprite_font,
                                        string.Format(text, index + 1, score), position_scores,
                                        Color.Teal, 0, origin, 1, SpriteEffects.None, 0);
                offset_y += 32;
            }
        }
    }
}

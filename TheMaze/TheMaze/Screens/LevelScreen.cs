using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using TheMaze.Entities;
using Microsoft.Xna.Framework.Audio;
using System.Runtime.Serialization;
using TheMaze.Buttons;
using System;

namespace TheMaze
{
    public class LevelScreen : GameScreen
    {
        public static GraphicsDevice GraphicsDevice { get; private set; }

        DesignButton design_button;
        FireButton fire_button;
        WaterButton water_button;
        NextButton next_button;
        PreviousButton previous_button;
        ReloadButton refresh_button;
        SoundButton sound_button;
        MusicButton music_button;
        MenuButton menu_button;

        Vector2 position_texte_score;
        Vector2 position_title;
        string info_bulle;

        public LevelScreen(GraphicsDevice i_graphicsDevice)
        {
            GraphicsDevice = i_graphicsDevice;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            info_bulle = "Your number of moves (min number of moves to succeed)";
            position_texte_score = new Vector2(16, 8);

            design_button = new DesignButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\design"), new Vector2(4 * 32, 0));
            fire_button = new FireButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\fire"), new Vector2(5 * 32, 0));
            water_button = new WaterButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\water"), new Vector2(6 * 32, 0));
            music_button = new MusicButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\music"), new Vector2(8 * 32, 0));
            sound_button = new SoundButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\sound"), new Vector2(9 * 32, 0));
            refresh_button = new ReloadButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\refresh"), new Vector2(11 * 32, 0));
            previous_button = new PreviousButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\previous"), new Vector2(13 * 32, 0));
            next_button = new NextButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\next"), new Vector2(14 * 32, 0));
            menu_button = new MenuButton(ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Buttons\menu"), new Vector2(16 * 32, 0));

            MapManager.Instance.LoadContent();
            LevelManager.Instance.LoadContent();
            Actor.Instance.LoadContent();

            InitializeMusic();
        }

        protected void InitializeMusic()
        {
            SoundHelper.Instance.etat_music = GameScreen.parametres.music;
            SoundHelper.Instance.etat_effect = GameScreen.parametres.effects;

            if (SoundHelper.Instance.etat_music == Parametres.EtatsMusic.sound_ambiance)
                SoundHelper.Instance.ActiveAmbiance();
            else if (SoundHelper.Instance.etat_music == Parametres.EtatsMusic.sound_classic)
                SoundHelper.Instance.PlayClassic();
            else
            {
                SoundHelper.Instance.sound_classic.Volume = 0;
                SoundHelper.Instance.VolumeOff();
            }

            if (SoundHelper.Instance.etat_effect == Parametres.EtatsEffet.actif)
                SoundHelper.Instance.EffectsOn();
            else
                SoundHelper.Instance.EffectsOff();
        }

        public override void UnloadContent()
        {
            MapManager.Instance.UnloadContent();
            LevelManager.Instance.UnloadContent();
            Actor.Instance.UnloadContent();

            GameScreen.parametres.music = SoundHelper.Instance.etat_music;
            GameScreen.parametres.effects = SoundHelper.Instance.etat_effect;

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            SoundHelper.Instance.Update(gameTime);

            MapManager.Instance.Update(gameTime);
            LevelManager.Instance.Update(gameTime);
            Actor.Instance.Update(gameTime);

            design_button.Update(gameTime);
            fire_button.Update(gameTime);
            water_button.Update(gameTime);
            next_button.Update(gameTime);
            previous_button.Update(gameTime);
            refresh_button.Update(gameTime);
            sound_button.Update(gameTime);
            music_button.Update(gameTime);
            menu_button.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            MapManager.Instance.Draw(spriteBatch);
            LevelManager.Instance.Draw(spriteBatch);
            Actor.Instance.Draw(spriteBatch);

            // Affichage des boutons
            design_button.Draw(spriteBatch);
            fire_button.Draw(spriteBatch);
            water_button.Draw(spriteBatch);
            next_button.Draw(spriteBatch);
            previous_button.Draw(spriteBatch);
            refresh_button.Draw(spriteBatch);
            sound_button.Draw(spriteBatch);
            music_button.Draw(spriteBatch);
            menu_button.Draw(spriteBatch);

            // Affichage du score
            int current_moves = LevelManager.Instance.currentLevel.moves;
            int min_moves = LevelManager.Instance.currentLevel.min_moves;

            spriteBatch.DrawString( ScreenManager.Instance.sprite_font, 
                                    string.Format("{0} ({1})", current_moves, min_moves), 
                                    position_texte_score, Color.Black, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

            // info-bulles sur le score
            MouseState current_state = Mouse.GetState();
            Rectangle mouse_rect = new Rectangle(current_state.X, current_state.Y, 1, 1);
            Rectangle score_rect = new Rectangle(0, 0, 64, 32);
            if (info_bulle.Length > 0 && mouse_rect.Intersects(score_rect))
            {
                spriteBatch.DrawString(ScreenManager.Instance.sprite_font, info_bulle, new Vector2(0, 1.3f * 32), Color.White);
            }

            // Affichage de l'information du niveau (sur le coté)
            string level = string.Format("LEVEL {0}", LevelManager.Instance.levelIndex + 1);
            int position_y = 2 * Tile.Height;
            foreach (char c in level)
            {
                float scale = 1;
                Color color = Color.LightGray;
                if (System.Char.IsDigit(c))
                {
                    scale = 2;

                    if(MenuScreen.levels.difficulty[LevelManager.Instance.levelIndex] == 0) color = Color.LightGreen;
                    else if (MenuScreen.levels.difficulty[LevelManager.Instance.levelIndex] == 1) color = Color.Orange;
                    else if (MenuScreen.levels.difficulty[LevelManager.Instance.levelIndex] == 2) color = Color.Red;
                    else if(MenuScreen.levels.difficulty[LevelManager.Instance.levelIndex] == 3) color = Color.Violet;
                    else if (MenuScreen.levels.difficulty[LevelManager.Instance.levelIndex] == 4) color = Color.Aqua;
                    else color = Color.White;
                }

                position_title = new Vector2(ScreenManager.Instance.Dimension.X - 20, position_y);
                spriteBatch.DrawString( ScreenManager.Instance.sprite_font, 
                                        c.ToString(), position_title, color, 0, 
                                        Vector2.Zero, scale, SpriteEffects.None, 0);

                position_y += Tile.Height;
            }

            base.Draw(spriteBatch);
        }
    }
}

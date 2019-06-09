using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TheMaze.Entities
{
    public class Level
    {
        public Level()
        {
            int num_level = LevelManager.Instance.levelIndex;

            min_moves = MenuScreen.levels.min_moves[num_level];

            platforms = new List<Platform>();
            for (int column = 0; column < 3; column++)
            {
                for (int line = 0; line < 3; line++)
                {
                    if (MenuScreen.levels[num_level][line][column] == -1)
                    {
                        hole_position = new Vector2(column, line);
                        continue;
                    }

                    Platform platform = new Platform(MenuScreen.levels[num_level][line][column], line, column);
                    platforms.Add(platform);
                }
            }
        }

        List<Platform> platforms;
        MouseState previous_state;
        MouseState current_state;
        public Vector2 hole_position;
        Texture2D image_victory;
        Texture2D image_cursor;
        Vector2 image_cursor_position;
        float rotation = (float)Math.PI / 5;
        float vitesse = 0.04f;
        float scale;
        public int moves = 0;
        public bool victory;
        public int min_moves;

        public void LoadContent()
        {
            victory = false;

            image_victory = ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\victory");
        }

        public void UnloadContent()
        {
            if(victory)
            {
                // on stock le score
                if (LevelManager.Instance.currentLevel.victory == true)
                {
                    int level_num = LevelManager.Instance.levelIndex;

                    if (GameScreen.scores.ht_scores.ContainsKey(level_num) &&
                        (int)GameScreen.scores.ht_scores[level_num] > LevelManager.Instance.currentLevel.moves)
                    {
                        GameScreen.scores.ht_scores.Remove(level_num);
                    }

                    if(!GameScreen.scores.ht_scores.ContainsKey(level_num))
                        GameScreen.scores.ht_scores.Add(level_num, LevelManager.Instance.currentLevel.moves);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if(victory)
            {
                current_state = Mouse.GetState();
                if (current_state.LeftButton == ButtonState.Released &&
                    previous_state.LeftButton == ButtonState.Pressed &&
                ScreenManager.Instance.EnterScreen())
                {
                    LevelManager.Instance.NextLevel();
                }
                previous_state = current_state;

                return;
            }

            current_state = Mouse.GetState();
            Platform clicked_platform = null;

            if (current_state.LeftButton == ButtonState.Pressed &&
                previous_state.LeftButton == ButtonState.Released)
            {
                foreach (Platform platform in platforms)
                {
                    if (platform.isClicked())
                    {
                        if (platform == Actor.Instance.platform) // drag
                        {
                            Actor.Instance.moving = true;
                            HighlightAccessiblePlatforms(true, platform);
                            break;
                        }
                        else
                        {
                            clicked_platform = platform;
                            break;
                        }
                    }
                }
            }

            else if (current_state.LeftButton == ButtonState.Released &&
                previous_state.LeftButton == ButtonState.Pressed)
            {
                if (Actor.Instance.moving == true)
                {
                    foreach (Platform platform in platforms)
                    {
                        if (platform.isClicked()) // drop
                        {
                            if (platform.highlighted == true && platform.position != Actor.Instance.platform.position)
                            {
                                Actor.Instance.ChangePosition(platform);
                                moves += platform.cost;
                                SoundHelper.Instance.PlayPlatformEffect();
                            }

                            if(Actor.Instance.platform.floor == true)
                            {
                                SoundHelper.Instance.PlayVictoryEffect();
                                moves++; // sortie
                                victory = true;
                            }
                            break;
                        }
                    }
                    Actor.Instance.moving = false;
                    HighlightAccessiblePlatforms(false);
                }
            }

            previous_state = current_state;

            if (Actor.Instance.moving == true)
            {
                image_cursor_position.X = Mouse.GetState().X;
                image_cursor_position.Y = Mouse.GetState().Y;
                
                image_cursor = ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\Actor\actor");
                scale = 1.3f;
                rotation += vitesse;

                if (rotation >= Math.PI/4 || rotation <= -Math.PI/4)
                    vitesse = -vitesse;
            }
            else
            {
                image_cursor_position = Vector2.Zero;
                image_cursor = null;
                rotation = 0;
                scale = 1;
            }

            if (clicked_platform != null)
            {
                ChangePosition(clicked_platform);
            }

            foreach (Platform platform in platforms)
            {
                platform.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Platform platform in platforms)
            {
                platform.Draw(spriteBatch);
            }

            if (image_cursor != null)
            {
                spriteBatch.Draw(   image_cursor,
                                    image_cursor_position,
                                    null,
                                    Color.White,
                                    rotation,
                                    new Vector2(image_cursor.Width/2, 0),
                                    scale,
                                    SpriteEffects.None,
                                    1);
            }

            if (victory)
            {
                spriteBatch.Draw(image_victory, new Vector2(ScreenManager.Instance.Dimension.X / 2 - image_victory.Width / 2,
                                                      ScreenManager.Instance.Dimension.Y / 2 - image_victory.Height / 2),
                                                      Color.White);
            }
        }

        public Platform Plateform(Vector2 i_position) // line/column
        {
            foreach (Platform platform in platforms)
            {
                if(platform.position == i_position)
                    return platform;
            }

            return null;
        }

        public void ChangePosition( Platform i_plateform)
        {
            if (CanMoveUp(i_plateform))
            {
                SoundHelper.Instance.PlayPlatformEffect();
                i_plateform.ChangePosition(0, -1);
                hole_position.Y += 1;
                moves++;
            }
            else if (CanMoveDown(i_plateform))
            {
                SoundHelper.Instance.PlayPlatformEffect();
                i_plateform.ChangePosition(0, 1);
                hole_position.Y -= 1;
                moves++;
            }
            else if (CanMoveRight(i_plateform))
            {
                SoundHelper.Instance.PlayPlatformEffect();
                i_plateform.ChangePosition(1, 0);
                hole_position.X -= 1;
                moves++;
            }
            else if (CanMoveLeft(i_plateform))
            {
                SoundHelper.Instance.PlayPlatformEffect();
                i_plateform.ChangePosition(-1, 0);
                hole_position.X += 1;
                moves++;
            }
        }

        bool CanMoveUp(Platform i_plateform)
        {
            if(i_plateform.position.X == hole_position.X &&
                i_plateform.position.Y - 1 == hole_position.Y) 
                return true;

            return false;
        }

        bool CanMoveDown(Platform i_plateform)
        {
            if (i_plateform.position.X == hole_position.X &&
                i_plateform.position.Y + 1 == hole_position.Y)
                return true;

            return false;
        }

        bool CanMoveRight(Platform i_plateform)
        {
            if (i_plateform.position.X + 1 == hole_position.X &&
                i_plateform.position.Y == hole_position.Y)
                return true;

            return false;
        }

        bool CanMoveLeft(Platform i_plateform)
        {
            if (i_plateform.position.X - 1 == hole_position.X &&
                i_plateform.position.Y == hole_position.Y)
                return true;

            return false;
        }

        void HighlightAccessiblePlatforms(bool i_highlight, Platform i_platform = null)
        {
            List<Platform> platforms;
            if (i_highlight == true)
                platforms = PathFinder.Instance.AvailablePlatforms(i_platform);
            else
                platforms = this.platforms;

            foreach (Platform platform in platforms)
            {
                platform.highlighted = i_highlight;
                if (i_highlight == false) platform.cost = 0;

            }
        }
    }

    [DataContract]
    public class Levels
    {
        public static int NBR_LEVELS;
        
        [DataMember]
        public int[] min_moves;

        [DataMember]
        public int[] difficulty;

        [DataMember]
        public int[][][] levels;

        public int[][] this[int index]
        {
            get { return levels[index]; }
        }

    }
}

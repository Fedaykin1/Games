using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TheMaze.Entities
{
    public class Platform
    {
        public static int Width = 5 * 32;
        public static int Height = 5 * 32;
        public static Platforms platforms = JsonHelper<Platforms>.Read(@"json\platforms.json");

        public int numero;
        public Vector2 position;
        public Vector2 previous_position;
        private Tile[,] tiles = new Tile[5, 5];
        public bool floor = false;
        public bool highlighted = false;
        public int cost = 0;

        public Platform(int i_platform_num, int i_line, int i_column)
        {
            position = new Vector2(i_column, i_line );

            numero = Math.Abs(i_platform_num / 4);
            floor = platforms.floors.Contains(i_platform_num) ? true : false;

            int tile_x = (i_platform_num % 4) * Platform.Width;
            int tile_y = numero * Platform.Height;

            int pos_x = tile_x, pos_y = tile_y;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    tiles[x, y] = new Tile(new Vector2(pos_x, pos_y), new Vector2(x * Tile.Width, y * Tile.Height), this);
                    pos_x = pos_x + Tile.Width;
                }
                pos_y = pos_y + Tile.Height;
                pos_x = tile_x;
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int index1 = 0; index1 < 5; index1++)
            {
                for (int index2 = 0; index2 < 5; index2++)
                {
                    tiles[index1, index2].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index1 = 0; index1 < 5; index1++)
            {
                for (int index2 = 0; index2 < 5; index2++)
                {
                    // gestion du highlight
                    if (index1 == 2 && index2 == 2)
                    {
                        if (this.highlighted == true)
                            tiles[index1, index2].highlighted = true;
                        else
                            tiles[index1, index2].highlighted = false;
                    }

                    tiles[index1, index2].Draw(spriteBatch);
                }
            }
        }

        public bool isClicked()
        {
            MouseState current_state = Mouse.GetState();

            Rectangle mouse_rect = new Rectangle(current_state.X, current_state.Y, 1, 1);
            Rectangle platform = new Rectangle(LevelManager.Instance.offset_x + (int)position.X * Width,
                                                LevelManager.Instance.offset_y + (int)Math.Abs(position.Y) * Height,
                                                Width, Height);

            if (mouse_rect.Intersects(platform))
                return true;

            return false;
        }

        public void ChangePosition(int dx, int dy)
        {
            previous_position = position;
            position.X += dx;
            position.Y += dy;
        }

        public TileCollision CanMoveUp()
        {
            return tiles[2, 0].collision;
        }

        public TileCollision CanMoveDown()
        {
            return tiles[2, 4].collision;
        }

        public TileCollision CanMoveRight()
        {
            return tiles[4, 2].collision;
        }

        public TileCollision CanMoveLeft()
        {
            return tiles[0, 2].collision;
        }
    }


    [DataContract]
    public class Platforms
    {
        [DataMember]
        public List<int> floors;

        [DataMember]
        public int[][][] platforms;

        public int[][] this[int index]
        {
            get { return platforms[index]; }
        }
    }
}

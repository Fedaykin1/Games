using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMaze.Entities
{
    public enum TileCollision
    {
        Passable = 0,
        Impassable = 1,
        Escalier = 2
    }

    class Tile
    {
        public static int Width = 32;
        public static int Height = 32;
        Platform platform;
        public bool highlighted = false;
        public Vector2 positionOnPlatform = Vector2.Zero;
        Vector2 positionOnTileset = Vector2.Zero;
        public TileCollision collision = TileCollision.Passable;

        protected int delta_x, delta_y; // pour le deplacement random de la tuile

        protected int pos_origin_x, pos_origin_y;
        protected int pos_destination_x, pos_destination_y;
        float rotation;

        public Tile(Vector2 i_posOnTileSheet, Vector2 i_posOnPlatform, Platform i_plateform)
        {
            positionOnPlatform = i_posOnPlatform;
            positionOnTileset = i_posOnTileSheet;

            platform = i_plateform;

            int line = (int)positionOnPlatform.Y / Height;
            int column = (int)positionOnPlatform.X / Width;
            int numero_on_tilesheet = (int)(positionOnTileset.X / Platform.Width) + (int)(positionOnTileset.Y / Platform.Height) * 4;

            int collision_type = Platform.platforms[numero_on_tilesheet][line][column];
            if ( collision_type == 1)
                collision = TileCollision.Impassable;
            else if (collision_type == 2)
                collision = TileCollision.Escalier;

            pos_origin_x = 0;
            pos_origin_y = 0;

            pos_destination_x = (int)(LevelManager.Instance.offset_x + platform.position.X * Platform.Width + positionOnPlatform.X);
            pos_destination_y = (int)(LevelManager.Instance.offset_y + platform.position.Y * Platform.Height + positionOnPlatform.Y);
        }

        public void Update(GameTime gameTime)
        {
            int tuile = (int)(positionOnPlatform.X / 32) % 5 + (int)(positionOnPlatform.Y / 32) * 5;

            // algo pour deplacement graduel des tuiles
            Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            pos_destination_x = (int)(LevelManager.Instance.offset_x + platform.position.X * Platform.Width + positionOnPlatform.X);
            pos_destination_y = (int)(LevelManager.Instance.offset_y + platform.position.Y * Platform.Height + positionOnPlatform.Y);

            if (pos_destination_x > pos_origin_x)
            {
                delta_x = rand.Next(1, ((tuile + 1) % 10 + 1) * 10);
            }
            else if (pos_destination_x < pos_origin_x)
            {
                delta_x = -rand.Next(1, ((tuile + 1) % 10 +1) * 10);
            }

            if (pos_destination_y > pos_origin_y)
            {
                delta_y = rand.Next(1, ((tuile + 1) % 10 + 1) * 10);
            }
            else if (pos_destination_y < pos_origin_y)
            {
                delta_y = -rand.Next(1, ((tuile + 1) % 10 + 1) * 10);
            }

            if (pos_origin_x != pos_destination_x)
            {
                pos_origin_x = pos_origin_x + delta_x;

                if ((delta_x < 0 && pos_origin_x < pos_destination_x) ||
                    (delta_x > 0 && pos_origin_x > pos_destination_x))
                {
                    pos_origin_x = pos_destination_x;
                }
            }

            if (pos_origin_y != pos_destination_y)
            {
                pos_origin_y = pos_origin_y + delta_y;
                if ((delta_y < 0 && pos_origin_y < pos_destination_y) ||
                    (delta_y > 0 && pos_origin_y > pos_destination_y))
                {
                    pos_origin_y = pos_destination_y;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = LevelManager.Instance.tileset;
            Rectangle dest_rect = new Rectangle(pos_origin_x, pos_origin_y, Width, Height);
            Rectangle? source_rect = new Rectangle((int)positionOnTileset.X, (int)positionOnTileset.Y, Width, Height);
            Color color = Color.White;
            Vector2 origin = Vector2.Zero;

            spriteBatch.Draw(texture, dest_rect, source_rect, color, 0, origin, SpriteEffects.None, 0);

            // highlight les tuiles centrales si pathfinder
            if (highlighted == true)
            {
                color = Color.Gold;

                if (platform.floor == true)
                {
                    texture = ScreenManager.Instance.Content.Load<Texture2D>(@"Graphics\exit");
                    dest_rect = new Rectangle(pos_origin_x + Height/2, pos_origin_y + Width / 2, Width-5, Height-5);
                    source_rect = new Rectangle(0, 0, Width, Height);
                    origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    rotation += 0.1f; 
                }
                else
                {
                    origin = Vector2.Zero;
                    rotation = 0;
                }

                spriteBatch.Draw(texture, dest_rect, source_rect, color, rotation, origin, SpriteEffects.None, 0);
            }
        }
    }
}
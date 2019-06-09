using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMaze.Buttons
{
    public abstract class Button
    {
        Texture2D texture;
        Vector2 position;
        string info_bulle;
        protected MouseState current_state;
        protected MouseState previous_state;
        protected float offset = -20;
        Rectangle button;

        public object MouseInput { get; private set; }

        public Button( Texture2D i_texture, Vector2 i_position, string i_info_bulle = "")
        {
            texture = i_texture;
            position = i_position;
            info_bulle = i_info_bulle;

            button = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public bool EnterButton()
        {
            bool ret = false;
            current_state = Mouse.GetState();

            Rectangle mouse_rect = new Rectangle(current_state.X, current_state.Y, 1, 1);
            if (mouse_rect.Intersects(button))
                return true;

            return ret;
        }

        public abstract void Update(GameTime gameTime);

        public void Draw(SpriteBatch i_spriteBatc)
        {
            // icones
            i_spriteBatc.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.White);

            // info-bulles
            current_state = Mouse.GetState();
            Rectangle mouse_rect = new Rectangle(current_state.X, current_state.Y, 1, 1);
            if (info_bulle.Length > 0 && mouse_rect.Intersects(button))
            {
                i_spriteBatc.DrawString(ScreenManager.Instance.sprite_font, info_bulle, new Vector2(position.X + offset, position.Y + 1.3f * texture.Height), Color.White);
            }
        }
    }
}

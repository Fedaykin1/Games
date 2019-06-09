using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheMaze.Entities;

namespace TheMaze.Buttons
{
    class MenuButton : Button
    {
        static string info_bulle = "Menu screen";

        public MenuButton(Texture2D i_texture, Vector2 i_position) : base(i_texture, i_position, info_bulle)
        {
            offset = -90;
        }

        public override void Update(GameTime gameTime)
        {
            current_state = Mouse.GetState();

            if (EnterButton() &&
                current_state.LeftButton == ButtonState.Released &&
                previous_state.LeftButton == ButtonState.Pressed)
            {
                ScreenManager.Instance.FirstScreen();
            }
            previous_state = current_state;
        }

    }
}

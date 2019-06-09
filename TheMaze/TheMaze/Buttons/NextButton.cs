using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheMaze.Buttons
{
    class NextButton : Button
    {
        static string info_bulle = "Next level !";

        public NextButton(Texture2D i_texture, Vector2 i_position) : base(i_texture, i_position, info_bulle)
        {
            offset = -40;
        }

        public override void Update(GameTime gameTime)
        {
            current_state = Mouse.GetState();

            if (EnterButton() &&
                current_state.LeftButton == ButtonState.Pressed &&
                previous_state.LeftButton == ButtonState.Released)
            {
                LevelManager.Instance.NextLevel();
            }
            previous_state = current_state;
        }

    }
}

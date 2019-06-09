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
    class SoundButton : Button
    {
        static string info_bulle = "Turn on/off sound effects !";

        public SoundButton(Texture2D i_texture, Vector2 i_position) : base(i_texture, i_position, info_bulle)
        {
            offset = -80;
        }

        public override void Update(GameTime gameTime)
        {
            current_state = Mouse.GetState();

            if (EnterButton() &&
                current_state.LeftButton == ButtonState.Pressed &&
                previous_state.LeftButton == ButtonState.Released)
            {
                if (SoundHelper.Instance.isEffectsOn())
                {
                    SoundHelper.Instance.EffectsOff();
                }
                else
                {
                    SoundHelper.Instance.EffectsOn();
                }
            }

            previous_state = current_state;
        }

    }
}

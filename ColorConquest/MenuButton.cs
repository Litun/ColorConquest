using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace ColorConquest
{
    class MenuButton: GeneralButton
    {
        public MenuButton(Game game, float floatY, float floatWidth, ButtonClick bClick, Texture2D[] textures)
            : base(game, new Rectangle(), bClick, textures)
        {
            int x, y, w, h;
            w = (int)(GraphicsDevice.Viewport.Width *floatWidth);
            h = w * textures[0].Height / textures[0].Width;
            x = (GraphicsDevice.Viewport.Width - w) / 2;
            y = (int)(GraphicsDevice.Viewport.Height * floatY);
            rectangle = new Rectangle(x, y, w, h);
        }
    }
}

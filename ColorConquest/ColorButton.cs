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
    enum States
    {
        Nothing,
        Hover,
        Click,
        UnTouch
    }

    public delegate void ColorButtonClick(int i);
    public delegate void ButtonClick();

    public class ColorButton: DrawableGameComponent
    {
       public  bool isActive = false;
        Texture2D[] textures;
        Rectangle rectangle;

        States state;
        ColorButtonClick buttonClick;
        int colorIndex;


       public  ColorButton(Game game, float x, float y, ColorButtonClick bClick, int colorIndex)
            : base(game)
        {
            this.colorIndex = colorIndex;
            buttonClick = bClick;

            textures = new Texture2D[4];
            LoadContent();
            rectangle = new Rectangle((int)(x * GraphicsDevice.Viewport.Width), (int)(y *1.0125* GraphicsDevice.Viewport.Height),
                GraphicsDevice.Viewport.Width/10, GraphicsDevice.Viewport.Height/10);
        }

       protected override void LoadContent()
        {
            
            textures[0] = Game.Content.Load<Texture2D>(colorIndex+"-1");
            textures[1] = Game.Content.Load<Texture2D>(colorIndex + "-2");
            textures[2] = Game.Content.Load<Texture2D>(colorIndex + "-3");
            textures[3] = Game.Content.Load<Texture2D>("untouch");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (isActive)
            {
                if (state != States.UnTouch && rectangle.Contains(mouseState.X, mouseState.Y))
                {
                    if (state == States.Hover && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        buttonClick(colorIndex);
                        state = States.Click;
                    }
                    else if (mouseState.LeftButton == ButtonState.Released)
                        state = States.Hover;
                }
                else
                    state = States.Nothing;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (isActive)
            {
                Game1.spriteBatch.Begin();
                if (state == States.Nothing)
                    Game1.spriteBatch.Draw(textures[0], rectangle, Color.White);
                else if (state == States.Hover)
                    Game1.spriteBatch.Draw(textures[1], rectangle, Color.White);
                else if (state == States.Click)
                    Game1.spriteBatch.Draw(textures[2], rectangle, Color.White);
                else
                    Game1.spriteBatch.Draw(textures[3], rectangle, Color.White);
                Game1.spriteBatch.End();
            }

            base.Draw(gameTime);
                
        }
    }
}

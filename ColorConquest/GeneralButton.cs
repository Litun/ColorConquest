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
    class GeneralButton : DrawableGameComponent
    {
        
       public  bool isActive = true;
        protected Texture2D[] textures;
        protected Rectangle rectangle;

        public SpriteBatch spriteBatch;
        States state;
        protected ButtonClick buttonClick;
        MouseState mouseState;
        States lastState;

       public  GeneralButton(Game game, Rectangle rectangle,ButtonClick bClick, Texture2D[] textures)
            : base(game)
        {
            this.textures = textures;
            buttonClick = bClick;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.rectangle = rectangle;
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                mouseState = Mouse.GetState();
                lastState = state;

                if (state != States.UnTouch && rectangle.Contains(mouseState.X, mouseState.Y))
                {
                    if (state == States.Hover && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        state = States.Click;
                    }
                    else if (mouseState.LeftButton == ButtonState.Released)
                        state = States.Hover;
                }
                else
                    state = States.Nothing;

                if (lastState == States.Click && state == States.Hover)
                    buttonClick();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (isActive)
            {
                spriteBatch.Begin();
                if (state == States.Nothing)
                    spriteBatch.Draw(textures[0], rectangle, Color.White);
                else if (state == States.Hover)
                    spriteBatch.Draw(textures[1], rectangle, Color.White);
                else if (state == States.Click)
                    spriteBatch.Draw(textures[2], rectangle, Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
                
        }
    }
}

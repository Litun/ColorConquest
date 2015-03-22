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
    class InfoScreen : DrawableGameComponent
    {
        Texture2D whiteSquare;

        Rectangle backRectangle;
        Vector2 frontVector2;
        Color colorSquare;

        bool result;
        Texture2D texture;

        public InfoScreen(Game game, bool res)
            : base(game)
        {
            result = res;
        }

        public override void Initialize()
        {
            colorSquare = new Color(0, 0, 0, 0);
            backRectangle = new Rectangle(0, 0, 2000, 2000);

            
            base.Initialize();
        }

        protected override void LoadContent()
        {

            whiteSquare = Game.Content.Load<Texture2D>("white");
            if (result)
                texture = Game.Content.Load<Texture2D>("Win");
            else
                texture = Game.Content.Load<Texture2D>("Lose");

            frontVector2 = new Vector2((GraphicsDevice.Viewport.Width - texture.Width) / 2, GraphicsDevice.Viewport.Height / 5);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (colorSquare.A < 180)
                colorSquare.A += 4;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            Game1.spriteBatch.Begin();
            Game1.spriteBatch.Draw(whiteSquare, backRectangle, colorSquare);
            Game1.spriteBatch.Draw(texture, frontVector2, Color.White);
            Game1.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

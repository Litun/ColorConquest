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
    class Menu: DrawableGameComponent
    {
        bool isActite = true;
        Texture2D whiteSquare;
        
        Rectangle rectangle;
        Color colorSquare;

        public MenuButton start, exit;

        public Menu(Game1 g)
            : base(g)
        {
        }

        void Start()
        {
            ((Game1)Game).field.isActive = true;
            for (int i = 0; i < ((Game1)Game).colorButtons.Length; i++)
            {
                ((Game1)Game).colorButtons[i].isActive = true;
            }
            isActite = false;
            start.isActive = false;
            exit.isActive = false;
            
        }

        public override void Initialize()
        {
            colorSquare = new Color(0, 0, 0, 180);
            rectangle = new Rectangle(0, 0,2000, 2000);

            Texture2D[] textures2d = new Texture2D[] { 
                Game.Content.Load<Texture2D>("Start-1"), 
                Game.Content.Load<Texture2D>("Start-2"), 
                Game.Content.Load<Texture2D>("Start-3") };
            start = new MenuButton(Game, 0.1f, 0.4f, new ButtonClick(Start), textures2d);

            textures2d = new Texture2D[]{
                Game.Content.Load<Texture2D>("Exit-1"),
                Game.Content.Load<Texture2D>("Exit-1"),
                Game.Content.Load<Texture2D>("Exit-2")};
            exit = new MenuButton(Game, 0.6f, 0.3f, new ButtonClick(Game.Exit), textures2d);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            whiteSquare = Game.Content.Load<Texture2D>("white");
            base.LoadContent();
            
        }

        public override void Update(GameTime gameTime)
        {
          //  if (((Game1)Game).IsActive)
          //      ((Game1)Game).Components.Remove(start);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (isActite)
            {
                Game1.spriteBatch.Begin();
                Game1.spriteBatch.Draw(whiteSquare, rectangle, colorSquare);
                Game1.spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace ColorConquest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static SpriteBatch spriteBatch;

        GraphicsDeviceManager graphics;

        public BasicEffect effect;
        // VertexBuffer buffer;

        public Field field;
        Camera camera;
        public ColorButton[] colorButtons;
        Menu menu;

        public Game1()
            : base()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);
            field = new Field(this);
            Components.Add(field);
            camera = new Camera(this, new Vector3(0, 0, 3f),
                Vector3.Zero, Vector3.Up);
            Components.Add(camera);

            colorButtons = new ColorButton[7];

            for (int i = 0; i < colorButtons.Length; i++)
            {
                colorButtons[i] = new ColorButton(this, 0.04f, 0.0625f + 0.125f * i, new ColorButtonClick(field.Go), i);
                Components.Add(colorButtons[i]);
            }

            menu = new Menu(this);
            Components.Add(menu);
            Components.Add(menu.start);
            Components.Add(menu.exit);

            //buffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 3, BufferUsage.WriteOnly);


            effect.VertexColorEnabled = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (field.end != 0)
            {
                for (int i = 0; i < colorButtons.Length; i++)
                    colorButtons[i].isActive = false;
                field.isActive = false;

                if (field.end == -1)
                    Components.Add(new InfoScreen(this, false));
                else if (field.end == 1)
                    Components.Add(new InfoScreen(this, true));
                field.end = 0;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here



            /*     foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                 {
                     pass.Apply();
                     GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                         PrimitiveType.TriangleList, field.array, 0, field.array.Length / 3);
                 }*/

            base.Draw(gameTime);
        }
    }
}

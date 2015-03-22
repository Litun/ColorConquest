using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace ColorConquest
{

    public class Camera : GameComponent
    {
        //Camera matrices
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            ((Game1)game).effect.View = view = Matrix.CreateLookAt(pos, target, up);

            ((Game1)game).effect.Projection = projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            game.GraphicsDevice.Viewport.AspectRatio,
            1.0f, 100.0f);
            
            
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        float rotX, rotY, rotZ;
        int xi, yi;
        MouseState mouse;
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

          /*  mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                rotX += -(float)(mouse.X - xi) / 100;
                rotY += -(float)(mouse.Y - yi) / 100;
                xi = mouse.X;
                yi = mouse.Y;
            }
            else
            {
                xi = mouse.X;
                yi = mouse.Y;
            }*/

           // ((Game1)Game).effect.View = view. Matrix.Identity * Matrix.CreateRotationX(rotY) * Matrix.CreateRotationY(-rotX) * Matrix.CreateRotationZ(rotZ);

            base.Update(gameTime);
        }
    }
}
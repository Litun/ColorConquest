using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ColorConquest
{
    public class Field: DrawableGameComponent
    {
        public  bool isActive = false;
        int[] numbers;
        Triangle[] triangles;
        int[][] graph;
        readonly Color[] colors;
        Random random;
        public VertexPositionColor[] array;
        VertexPositionColor[] startTriangle; 

        Vector3[] vec3;
        MouseState mouse;
        public int occupiedCount;
        List<Triangle> includedTriangles;
        SpriteFont font;
        Vector2 scorePosition;
        string score;
        public int step=0;
        int stepMax = 40;//44

//        public VertexPositionColor[] trianglesBorders;// херня

        public int end;

        public Field(Game g):base(g)
        {
            random = new Random();

            MakeFigure();

            colors = new Color[7] {
                new Color(66, 138, 206),//синий
                new Color(214, 60, 57),//розовый
                new Color(123, 81, 206),//красный
                new Color(107, 186, 82),//желтый
                new Color(231, 158, 24),//зеленый
                new Color(226, 131,196),//голубой
                new Color(193, 193, 193)
            };

            TrianglePartition();
            SecondTrianglePartition();
            Rounding();

            triangles = new Triangle[numbers.Length / 3];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = new Triangle(vec3[numbers[i * 3]], vec3[numbers[i * 3 + 1]], vec3[numbers[i * 3 + 2]], colors[random.Next(7)]);
            }

            Array.Sort(triangles);

            SetGraph();

            MakeIncludedList();

            MakeStartTriangle();

            MakeArray();

            scorePosition = new Vector2(GraphicsDevice.Viewport.Width / 60 * 49, GraphicsDevice.Viewport.Height / 5);

            
            /*
            #region границы треугольников, которые не работают
            trianglesBorders = new VertexPositionColor[array.Length * 2];
            for (int i = 0; i < array.Length; i += 3)
            {
                trianglesBorders[2 * i] = new VertexPositionColor(
                    new Vector3(array[i].Position.X * 1.01f, array[i].Position.Y * 1.01f, array[i].Position.Z * 1.01f), Color.Black);
                trianglesBorders[2 * i + 1] = new VertexPositionColor(
                    new Vector3(array[i + 1].Position.X * 1.01f, array[i + 1].Position.Y * 1.01f, array[i + 1].Position.Z * 1.01f), Color.Black);
                trianglesBorders[2 * i + 2] = new VertexPositionColor(
                    new Vector3(array[i + 1].Position.X * 1.01f, array[i + 1].Position.Y * 1.01f, array[i + 1].Position.Z * 1.01f), Color.Black);
                trianglesBorders[2 * i + 3] = new VertexPositionColor(
                    new Vector3(array[i + 2].Position.X * 1.01f, array[i + 2].Position.Y * 1.01f, array[i + 2].Position.Z * 1.01f), Color.Black);
                trianglesBorders[2 * i + 4] = new VertexPositionColor(
                    new Vector3(array[i + 2].Position.X * 1.01f, array[i + 2].Position.Y * 1.01f, array[i + 2].Position.Z * 1.01f), Color.Black);
                trianglesBorders[2 * i + 5] = new VertexPositionColor(
                    new Vector3(array[i].Position.X * 1.01f, array[i].Position.Y * 1.01f, array[i].Position.Z * 1.01f), Color.Black);
            };
            #endregion
            */
        }

        void MakeStartTriangle()
        {
            Vector3[] vecs = new Vector3[3];

            vecs[0] = (triangles[0].triangleVerts[0].Position + triangles[0].triangleVerts[1].Position) / 2;
            vecs[1] = (triangles[0].triangleVerts[1].Position + triangles[0].triangleVerts[2].Position) / 2;
            vecs[2] = (triangles[0].triangleVerts[2].Position + triangles[0].triangleVerts[0].Position) / 2;


            Vector3[] vecs2 = new Vector3[3]
            {
                (vecs[0]+vecs[1])/2,
                (vecs[1]+vecs[2])/2,
                (vecs[2]+vecs[0])/2
            };

            startTriangle = new VertexPositionColor[3]
            {
                new VertexPositionColor(vecs2[0], Color.Black),
                new VertexPositionColor(vecs2[1], Color.Black),
                new VertexPositionColor(vecs2[2], Color.Black)
            };
        }

        void MakeScoreString()
        {
            score = "Occupied:\n" + occupiedCount + " / " + triangles.Length +
                "\nSteps:\n" + step + " / " + stepMax;

        }

        void MakeIncludedList()
        {
            includedTriangles = new List<Triangle>();
            includedTriangles.Add(triangles[0]);

            int i=0;
            while (i < includedTriangles.Count)
            {
                foreach (int a in graph[includedTriangles[i].index])
                {
                    if (triangles[a].Color == includedTriangles[0].Color && !includedTriangles.Contains(triangles[a]))
                        includedTriangles.Add(triangles[a]);
                }

                i++;
            }

            occupiedCount = includedTriangles.Count;
            MakeScoreString();
        }

        void MakeFigure() 
        {
            numbers = new int[60] { 0,  2,1, 0, 3, 2, 0, 4, 3, 0, 5, 4, 0, 1, 5, 2, 7, 1, 3, 8, 2, 4, 9, 3, 5, 10, 4, 
                1, 6, 5, 6, 1, 7, 7, 2, 8, 8, 3, 9, 9, 4, 10, 10, 5, 6, 11, 6, 7, 11, 7, 8, 11, 8, 9, 11, 9, 10, 11, 10, 6 };

            float b = 0.85065080835f;
            float R = 0.95105654f;
            float r = b;

            vec3 = new Vector3[12]
            {
                new Vector3(0f, R, 0f),
                new Vector3(0f, b/2, r),
                new Vector3((float)(r*Math.Cos(Math.PI/10)), b/2, (float)(r*Math.Sin(Math.PI/10))),
                new Vector3((float)(r*Math.Sin(Math.PI/5)), b/2, (float)(-r*Math.Cos(Math.PI/5))),
                new Vector3((float)(-r*Math.Sin(Math.PI/5)), b/2, (float)(-r*Math.Cos(Math.PI/5))),
                new Vector3((float)(-r*Math.Cos(Math.PI/10)), b/2, (float)(r*Math.Sin(Math.PI/10))),
                
                new Vector3((float)(-r*Math.Sin(Math.PI/5)), -b/2, (float)(r*Math.Cos(Math.PI/5))),
                new Vector3((float)(r*Math.Sin(Math.PI/5)), -b/2, (float)(r*Math.Cos(Math.PI/5))),
                new Vector3((float)(r*Math.Cos(Math.PI/10)), -b/2, (float)(-r*Math.Sin(Math.PI/10))),
                new Vector3(0f, -b/2, -r),
                new Vector3((float)(-r*Math.Cos(Math.PI/10)), -b/2, (float)(-r*Math.Sin(Math.PI/10))),
                new Vector3(0f, -R, 0f)
            };
        }

        void MakeArray()
        {
            List<VertexPositionColor> list = new List<VertexPositionColor>();
            foreach (Triangle t in triangles)
            {
                list.AddRange(t.triangleVerts);
            }
            list.AddRange(startTriangle);
            array = list.ToArray();
        }

        void TrianglePartition()
        {
            List<int> numbersNew = new List<int>();
            List<int[]> a = new List<int[]>();
            List<Vector3> vec32 = new List<Vector3>();
            vec32 = vec3.ToList<Vector3>();
            int ind = 12;
            for (int i = 0; i < numbers.Length; i += 3)
            {
                int ai = 0, bi = 0, ci = 0;

                bool bo = true;
                foreach (int[] m in a)
                    if (m[0] == numbers[i] && m[1] == numbers[i + 1] || m[0] == numbers[i + 1] && m[1] == numbers[i])
                    {
                        bo = false;
                        ai = m[2];
                        break;
                    }

                if (bo)
                {
                    vec32.Add(new Vector3((vec3[numbers[i]].X + vec3[numbers[i + 1]].X) / 2,
                        (vec3[numbers[i]].Y + vec3[numbers[i + 1]].Y) / 2,
                        (vec3[numbers[i]].Z + vec3[numbers[i + 1]].Z) / 2));

                    ai = ind;
                    a.Add(new int[3] { numbers[i], numbers[i + 1], ind++ });
                }



                bo = true;
                foreach (int[] m in a)
                    if (m[0] == numbers[i + 2] && m[1] == numbers[i + 1] || m[0] == numbers[i + 1] && m[1] == numbers[i + 2])
                    {
                        bo = false;
                        bi = m[2];
                        break;
                    }

                if (bo)
                {
                    vec32.Add(new Vector3((vec3[numbers[i + 2]].X + vec3[numbers[i + 1]].X) / 2,
                        (vec3[numbers[i + 2]].Y + vec3[numbers[i + 1]].Y) / 2,
                        (vec3[numbers[i + 2]].Z + vec3[numbers[i + 1]].Z) / 2));

                    bi = ind;
                    a.Add(new int[3] { numbers[i + 2], numbers[i + 1], ind++ });
                }


                bo = true;
                foreach (int[] m in a)
                    if (m[0] == numbers[i] && m[1] == numbers[i + 2] || m[0] == numbers[i + 2] && m[1] == numbers[i])
                    {
                        bo = false;
                        ci = m[2];
                        break;
                    }

                if (bo)
                {
                    vec32.Add(new Vector3((vec3[numbers[i]].X + vec3[numbers[i + 2]].X) / 2,
                        (vec3[numbers[i]].Y + vec3[numbers[i + 2]].Y) / 2,
                        (vec3[numbers[i]].Z + vec3[numbers[i + 2]].Z) / 2));

                    ci = ind;
                    a.Add(new int[3] { numbers[i], numbers[i + 2], ind++ });
                }

                numbersNew.Add(numbers[i]);
                numbersNew.Add(ai);
                numbersNew.Add(ci);

                numbersNew.Add(numbers[i + 1]);
                numbersNew.Add(bi);
                numbersNew.Add(ai);

                numbersNew.Add(numbers[i + 2]);
                numbersNew.Add(ci);
                numbersNew.Add(bi);

                numbersNew.Add(ai);
                numbersNew.Add(bi);
                numbersNew.Add(ci);
            }
            numbers = numbersNew.ToArray();
            vec3 = vec32.ToArray();
        }

        void SecondTrianglePartition()
        {
            List<int> numbersNew = new List<int>();
            List<int[]> a = new List<int[]>();
            List<Vector3> vec32 = new List<Vector3>();
            vec32 = vec3.ToList<Vector3>();
            int ind = vec3.Length;
            for (int i = 0; i < numbers.Length; i += 3)
            {
                int ai = 0, bi = 0, ci = 0;

                bool bo = true;
                foreach (int[] m in a)
                    if (m[0] == numbers[i] && m[1] == numbers[i + 1] || m[0] == numbers[i + 1] && m[1] == numbers[i])
                    {
                        bo = false;
                        ai = m[2];
                        break;
                    }

                if (bo)
                {
                    vec32.Add(new Vector3((vec3[numbers[i]].X + vec3[numbers[i + 1]].X) / 2,
                        (vec3[numbers[i]].Y + vec3[numbers[i + 1]].Y) / 2,
                        (vec3[numbers[i]].Z + vec3[numbers[i + 1]].Z) / 2));

                    ai = ind;
                    a.Add(new int[3] { numbers[i], numbers[i + 1], ind++ });
                }



                bo = true;
                foreach (int[] m in a)
                    if (m[0] == numbers[i + 2] && m[1] == numbers[i + 1] || m[0] == numbers[i + 1] && m[1] == numbers[i + 2])
                    {
                        bo = false;
                        bi = m[2];
                        break;
                    }

                if (bo)
                {
                    vec32.Add(new Vector3((vec3[numbers[i + 2]].X + vec3[numbers[i + 1]].X) / 2,
                        (vec3[numbers[i + 2]].Y + vec3[numbers[i + 1]].Y) / 2,
                        (vec3[numbers[i + 2]].Z + vec3[numbers[i + 1]].Z) / 2));

                    bi = ind;
                    a.Add(new int[3] { numbers[i + 2], numbers[i + 1], ind++ });
                }


                bo = true;
                foreach (int[] m in a)
                    if (m[0] == numbers[i] && m[1] == numbers[i + 2] || m[0] == numbers[i + 2] && m[1] == numbers[i])
                    {
                        bo = false;
                        ci = m[2];
                        break;
                    }

                if (bo)
                {
                    vec32.Add(new Vector3((vec3[numbers[i]].X + vec3[numbers[i + 2]].X) / 2,
                        (vec3[numbers[i]].Y + vec3[numbers[i + 2]].Y) / 2,
                        (vec3[numbers[i]].Z + vec3[numbers[i + 2]].Z) / 2));

                    ci = ind;
                    a.Add(new int[3] { numbers[i], numbers[i + 2], ind++ });
                }

                numbersNew.Add(numbers[i]);
                numbersNew.Add(ai);
                numbersNew.Add(ci);

                numbersNew.Add(numbers[i + 1]);
                numbersNew.Add(bi);
                numbersNew.Add(ai);

                numbersNew.Add(numbers[i + 2]);
                numbersNew.Add(ci);
                numbersNew.Add(bi);

                numbersNew.Add(ai);
                numbersNew.Add(bi);
                numbersNew.Add(ci);


            }

            numbers = numbersNew.ToArray();
            vec3 = vec32.ToArray();
        }

        void Rounding()
        {
            float R = 0.95105654f;
            float k;
            for (int i = 0; i < vec3.Length; i++)
            {
                k = (float)(R / Math.Sqrt(vec3[i].X * vec3[i].X + vec3[i].Y * vec3[i].Y + vec3[i].Z * vec3[i].Z));
                vec3[i].X *= k;
                vec3[i].Y *= k;
                vec3[i].Z *= k;
            }
        }

        void SetGraph()
        {
            graph = new int[triangles.Length][];
            foreach (Triangle t in triangles)
            {
                List<int> list= new List<int>();
                foreach (Triangle t2 in triangles)
                {
                    int n=0;
                    if (t.triangleVerts[0].Position == t2.triangleVerts[0].Position || 
                        t.triangleVerts[0].Position == t2.triangleVerts[1].Position || 
                        t.triangleVerts[0].Position == t2.triangleVerts[2].Position)
                        n++;
                    if (t.triangleVerts[1].Position == t2.triangleVerts[0].Position ||
                        t.triangleVerts[1].Position == t2.triangleVerts[1].Position ||
                        t.triangleVerts[1].Position == t2.triangleVerts[2].Position)
                        n++;
                    if (t.triangleVerts[2].Position == t2.triangleVerts[0].Position ||
                        t.triangleVerts[2].Position == t2.triangleVerts[1].Position ||
                        t.triangleVerts[2].Position == t2.triangleVerts[2].Position)
                        n++;
                    if (n == 2)
                        list.Add(t2.index);
                }

                graph[t.index] = list.ToArray();
            }
        }

      /*  void Go(Color color)
        {
            List<Triangle> queue = new List<Triangle>();
            queue.Add(triangles[0]);
            Color colorFrom= triangles[0].Color;
            triangles[0].Color=color;
            if (color==colorFrom)
                queue.RemoveAt(0);

            while (queue.Count > 0)
            {
                int index = queue[0].index;
                queue.RemoveAt(0);
                foreach (int a in graph[index])
                {
                    if (triangles[a].Color == colorFrom)
                    {
                        triangles[a].Color = color;
                        queue.Add(triangles[a]);
                    }
                }
            }



            MakeArray();
        }

       */ 
       
        public void Go(int colorIndex)
        {
            Color colorFrom = triangles[0].Color;
            if (colors[colorIndex] != colorFrom)
            {
                step++;

                List<Triangle> queue = new List<Triangle>();

                foreach (Triangle t in includedTriangles)
                {
                    foreach (int a in graph[t.index])
                    {
                        if(triangles[a].Color==colors[colorIndex])
                        {
                            queue.Add(triangles[a]);
                            triangles[a].Color=colorFrom;
                        }
                    }
                }

                while(queue.Count>0)
                {
                    foreach (int a in graph[queue[0].index])
                    {
                        if (triangles[a].Color == colors[colorIndex])
                        {
                            queue.Add(triangles[a]);
                            triangles[a].Color = colorFrom;
                        }
                    }

                    includedTriangles.Add(queue[0]);
                    queue.RemoveAt(0);
                }

                foreach (Triangle t in includedTriangles)
                    t.Color = colors[colorIndex];

                occupiedCount = includedTriangles.Count;
                MakeScoreString();
                if (occupiedCount == triangles.Length)
                    end = 1;
                else if (step == stepMax)
                    end = -1;
                MakeArray();
            }
        }

        int SetStepMax()
        {
           //Надо б написать
            return 40;
        } 

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("ScoreFont");
            base.LoadContent();
        }

        float rotX,rotY,rotZ;
        int xi, yi;
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                mouse = Mouse.GetState();
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
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            //((Game1)Game).effect.World = Matrix.Identity * Matrix.CreateRotationX(rotY) * Matrix.CreateRotationY(-rotX) * Matrix.CreateRotationZ(rotZ);
            ((Game1)Game).effect.World = Matrix.CreateFromYawPitchRoll(-rotX, -rotY, rotZ);
            foreach (EffectPass pass in ((Game1)Game).effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //границы  не работают
                //GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                //  trianglesBorders, 0, trianglesBorders.Length / 2);

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, array, 0, array.Length / 3);

              
            }

            if (isActive)
            {
                Game1.spriteBatch.Begin();
                Game1.spriteBatch.DrawString(font, score, scorePosition, Color.White);
                Game1.spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}

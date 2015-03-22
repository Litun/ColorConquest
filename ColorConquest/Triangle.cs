using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ColorConquest
{
    class Triangle :IComparable
    {
        static int staticIndex = 0;
        public VertexPositionColor[] triangleVerts;
        public int index;
        Color color;

        public Color Color 
        { 
            get { return color; }
            set 
            { 
                color = value;
                triangleVerts[0].Color = color;
                triangleVerts[1].Color = color;
                triangleVerts[2].Color = color;
            }
        }

        public Triangle(Vector3 a, Vector3 b, Vector3 c, Color color)
        {
            index = staticIndex++;
            triangleVerts = new VertexPositionColor[3];
            triangleVerts[0] = new VertexPositionColor(a, color);
            triangleVerts[1] = new VertexPositionColor(b, color);
            triangleVerts[2] = new VertexPositionColor(c, color);
            this.color = color;
        }

        public int CompareTo(object obj)
        {
            return index.CompareTo(((Triangle)obj).index);
        }
    }
}

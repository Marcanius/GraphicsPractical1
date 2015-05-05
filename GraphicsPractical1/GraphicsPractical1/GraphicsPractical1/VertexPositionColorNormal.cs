using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GraphicsPractical1
{
    struct VertexPositionColorNormal : IVertexType
    {
        /// <summary>
        /// 
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// 
        /// </summary>
        public Color Color;
        /// <summary>
        /// 
        /// </summary>
        public Vector3 Normal;

        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = normal;
        }

        public static VertexElement[] VertexElements = 
        {
            new VertexElement(0,VertexElementFormat.Vector3,VertexElementUsage.Position,0),
            new VertexElement(sizeof(float)*3,VertexElementFormat.Color,VertexElementUsage.Color,0),
            new VertexElement(sizeof(float)*3+4,VertexElementFormat.Vector3,VertexElementUsage.Normal,0),
        };

        public static readonly VertexDeclaration VertexDeclaration = 
            new VertexDeclaration(VertexPositionColorNormal.VertexElements);

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexPositionColorNormal.VertexDeclaration; }
        }
    }
}

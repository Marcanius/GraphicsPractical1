using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GraphicsPractical1
{
    struct VertexPositionColorNormal : IVertexType
    {
        #region Variables

        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        #endregion

        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = normal;
        }

        // Explains to the graphics card how much, and what kind of memory to set aside for each vertex.
        public static VertexElement[] VertexElements = 
        {
            new VertexElement(0,VertexElementFormat.Vector3,VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3,VertexElementFormat.Color,VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4,VertexElementFormat.Vector3,VertexElementUsage.Normal, 0),
        };

        // Creates the vertex declaration, using the size of the VertexElements.
        public static readonly VertexDeclaration VertexDeclaration = 
            new VertexDeclaration(VertexPositionColorNormal.VertexElements);

        // Returns the VertexDeclaration.
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexPositionColorNormal.VertexDeclaration; }
        }
    }
}

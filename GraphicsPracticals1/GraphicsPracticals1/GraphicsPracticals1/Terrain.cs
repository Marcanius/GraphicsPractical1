using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    class Terrain
    {
        private int width, height;

        private VertexPositionColor[] vertices;

        public Terrain(float[,] heightData)
        {
            this.width = heightData.GetLength(0);
            this.height = heightData.GetLength(1);

            VertexPositionColor[] heightDataVertices = this.loadVertices();
            this.setupVertices(heightDataVertices);
        }

        public void Draw(GraphicsDevice device)
        {
            device.DrawUserPrimitives(PrimitiveType.TriangleList, this.vertices,
                0, this.vertices.Length / 3, VertexPositionColor.VertexDeclaration);
        }

        private VertexPositionColor[] loadVertices()
        {
            VertexPositionColor[] vertices = new VertexPositionColor[this.width * this.height];

            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                {
                    int v = x + y * this.width;

                    vertices[v].Position = new Vector3(x, 0, -y);
                    vertices[v].Color = Color.White;
                }

            return vertices;
        }

        private void setupVertices(VertexPositionColor[] heightDataVertices)
        {
            this.vertices = new VertexPositionColor[(this.width - 1) * (this.height - 1) * 3];

            int counter = 0;

            for (int x = 0; x < this.width - 1; x++)
                for (int y = 0; y < this.height - 1; y++)
                {
                    int lowerLeft = x + y * this.width;
                    int lowerRight = (x + 1) + y * this.width;
                    int topLeft = x + (y + 1) * this.width;
                    int topRight = (x + 1) + (y + 1) * this.width;

                    this.vertices[counter++] = heightDataVertices[topLeft];
                    this.vertices[counter++] = heightDataVertices[lowerRight];
                    this.vertices[counter++] = heightDataVertices[lowerLeft];
                }
        }

        public int Width
        {
            get { return this.width; }
        }

        public int Height
        {
            get { return this.height; }
        }
    }
}

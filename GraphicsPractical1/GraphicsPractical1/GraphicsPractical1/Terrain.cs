using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    partial class Terrain
    {
        #region Variables
        private int width, height;
        private short[] indices;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private VertexPositionColorNormal[] vertices;

        #endregion

        public Terrain(HeightMap heightMap, float heightScale, GraphicsDevice device)
        {
            this.width = heightMap.Width;
            this.height = heightMap.Height;

            this.vertices = this.loadVertices(heightMap, heightScale);
            this.setupIndices();
            this.calculateNormals();
            this.calculateAverages();

            this.copyToBuffers(device);
        }

        #region Methods

        public void Draw(GraphicsDevice device)
        {
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this.vertices.Length, 0, this.indices.Length / 3);
        }

        /// <summary>
        /// Creates the array of vertices and fills it.
        /// </summary>
        /// <param name="heightMap"> The heightmap used to calculate the y position. </param>
        /// <param name="heightScale"> The float with which to scale te height. </param>
        /// <returns></returns>
        private VertexPositionColorNormal[] loadVertices(HeightMap heightMap, float heightScale)
        {
            VertexPositionColorNormal[] vertices = new VertexPositionColorNormal[this.width * this.height];

            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                {
                    int v = x + y * this.width;
                    float h = heightMap[x, y] * heightScale;

                    vertices[v].Position = new Vector3(x, h, -y);
                    vertices[v].Color = calculateColor(heightMap[x, y]);
                }

            return vertices;
        }

        private void setupIndices()
        {
            this.indices = new short[(this.width - 1) * (this.height - 1) * 6];

            int counter = 0;

            for (int x = 0; x < this.width - 1; x++)
                for (int y = 0; y < this.height - 1; y++)
                {
                    int lowerLeft = x + y * this.width;
                    int lowerRight = (x + 1) + y * this.width;
                    int topLeft = x + (y + 1) * this.width;
                    int topRight = (x + 1) + (y + 1) * this.width;

                    this.indices[counter++] = (short)topLeft;
                    this.indices[counter++] = (short)lowerRight;
                    this.indices[counter++] = (short)lowerLeft;

                    this.indices[counter++] = (short)topLeft;
                    this.indices[counter++] = (short)topRight;
                    this.indices[counter++] = (short)lowerRight;
                }
        }

        private void calculateNormals()
        {
            for (int i = 0; i < this.indices.Length / 3; i++)
            {
                short i1 = this.indices[i * 3];
                short i2 = this.indices[i * 3 + 1];
                short i3 = this.indices[i * 3 + 2];

                Vector3 side1 = this.vertices[i3].Position - this.vertices[i1].Position;
                Vector3 side2 = this.vertices[i2].Position - this.vertices[i1].Position;

                Vector3 normal = Vector3.Cross(side1, side2);
                normal.Normalize();

                this.vertices[i1].Normal += normal;
                this.vertices[i2].Normal += normal;
                this.vertices[i3].Normal += normal;
            }

            for (int i = 0; i < this.vertices.Length; i++)
                this.vertices[i].Normal.Normalize();
        }

        // The copyToBuffers method. Used to store all the vertices and indices in the GPU memory.
        private void copyToBuffers(GraphicsDevice device)
        {
            this.vertexBuffer = new VertexBuffer(device, VertexPositionColorNormal.VertexDeclaration,
                this.vertices.Length, BufferUsage.WriteOnly);
            this.vertexBuffer.SetData(this.vertices);

            this.indexBuffer = new IndexBuffer(device, typeof(short), this.indices.Length,
                BufferUsage.WriteOnly);
            this.indexBuffer.SetData(this.indices);

            device.Indices = this.indexBuffer;
            device.SetVertexBuffer(this.vertexBuffer);
        }

        #endregion
        #region Properties

        // Property to return the width.
        public int Width
        {
            get { return this.width; }
        }

        // Property to return the height.
        public int Height
        {
            get { return this.height; }
        }

        #endregion
    }
}

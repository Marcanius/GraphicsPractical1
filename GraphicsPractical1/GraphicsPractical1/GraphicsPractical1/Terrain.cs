﻿using System;
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
        private short[] indices;

        private VertexPositionColorNormal[] vertices;

        public Terrain(HeightMap heightMap, float heightScale)
        {
            this.width = heightMap.Width;
            this.height = heightMap.Height;

            this.vertices = this.loadVertices(heightMap, heightScale);
            this.setupIndices();
        }

        public void Draw(GraphicsDevice device)
        {
            device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.vertices,
                0, this.vertices.Length, this.indices, 0, this.indices.Length / 3);
        }

        private VertexPositionColorNormal[] loadVertices(HeightMap heightMap, float heightScale)
        {
            VertexPositionColorNormal[] vertices = new VertexPositionColorNormal[this.width * this.height];

            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                {
                    int v = x + y * this.width;
                    float h = heightMap[x, y] * heightScale;

                    vertices[v].Position = new Vector3(x, h, -y);
                    vertices[v].Color = Color.Green;
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
            for (int i = 0; i < this.vertices.Length / 3; i++)
            {
                VertexPositionColorNormal v1 = this.vertices[i * 3];
                VertexPositionColorNormal v2 = this.vertices[i * 3 + 1];
                VertexPositionColorNormal v3 = this.vertices[i * 3 + 2];

                Vector3 side1 = v3.Position - v1.Position;
                Vector3 side2 = v2.Position - v1.Position;

                Vector3 normal = Vector3.Cross(side1, side2);
                normal.Normalize();

                this.vertices[i * 3].Normal = normal;
                this.vertices[i * 3 + 1].Normal = normal;
                this.vertices[i * 3 + 2].Normal = normal;
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

﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    class HeightMap
    {
        #region Variables

        // The variables
        private int width, height;
        private byte[,] heightData;

        #endregion

        // The construct method of the HeightMap class.
        public HeightMap(Texture2D heightMap)
        {
            this.width = heightMap.Width;
            this.height = heightMap.Height;

            this.loadHeightData(heightMap);
        }

        #region Methods

        /// <summary>
        /// The loadHeightData method. This method is used to get the color from the texture and fill the heightData.
        /// </summary>
        /// <param name="heightMap"></param>
        private void loadHeightData(Texture2D heightMap)
        {
            this.heightData = new byte[this.width, this.height];
            
            Color[] colorData = new Color[this.width * this.height];
            heightMap.GetData(colorData);
            
            // For loop in a for loop to check every pixel in the texture.
            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                    this.heightData[x, y] = colorData[x + y * this.width].R;
        }

        #endregion

        #region properties

        // Property to return or set the heightData byte array element.
        public byte this[int x, int y]
        {
            get { return this.heightData[x, y]; }
            set { this.heightData[x, y] = value; }
        }
        
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

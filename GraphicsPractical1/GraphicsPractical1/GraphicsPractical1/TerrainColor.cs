using Microsoft.Xna.Framework;

namespace GraphicsPractical1
{
    /// <summary>
    /// The Methods that determine the color of the terrain.
    /// </summary>
    partial class Terrain
    {
        #region Methods

        /// <summary>
        /// Determines the first run of colors.
        /// </summary>
        /// <param name="height"> The height of a given vertex. </param>
        /// <returns> The color corresponding to the given height. </returns>
        private Color calculateColor(float height)
        {
            if (height <= 51)
                return Color.Blue;
            else if (height < 58)
                return new Color(73, 120, 3);
            else if (height < 116)
                return new Color(202, 147, 8);
            else if (height < 162)
                return new Color(132, 99, 0);
            else if (height < 170)
                return new Color(212, 159, 0);
            else if (height < 183)
                return new Color(178, 146, 63);
            else if (height < 198)
                return new Color(197, 151, 106);
            else
                return new Color(239, 210, 130);

            
        }

        /// <summary>
        /// Makes the transition between different colored vertices smoother by averaging them out.
        /// </summary>
        private void calculateAverages()
        {
            for (int j = 0; j < 2; j++)
            {
                for (int x = 1; x < width - 1; x++)
                    for (int y = 1; y < height - 1; y++)
                    {
                        // Get all 9 surrounding indices.
                        int[] ints = new int[9];

                        ints[0] = (x - 1) + (y - 1) * this.width;
                        ints[1] = x + (y - 1) * this.width;
                        ints[2] = (x + 1) + (y - 1) * this.width;
                        ints[3] = (x - 1) + y * this.width;
                        ints[4] = x + y * this.width;
                        ints[5] = (x + 1) + y * this.width;
                        ints[6] = (x - 1) + (y + 1) * this.width;
                        ints[7] = x + (y + 1) * this.width;
                        ints[8] = (x + 1) + (y + 1) * this.width;

                        int red = 0;
                        int green = 0;
                        int blue = 0;

                        // Get the color of all surrounding colors.
                        for (int i = 0; i < 9; i++)
                        {
                            red += vertices[ints[i]].Color.R;
                            green += vertices[ints[i]].Color.G;
                            blue += vertices[ints[i]].Color.B;
                        }

                        // Average them.
                        red = red / 9;
                        green = green / 9;
                        blue = blue / 9;

                        // Apply to the colors.
                        vertices[x + y * width].Color = new Color(red, green, blue);
                    }
            
            }
        }

        #endregion
    }
}

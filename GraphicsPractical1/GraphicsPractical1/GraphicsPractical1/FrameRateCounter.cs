using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    class FrameRateCounter : DrawableGameComponent
    {
        /// <summary>
        /// The integer containing the number of frames drawn in the last second.
        /// </summary>
        int frameRate; 
        /// <summary>
        /// The integer counting the number of frames drawn in the last second.
        /// </summary>
        int frameCounter; 
        /// <summary>
        /// The integer counting the number of seconds passed since the last 
        /// </summary>
        int secondsPassed;

        /// <summary>
        public FrameRateCounter(Game game)
            : base(game)
        {
            frameRate = 0;
            frameCounter = 0;
            secondsPassed = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (secondsPassed != gameTime.TotalGameTime.Seconds)
            {
                frameRate = frameCounter;
                secondsPassed = gameTime.TotalGameTime.Seconds;
                frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
        }

        public int FrameRate
        {
            get { return frameRate; }
        }
    }
}

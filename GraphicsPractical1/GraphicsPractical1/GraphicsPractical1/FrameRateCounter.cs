using Microsoft.Xna.Framework;

namespace GraphicsPractical1
{
    class FrameRateCounter : DrawableGameComponent
    {
        #region Variables

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

        #endregion

        /// <summary>
        public FrameRateCounter(Game game)
            : base(game)
        {
            frameRate = 0;
            frameCounter = 0;
            secondsPassed = 0;
        }

        #region Methods

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

        #endregion
        #region Properties

        public int FrameRate
        {
            get { return frameRate; }
        }

        #endregion
    }
}

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
        /// The integer counting the number of seconds passed since the last Draw call.
        /// </summary>
        int secondsPassed;

        #endregion

        public FrameRateCounter(Game game)
            : base(game)
        {
            frameRate = 0;
            frameCounter = 0;
            secondsPassed = 0;
        }

        #region Methods

        /// <summary>
        /// Updates the framerate.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (secondsPassed != gameTime.TotalGameTime.Seconds)
            {
                frameRate = frameCounter;
                secondsPassed = gameTime.TotalGameTime.Seconds;
                frameCounter = 0;
            }
        }

        /// <summary>
        /// Increases the framecounter. Obvs.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A property to return the framerate.
        /// </summary>
        public int FrameRate
        {
            get { return frameRate; }
        }

        #endregion
    }
}

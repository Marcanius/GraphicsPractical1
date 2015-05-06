using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


namespace GraphicsPractical1
{
    public class Camera
    {
        #region Variables

        #region Matrices
        /// <summary>
        /// The Matrix that tells us how we look at the world.
        /// </summary>
        private Matrix viewMatrix;
        /// <summary>
        /// The Matrix that tells us what part of our surroundings we render.
        /// </summary>
        private Matrix projectionMatrix;
        #endregion

        #region Vector3s
        /// <summary>
        /// The Vector3 pointing up, used in the viewMatrix.
        /// </summary>
        private Vector3 up;
        /// <summary>
        /// The Vector3 pointing to the location of the camera.
        /// </summary>
        private Vector3 position;
        /// <summary>
        /// The Vector3 pointing to the point at which the camera is looking. Always at a distance of 1 from the camera.
        /// </summary>
        private Vector3 focus;
        /// <summary>
        /// The Vector3 pointing to the focus, relative to the camera. Is of normalised length. 
        /// </summary>
        private Vector3 relativeFocus;
        #endregion

        #region Angles
        /// <summary>
        /// Our horizontal angle.
        /// </summary>
        private float angleH;
        /// <summary>
        /// Our vertical angle.
        /// </summary>
        private float angleV;
        /// <summary>
        /// The difference in our horizontal angle.
        /// </summary>
        private float deltaAngleH;
        /// <summary>
        /// The difference in our vertical angle.
        /// </summary>
        private float deltaAngleV;
        #endregion

        #region Speeds
        /// <summary>
        /// The speed at which we move the camera.
        /// </summary>
        private readonly int moveSpeed = 20;
        /// <summary>
        /// The speed at which we turn the camera.
        /// </summary>
        private readonly float turnSpeed = 0.5f;
        #endregion

        #endregion

        public Camera(Vector3 camEye, Vector3 camUp, float aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.position = camEye;

            this.angleH = 7;
            this.angleV = -0.55f;
            this.UpdateFocus();

            this.updateViewMatrix();
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 300.0f);
        }

        #region Methods

        public void Update(GameTime gT)
        {
            // The time since the last update.
            float timeStep = (float)gT.ElapsedGameTime.TotalSeconds;
            deltaAngleH = 0;
            deltaAngleV = 0;

            // Checking for keyboard input and applying its interaction.
            KeyboardState kbState = Keyboard.GetState();

            // Check to see if the Left Arrow Key is pressed.
            if (kbState.IsKeyDown(Keys.Left))
                deltaAngleH += -turnSpeed * timeStep;
            // Check to see if the Right Arrow Key is pressed.
            if (kbState.IsKeyDown(Keys.Right))
                deltaAngleH += turnSpeed * timeStep;
            // Check to see of the Up Arrow Key is pressed.
            if (kbState.IsKeyDown(Keys.Up))
                deltaAngleV += -turnSpeed * timeStep;
            // Check to see if the Right Arrow Key is pressed.
            if (kbState.IsKeyDown(Keys.Down))
                deltaAngleV += turnSpeed * timeStep;

            // Check to see if the matrix needs to be adjusted with a new angle.
            if (deltaAngleH != 0 || deltaAngleV != 0)
            {
                angleH += deltaAngleH;
                angleV = MathHelper.Clamp(angleV - deltaAngleV, -MathHelper.PiOver2 + 0.01F, MathHelper.PiOver2 - 0.01F);

                UpdateFocus();
            }

            // Movements in the X,Z-plane.
            if (kbState.IsKeyDown(Keys.W))
                moveCamera(timeStep, new Vector3((float)Math.Cos(angleH), 0, (float)Math.Sin(angleH)) * moveSpeed);
            if (kbState.IsKeyDown(Keys.A))
                moveCamera(timeStep, new Vector3((float)Math.Sin(angleH), 0, -(float)Math.Cos(angleH)) * moveSpeed);
            if (kbState.IsKeyDown(Keys.S))
                moveCamera(timeStep, new Vector3(-(float)Math.Cos(angleH), 0, -(float)Math.Sin(angleH)) * moveSpeed);
            if (kbState.IsKeyDown(Keys.D))
                moveCamera(timeStep, new Vector3(-(float)Math.Sin(angleH), 0, (float)Math.Cos(angleH)) * moveSpeed);

            // Movement along the Y-axis.
            if (kbState.IsKeyDown(Keys.Space))
                moveCamera(timeStep, new Vector3(0, 1, 0) * moveSpeed);
            if (kbState.IsKeyDown(Keys.LeftShift))
                moveCamera(timeStep, new Vector3(0, -1, 0) * moveSpeed);
        }

        /// <summary>
        /// Moves the position of the camera, but not its orientation.
        /// </summary>
        /// <param name="timeStep"> The amount of seconds that passed since the last update. </param>
        /// <param name="direction"> The direction ion which we want to move the camera. </param>
        private void moveCamera(float timeStep, Vector3 direction)
        {
            Eye = Eye + direction * timeStep;
            Focus = Focus + direction * timeStep;
        }

        /// <summary>
        /// Updates the viewMatrix, using the position of the camera, the focus it looks at, and the axis which points upwards.
        /// </summary>
        private void updateViewMatrix()
        {
            this.viewMatrix = Matrix.CreateLookAt(this.position, this.focus, this.up);
        }

        /// <summary>
        /// Updates the focus of the camera, using both the horizontal and the vertical angle to place the focus at the correct position in front of the camera.
        /// </summary>
        private void UpdateFocus()
        {
            relativeFocus = new Vector3((float)Math.Cos(angleH), 0, (float)Math.Sin(angleH));
            relativeFocus = new Vector3(relativeFocus.X * (float)Math.Cos(angleV), (float)Math.Sin(angleV), relativeFocus.Z * (float)Math.Cos(angleV));

            Focus = Eye + relativeFocus;
        }

        #endregion

        #region Properties

        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }
        }

        public Matrix ProjectionMatrix
        {
            get { return this.projectionMatrix; }
        }

        public Vector3 Eye
        {
            get { return this.position; }
            set
            {
                this.position = value;
                this.updateViewMatrix();
            }
        }

        public Vector3 Focus
        {
            get { return this.focus; }
            set
            {
                this.focus = value;
                this.updateViewMatrix();
            }
        }

        #endregion
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


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
        /// 
        /// </summary>
        private Vector3 up;
        /// <summary>
        /// 
        /// </summary>
        private Vector3 eye;
        /// <summary>
        /// 
        /// </summary>
        private Vector3 focus;
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        private float deltaAngleH;
        /// <summary>
        /// 
        /// </summary>
        private float deltaAngleV;
        #endregion

        #region Speeds
        /// <summary>
        /// 
        /// </summary>
        private int moveSpeed = 20;
        /// <summary>
        /// 
        /// </summary>
        private float turnSpeed = 0.5f;
        #endregion

        #endregion

        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.eye = camEye;
            this.focus = camFocus;
            this.updateViewMatrix();
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 300.0f);
            angleH = 1;
            angleV = 0;

            UpdateFocus();
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
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.focus, this.up);
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
            get { return this.eye; }
            set
            {
                this.eye = value;
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


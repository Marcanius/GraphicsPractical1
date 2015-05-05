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
        private Matrix viewMatrix, projectionMatrix;
        private Vector3 up, eye, focus;
        Vector3 cameraReference;
        Matrix rotationMatrixH, rotationMatrixV;
        float angleH, deltaAngleH, angleV, deltaAngleV;
        Vector3 transformedReference;

        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.eye = camEye;
            this.focus = camFocus;
            this.updateViewMatrix();
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 300.0f);

            cameraReference = new Vector3(0, 0, 1);
        }

        public void Update(GameTime gT)
        {

            float timeStep = (float)gT.ElapsedGameTime.TotalSeconds;
            deltaAngleH = 0;
            deltaAngleV = 0;

            // Checking for keyboard input and applying its interaction.
            KeyboardState kbState = Keyboard.GetState();

            // Check to see if the left key is pressed.
            if (kbState.IsKeyDown(Keys.Left))
                deltaAngleH += -3 * timeStep;
            // Check to see if the right key is pressed.
            if (kbState.IsKeyDown(Keys.Right))
                deltaAngleH += 3 * timeStep;

            // Check to see if the matrix needs to be adjusted with the new angle.
            if (deltaAngleH != 0)
            {
                angleH += deltaAngleH;
                rotationMatrixH = Matrix.CreateRotationY(angleH);
            }

            // Check to see if the left key is pressed.
            if (kbState.IsKeyDown(Keys.Up))
                deltaAngleV += -3 * timeStep;
            // Check to see if the right key is pressed.
            if (kbState.IsKeyDown(Keys.Down))
                deltaAngleV += 3 * timeStep;

            // Check to see if the matrix needs to be adjusted with the new angle.
            if (deltaAngleV != 0)
            {
                angleV += deltaAngleV;
                rotationMatrixV = Matrix.CreateRotationX(angleV);
            }

            if (deltaAngleH != 0 || deltaAngleV != 0)
            {
                transformedReference = Vector3.Transform(cameraReference, rotationMatrixH);
                transformedReference = Vector3.Transform(transformedReference, rotationMatrixV);
                Focus = Eye + transformedReference;
            }

            // The four cardinal directions.
            if (kbState.IsKeyDown(Keys.W))
                moveCamera(timeStep, new Vector3(5, 0, 0));
            if (kbState.IsKeyDown(Keys.A))
                moveCamera(timeStep, new Vector3(0, 0, -5));
            if (kbState.IsKeyDown(Keys.S))
                moveCamera(timeStep, new Vector3(-5, 0, 0));
            if (kbState.IsKeyDown(Keys.D))
                moveCamera(timeStep, new Vector3(0, 0, 5));

            // Up and Down
            if (kbState.IsKeyDown(Keys.Space))
                moveCamera(timeStep, new Vector3(0, 5, 0));
            if (kbState.IsKeyDown(Keys.LeftShift))
                moveCamera(timeStep, new Vector3(0, -5, 0));
        }

        private void moveCamera(float timeStep, Vector3 direction)
        {
            Eye = Eye + direction * timeStep;
            Focus = Focus + direction * timeStep;
        }

        private void updateViewMatrix()
        {
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.focus, this.up);
        }

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
    }
}

